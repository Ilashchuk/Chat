using BLL.Models;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Chat;

public sealed class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserService _userService;

    public ChatHub(IChatService chatService, IUserService userService)
    {
        _chatService = chatService;
        _userService = userService;
    }

    public async Task AddToGroup(int groupId, int userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId.ToString());
        await _chatService.AddUserToChatAsync(groupId, userId);
    }

    public async Task RemoveFromGroup(int groupId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId.ToString());
    }

    public async Task SendMessageToGroup(MessageModel message)
    {
        var userEntity = await _userService.GetUserByIdAsync(message.UserId);

        var chat = await _chatService.GetChatByIdAsync(message.ChatId);
        if (chat == null)
        {
            throw new KeyNotFoundException($"Chat with ID {message.ChatId} does not exist");
        }

        var userInChat = chat.UserIds.Any(uc => uc == message.UserId);
        if (!userInChat)
        {
            throw new InvalidOperationException($"User with ID {message.UserId} is not a member of chat with ID {message.ChatId}");
        }

        var msg = await _chatService.SendMessageAsync(message);

        await Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", userEntity.Username, message, message.ChatId.ToString());
    }
}
