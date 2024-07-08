using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using Chat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;

        public ChatsController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] ChatViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chat = await _chatService.CreateChatAsync(_mapper.Map<ChatModel>(request));
            return Ok(_mapper.Map<ChatViewModel>(chat));
        }

        [HttpPost("{chatId}/users")]
        public async Task<IActionResult> AddUserToChat([FromBody] ChatViewModel request)
        {
            await _chatService.AddUserToChatAsync(request.Id, request.UserId);
            return Ok();
        }

        [HttpDelete("{chatId}")]
        public async Task<IActionResult> DeleteChat(int chatId, int userId)
        {
            await _chatService.DeleteChatAsync(chatId, userId);
            return NoContent();
        }
    }
}
