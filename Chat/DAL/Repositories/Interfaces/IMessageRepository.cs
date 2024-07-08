using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetAllByChatIdAsync(int chatId);
}
