using AutoMapper;
using BLL.Models;
using Chat.ViewModels;
using DAL.Entities;

namespace Chat.Mappings;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<UserModel, User>()
            .ForMember(dest => dest.UsersChats, opt => opt.MapFrom(src => src.ChatIds.Select(p => new UsersChats { UserId = src.Id, ChatId = p }).ToList()))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.MessageIds.Select(p => new Message { Id = p }).ToList()));


        CreateMap<User, UserModel>()
            .ForMember(dest => dest.ChatIds, opt => opt.MapFrom(src => src.UsersChats.Select(r => r.ChatId).ToList()))
            .ForMember(dest => dest.MessageIds, opt => opt.MapFrom(src => src.Messages.Select(l => l.Id).ToList()));

        CreateMap<MessageModel, Message>().ReverseMap();

        CreateMap<ChatModel, DAL.Entities.Chat>()
            .ForMember(dest => dest.UsersChats, opt => opt.MapFrom(src => src.UserIds.Select(p => new UsersChats { UserId = p, ChatId = src.UserId }).ToList()));

        CreateMap<DAL.Entities.Chat, ChatModel>()
            .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.UsersChats.Select(r => r.UserId).ToList()))
            .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(r => new MessageModel { Id = r.Id, Text = r.Text, ChatId = r.ChatId, CreationDate = r.CreationDate, UserId = r.UserId }).ToList()));

        CreateMap<ChatModel, ChatViewModel>().ReverseMap();

        CreateMap<MessageModel, MessageViewModel>().ReverseMap();

        CreateMap<UserModel, UserViewModel>().ReverseMap();
    }
}
