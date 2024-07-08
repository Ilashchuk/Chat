using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Chat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IChatService chatService, IMapper mapper)
        {
            _messageService = messageService;
            _chatService = chatService;
            _mapper = mapper;
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessagesForChat(int chatId)
        {
            return Ok(await _messageService.GetMessagesForChatAsync(chatId));
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageViewModel request)
        {
            var message = await _chatService.SendMessageAsync(_mapper.Map<MessageModel>(request));
            return Ok(message);
        }
    }
}
