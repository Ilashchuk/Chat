using BLL.Models;

namespace BLL.Services.Interfaces;

public interface IMessageService
{
    Task<IEnumerable<MessageModel>> GetMessagesForChatAsync(int chatId);
}
