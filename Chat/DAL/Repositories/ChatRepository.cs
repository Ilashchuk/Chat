using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ChatRepository : Repository<Chat>, IChatRepository
{
    private readonly DbSet<Chat> _chatDbSet;
    public ChatRepository(SimpleChatContext dbContext) : base(dbContext)
    {
        _chatDbSet = dbContext.Set<Chat>();
    }

    public async override Task<Chat?> GetByIdAsync(int id)
    {
        return await _chatDbSet
            .Include(p => p.UsersChats).ThenInclude(p => p.Chat)
            .Include(p=> p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async override Task<IEnumerable<Chat>> GetAllAsync()
    {
        return await _chatDbSet
            .Include(p => p.UsersChats).ThenInclude(p => p.Chat)
            .Include(p => p.User)
            .ToListAsync();
    }
}
