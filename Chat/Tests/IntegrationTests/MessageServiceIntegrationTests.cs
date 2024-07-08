using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Services;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using DAL.Entities;

namespace Tests.IntegrationTests
{
    public class MessageServiceIntegrationTests
    {
        private readonly IMapper _mapper;
        private readonly ServiceProvider _serviceProvider;

        public MessageServiceIntegrationTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SimpleChatContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            serviceCollection.AddScoped<IMessageRepository, MessageRepository>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IMessageService, MessageService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Chat.Mappings.AutoMapper());
            });

            _mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(_mapper);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task GetMessagesForChatAsync_ValidChatId_ReturnsMessages()
        {
            var messageService = _serviceProvider.GetService<IMessageService>();
            var chatId = 1;

            var messages = new List<Message>
            {
                new Message { ChatId = chatId, UserId = 1, Text = "Hello", CreationDate = DateTime.UtcNow },
                new Message { ChatId = chatId, UserId = 2, Text = "Hi", CreationDate = DateTime.UtcNow }
            };

            var messageRepository = _serviceProvider.GetService<IMessageRepository>();
            foreach (var message in messages)
            {
                await messageRepository.AddAsync(message);
            }
            await _serviceProvider.GetService<IUnitOfWork>().SaveAsync();

            var retrievedMessages = await messageService.GetMessagesForChatAsync(chatId);

            Assert.NotNull(retrievedMessages);
            Assert.Equal(messages.Count, retrievedMessages.Count());
            Assert.Equal(messages.Select(m => m.Text), retrievedMessages.Select(mm => mm.Text));
        }

        [Fact]
        public async Task GetMessagesForChatAsync_InvalidChatId_ReturnsEmpty()
        {
            var messageService = _serviceProvider.GetService<IMessageService>();
            var invalidChatId = 999;

            var retrievedMessages = await messageService.GetMessagesForChatAsync(invalidChatId);

            Assert.NotNull(retrievedMessages);
            Assert.Empty(retrievedMessages);
        }
    }
}
