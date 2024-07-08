using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Moq;

namespace Tests.ServicesTests
{
    public class MessageServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly MessageService _messageService;

        public MessageServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _messageService = new MessageService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetMessagesForChatAsync_ValidChatId_ReturnsMessages()
        {
            var chatId = 1;
            var messages = new List<Message>
            {
                new Message { Id = 1, ChatId = chatId, UserId = 1, Text = "Message 1" },
                new Message { Id = 2, ChatId = chatId, UserId = 2, Text = "Message 2" }
            };
            var messageModels = new List<MessageModel>
            {
                new MessageModel { Id = 1, ChatId = chatId, UserId = 1, Text = "Message 1" },
                new MessageModel { Id = 2, ChatId = chatId, UserId = 2, Text = "Message 2" }
            };

            _unitOfWorkMock.Setup(u => u.Messages.GetAllByChatIdAsync(chatId)).ReturnsAsync(messages);
            _mapperMock.Setup(m => m.Map<IEnumerable<MessageModel>>(messages)).Returns(messageModels);

            var result = await _messageService.GetMessagesForChatAsync(chatId);

            Assert.NotNull(result);
            Assert.Equal(messageModels.Count, result.Count());
            Assert.Equal(messageModels, result);
        }

        [Fact]
        public async Task GetMessagesForChatAsync_InvalidChatId_ReturnsEmptyList()
        {
            var chatId = 1;
            var messages = new List<Message>();
            var messageModels = new List<MessageModel>();

            _unitOfWorkMock.Setup(u => u.Messages.GetAllByChatIdAsync(chatId)).ReturnsAsync(messages);
            _mapperMock.Setup(m => m.Map<IEnumerable<MessageModel>>(messages)).Returns(messageModels);

            var result = await _messageService.GetMessagesForChatAsync(chatId);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
