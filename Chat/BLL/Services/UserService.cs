using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.Entities;
using DAL.UnitOfWork;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserModel?> GetUserByIdAsync(int userId)
    {
        return _mapper.Map<UserModel>(await _unitOfWork.Users.GetByIdAsync(userId));
    }

    public async Task<UserModel?> GetUserByNameAsync(string username)
    {
        return _mapper.Map<UserModel>(await _unitOfWork.Users.GetUserByNameAsync(username));
    }

    public async Task<UserModel> CreateUserAsync(UserModel userRequest)
    {
        if (string.IsNullOrEmpty(userRequest.Email))
        {
            throw new ArgumentException("Email cannot be null or empty");
        }

        if (string.IsNullOrEmpty(userRequest.Username))
        {
            throw new ArgumentException("Username cannot be null or empty");
        }

        if (string.IsNullOrEmpty(userRequest.Password))
        {
            throw new ArgumentException("Password cannot be null or empty");
        }

        var user = _mapper.Map<User>(userRequest);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserModel>(user);
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        return _mapper.Map<IEnumerable<UserModel>>(await _unitOfWork.Users.GetAllAsync());
    }
}
