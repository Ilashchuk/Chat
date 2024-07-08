using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Moq;

namespace Tests.ServicesTests
{
    public class ChatServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ChatService _chatService;

        public ChatServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _chatService = new ChatService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateChatAsync_ValidRequest_ReturnsChatModel()
        {
            var chatRequest = new ChatModel { Name = "Test Chat", UserId = 1 };
            var user = new User { Id = 1 };
            var chat = new DAL.Entities.Chat { Id = 1, Name = "Test Chat", UserId = 1 };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(1)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<DAL.Entities.Chat>(chatRequest)).Returns(chat);
            _unitOfWorkMock.Setup(u => u.Chats.AddAsync(chat)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ChatModel>(chat)).Returns(chatRequest);

            var result = await _chatService.CreateChatAsync(chatRequest);

            Assert.NotNull(result);
            Assert.Equal(chatRequest.Name, result.Name);
            Assert.Equal(chatRequest.UserId, result.UserId);
        }

        [Fact]
        public async Task CreateChatAsync_InvalidUserId_ThrowsKeyNotFoundException()
        {
            var chatRequest = new ChatModel { Name = "Test Chat", UserId = 99 };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(99)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _chatService.CreateChatAsync(chatRequest));
        }

        [Fact]
        public async Task AddUserToChatAsync_ValidRequest_AddsUserToChat()
        {
            var chatId = 1;
            var userId = 1;
            var chat = new DAL.Entities.Chat { Id = chatId, UsersChats = new List<UsersChats>() };
            var user = new User { Id = userId };

            _unitOfWorkMock.Setup(u => u.Chats.GetByIdAsync(chatId)).ReturnsAsync(chat);
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _unitOfWorkMock.Setup(u => u.Chats.Update(chat)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask).Verifiable();

            await _chatService.AddUserToChatAsync(chatId, userId);

            _unitOfWorkMock.Verify(u => u.Chats.Update(It.Is<DAL.Entities.Chat>(c => c.UsersChats.Any(uc => uc.UserId == userId))), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task SendMessageAsync_ValidRequest_ReturnsMessageModel()
        {
            var messageRequest = new MessageModel { ChatId = 1, UserId = 1, Text = "Test Message" };
            var chat = new DAL.Entities.Chat { Id = 1 };
            var user = new User { Id = 1 };
            var message = new Message { Id = 1, ChatId = 1, UserId = 1, Text = "Test Message" };

            _unitOfWorkMock.Setup(u => u.Chats.GetByIdAsync(messageRequest.ChatId)).ReturnsAsync(chat);
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(messageRequest.UserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Message>(messageRequest)).Returns(message);
            _unitOfWorkMock.Setup(u => u.Messages.AddAsync(message)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<MessageModel>(message)).Returns(messageRequest);

            var result = await _chatService.SendMessageAsync(messageRequest);

            Assert.NotNull(result);
            Assert.Equal(messageRequest.Text, result.Text);
        }

        [Fact]
        public async Task DeleteChatAsync_ValidRequest_DeletesChat()
        {
            var chatId = 1;
            var userId = 1;
            var chat = new DAL.Entities.Chat { Id = chatId, UserId = userId };

            _unitOfWorkMock.Setup(u => u.Chats.GetByIdAsync(chatId)).ReturnsAsync(chat);
            _unitOfWorkMock.Setup(u => u.Chats.DeleteByIdAsync(chatId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            await _chatService.DeleteChatAsync(chatId, userId);

            _unitOfWorkMock.Verify(u => u.Chats.DeleteByIdAsync(chatId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteChatAsync_UnauthorizedUser_ThrowsUnauthorizedAccessException()
        {
            var chatId = 1;
            var userId = 1;
            var unauthorizedUserId = 2;
            var chat = new DAL.Entities.Chat { Id = chatId, UserId = userId };

            _unitOfWorkMock.Setup(u => u.Chats.GetByIdAsync(chatId)).ReturnsAsync(chat);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _chatService.DeleteChatAsync(chatId, unauthorizedUserId));
        }
    }
}
