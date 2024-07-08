using AutoMapper;
using BLL.Models;
using BLL.Services.Interfaces;
using DAL.UnitOfWork;

namespace BLL.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MessageModel>> GetMessagesForChatAsync(int chatId)
    {
        return _mapper.Map<IEnumerable<MessageModel>>(await _unitOfWork.Messages.GetAllByChatIdAsync(chatId));
    }
}
