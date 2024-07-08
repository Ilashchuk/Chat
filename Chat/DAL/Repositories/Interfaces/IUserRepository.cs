using DAL.Entities;

namespace DAL.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByNameAsync(string username);
}
