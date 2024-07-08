using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class MessageRepository : Repository<Message>, IMessageRepository
{
    private readonly DbSet<Message> _messageDbSet;
    public MessageRepository(SimpleChatContext dbContext) : base(dbContext)
    {
        _messageDbSet = dbContext.Set<Message>();
    }


    public async Task<IEnumerable<Message>> GetAllByChatIdAsync(int chatId)
    {
        return await _messageDbSet
                             .Where(m => m.ChatId == chatId)
                             .ToListAsync();
    }

    public async override Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _messageDbSet
            .Include(p => p.User)
            .Include(p => p.Chat)
            .ToListAsync();
    }

    public async override Task<Message?> GetByIdAsync(int id)
    {
        return await _messageDbSet
            .Include(p => p.User)
            .Include(p => p.Chat)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
