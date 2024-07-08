using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly DbSet<User> _userDbSet;
    public UserRepository(SimpleChatContext dbContext) : base(dbContext)
    {
        _userDbSet = dbContext.Set<User>();
    }

    public async Task<User?> GetUserByNameAsync(string username)
    {
        return await _userDbSet.SingleOrDefaultAsync(x => x.Username == username);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _userDbSet
            .Include(p => p.UsersChats).ThenInclude(p => p.Chat)
            .ToListAsync();
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _userDbSet
            .Include(p => p.UsersChats).ThenInclude(p => p.Chat)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
