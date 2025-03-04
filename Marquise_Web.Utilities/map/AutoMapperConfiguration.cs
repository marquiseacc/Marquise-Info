using Marquise_Web.Data;
using System;
using Utilities.Convert;
using AutoMapper;
using Marquise_Web.Model.SiteModel;
using MArquise_Web.Model.CRM;

namespace Utilities.Map
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<MessageModel, Message>()
                .ForMember(dest => dest.Phonenumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MessageContactModel, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Birthday, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore());

            CreateMap<Message, MessageContactModel>();
            CreateMap<Message, MessageModel>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.MessageText));

            CreateMap<User, UserModel>()
            .ForMember(dest => dest.CRMId, opt => opt.MapFrom(src => src.CRMId ?? Guid.Empty)); 

        
            CreateMap<UserModel, User>()
                .ForMember(dest => dest.CRMId, opt => opt.MapFrom(src => src.CRMId));

        }

    }
}
