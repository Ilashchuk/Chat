using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using BLL.Services;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.IntegrationTests
{
    public class ChatServiceIntegrationTests
    {
        private readonly IMapper _mapper;
        private readonly ServiceProvider _serviceProvider;

        public ChatServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SimpleChatContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            serviceCollection.AddScoped<IChatRepository, ChatRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IChatService, ChatService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Chat.Mappings.AutoMapper());
            });

            _mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(_mapper);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task CreateChatAsync_ValidRequest_CreatesChat()
        {
            var chatService = _serviceProvider.GetService<IChatService>();

            var user = new User
            {
                Id = 1,
                Email = "test@example.com",
                Username = "testuser",
                Password = "password"
            };

            var userRepository = _serviceProvider.GetService<IUserRepository>();
            await userRepository.AddAsync(user);
            await _serviceProvider.GetService<IUnitOfWork>().SaveAsync();

            var chatRequest = new ChatModel
            {
                Name = "Test Chat",
                UserId = user.Id
            };

            var chat = await chatService.CreateChatAsync(chatRequest);

            Assert.NotNull(chat);
            Assert.Equal(chatRequest.Name, chat.Name);
            Assert.Equal(chatRequest.UserId, chat.UserId);
        }
    }
}
