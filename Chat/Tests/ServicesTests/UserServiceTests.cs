using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Entities;
using DAL.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidId_ReturnsUser()
        {
            var userId = 1;
            var user = new User { Id = userId, Username = "testUser" };
            var userModel = new UserModel { Id = userId, Username = "testUser" };

            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserModel>(user)).Returns(userModel);
            
            var result = await _userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userModel.Id, result.Id);
            Assert.Equal(userModel.Username, result.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_InvalidId_ReturnsNull()
        {
            var userId = 1;
            _unitOfWorkMock.Setup(u => u.Users.GetByIdAsync(userId)).ReturnsAsync((User)null);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByNameAsync_ValidUsername_ReturnsUser()
        {
            var username = "testUser";
            var user = new User { Id = 1, Username = username };
            var userModel = new UserModel { Id = 1, Username = username };

            _unitOfWorkMock.Setup(u => u.Users.GetUserByNameAsync(username)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<UserModel>(user)).Returns(userModel);

            var result = await _userService.GetUserByNameAsync(username);

            Assert.NotNull(result);
            Assert.Equal(userModel.Id, result.Id);
            Assert.Equal(userModel.Username, result.Username);
        }

        [Fact]
        public async Task GetUserByNameAsync_InvalidUsername_ReturnsNull()
        {
            var username = "nonExistentUser";
            _unitOfWorkMock.Setup(u => u.Users.GetUserByNameAsync(username)).ReturnsAsync((User)null);

            var result = await _userService.GetUserByNameAsync(username);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ReturnsCreatedUser()
        {
            var userRequest = new UserModel { Email = "test@example.com", Username = "testUser", Password = "password" };
            var user = new User { Id = 1, Email = "test@example.com", Username = "testUser", Password = "password" };
            var createdUserModel = new UserModel { Id = 1, Email = "test@example.com", Username = "testUser" };

            _mapperMock.Setup(m => m.Map<User>(userRequest)).Returns(user);
            _unitOfWorkMock.Setup(u => u.Users.AddAsync(user)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<UserModel>(user)).Returns(createdUserModel);

            var result = await _userService.CreateUserAsync(userRequest);

            Assert.NotNull(result);
            Assert.Equal(createdUserModel.Id, result.Id);
            Assert.Equal(createdUserModel.Email, result.Email);
            Assert.Equal(createdUserModel.Username, result.Username);
        }

        [Fact]
        public async Task CreateUserAsync_EmptyEmail_ThrowsArgumentException()
        {
            var userRequest = new UserModel { Email = "", Username = "testUser", Password = "password" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(userRequest));
            Assert.Equal("Email cannot be null or empty", exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_EmptyUsername_ThrowsArgumentException()
        {
            var userRequest = new UserModel { Email = "test@example.com", Username = "", Password = "password" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(userRequest));
            Assert.Equal("Username cannot be null or empty", exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_EmptyPassword_ThrowsArgumentException()
        {
            var userRequest = new UserModel { Email = "test@example.com", Username = "testUser", Password = "" };

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateUserAsync(userRequest));
            Assert.Equal("Password cannot be null or empty", exception.Message);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@example.com", Username = "user1" },
                new User { Id = 2, Email = "user2@example.com", Username = "user2" }
            };
            var userModels = new List<UserModel>
            {
                new UserModel { Id = 1, Email = "user1@example.com", Username = "user1" },
                new UserModel { Id = 2, Email = "user2@example.com", Username = "user2" }
            };

            _unitOfWorkMock.Setup(u => u.Users.GetAllAsync()).ReturnsAsync(users);
            _mapperMock.Setup(m => m.Map<IEnumerable<UserModel>>(users)).Returns(userModels);

            var result = await _userService.GetAllUsersAsync();

            Assert.NotNull(result);
            Assert.Equal(userModels.Count, result.Count());
            Assert.Equal(userModels, result);
        }
    }
}
