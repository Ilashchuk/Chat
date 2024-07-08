using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;

namespace BLL.Services;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ChatService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ChatModel> CreateChatAsync(ChatModel chatRequest)
    {
        if (string.IsNullOrEmpty(chatRequest.Name))
        {
            throw new ArgumentException("Chat name cannot be null or empty");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(chatRequest.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {chatRequest.UserId} does not exist");
        }

        var chat = _mapper.Map<Chat>(chatRequest);

        await _unitOfWork.Chats.AddAsync(chat);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ChatModel>(chat);
    }

    public async Task AddUserToChatAsync(int chatId, int userId)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId);
        if (chat == null)
        {
            throw new KeyNotFoundException($"Chat with ID {chatId} does not exist");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} does not exist");
        }

        var userChat = new UsersChats { ChatId = chatId, UserId = userId };

        if (chat.UsersChats.Any(uc => uc.UserId == userId))
        {
            throw new InvalidOperationException($"User with ID {userId} is already in the chat");
        }

        chat.UsersChats.Add(userChat);
        _unitOfWork.Chats.Update(chat);
        await _unitOfWork.SaveAsync();
    }

    public async Task<MessageModel> SendMessageAsync(MessageModel messageRequest)
    {
        if (string.IsNullOrEmpty(messageRequest.Text))
        {
            throw new ArgumentException("Message text cannot be null or empty");
        }

        var chat = await _unitOfWork.Chats.GetByIdAsync(messageRequest.ChatId);
        if (chat == null)
        {
            throw new KeyNotFoundException($"Chat with ID {messageRequest.ChatId} does not exist");
        }

        var user = await _unitOfWork.Users.GetByIdAsync(messageRequest.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {messageRequest.UserId} does not exist");
        }

        var message = _mapper.Map<Message>(messageRequest);

        await _unitOfWork.Messages.AddAsync(message);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<MessageModel>(message);
    }

    public async Task<ChatModel?> GetChatByIdAsync(int id)
    {
        return _mapper.Map<ChatModel>(await _unitOfWork.Chats.GetByIdAsync(id));
    }

    public async Task DeleteChatAsync(int chatId, int userId)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(chatId);

        if (chat == null)
        {
            throw new KeyNotFoundException($"Chat with ID {chatId} not found");
        }

        if (chat.UserId != userId)
        {
            throw new UnauthorizedAccessException($"User with ID {userId} is not authorized to delete chat with ID {chatId}");
        }

        await _unitOfWork.Chats.DeleteByIdAsync(chatId);
        await _unitOfWork.SaveAsync();
    }
}
