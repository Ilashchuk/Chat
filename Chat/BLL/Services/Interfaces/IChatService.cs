using BLL.Models;
using DAL.Entities;

namespace BLL.Services.Interfaces;

public interface IChatService
{
    Task<ChatModel> CreateChatAsync(ChatModel chatRequest);

    Task AddUserToChatAsync(int chatId, int userId);

    Task<MessageModel> SendMessageAsync(MessageModel messageRequest);

    Task<ChatModel?> GetChatByIdAsync(int id);

    Task DeleteChatAsync(int chatId, int userId);
}
