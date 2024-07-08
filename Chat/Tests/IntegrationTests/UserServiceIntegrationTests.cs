

using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using BLL.Services;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests
{
    public class UserServiceIntegrationTests
    {
        private readonly IMapper _mapper;
        private readonly ServiceProvider _serviceProvider;

        public UserServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SimpleChatContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserService, UserService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Chat.Mappings.AutoMapper());
            });

            _mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(_mapper);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_CreatesUser()
        {
            var userService = _serviceProvider.GetService<IUserService>();
            var userRequest = new UserModel
            {
                Email = "test@example.com",
                Username = "testUser",
                Password = "password"
            };

            var createdUser = await userService.CreateUserAsync(userRequest);
            var userService2 = _serviceProvider.GetService<IUserService>();
            var retrievedUser = await userService2.GetUserByIdAsync(createdUser.Id);

            Assert.NotNull(createdUser);
            Assert.Equal(userRequest.Email, createdUser.Email);
            Assert.Equal(userRequest.Username, createdUser.Username);
            Assert.NotNull(retrievedUser);
            Assert.Equal(createdUser.Id, retrievedUser.Id);
            Assert.Equal(userRequest.Email, retrievedUser.Email);
            Assert.Equal(userRequest.Username, retrievedUser.Username);
        }

        [Fact]
        public async Task GetUserByNameAsync_ValidUsername_ReturnsUser()
        {
            var userService = _serviceProvider.GetService<IUserService>();
            var user = new User
            {
                Email = "test@example.com",
                Username = "testUser",
                Password = "password"
            };

            var userRepository = _serviceProvider.GetService<IUserRepository>();
            await userRepository.AddAsync(user);
            await _serviceProvider.GetService<IUnitOfWork>().SaveAsync();

            var retrievedUser = await userService.GetUserByNameAsync(user.Username);

            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Username, retrievedUser.Username);
            Assert.Equal(user.Email, retrievedUser.Email);
        }
    }
}
