using BLL.Models;

namespace BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> GetUserByIdAsync(int userId);

    Task<UserModel?> GetUserByNameAsync(string username);

    Task<UserModel> CreateUserAsync(UserModel userRequest);

    Task<IEnumerable<UserModel>> GetAllUsersAsync();
}
