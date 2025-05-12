using Marquise_Web.Data;
using System;
using AutoMapper;
using Marquise_Web.Model.DTOs.SiteModel;

namespace Utilities.Map
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<MessageDTO, Message>()
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<MessageContactDTO, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.RegisterDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Birthday, opt => opt.Ignore())
                .ForMember(dest => dest.FilePath, opt => opt.Ignore());

            CreateMap<Message, MessageContactDTO>()
                .ForMember(dest => dest.EmailBody, opt => opt.Ignore())
                .ForMember(dest => dest.EmailAddress, opt => opt.Ignore())
                .ForMember(dest => dest.EmailSubject, opt => opt.Ignore());
            CreateMap<Message, MessageDTO>()
                 .ForMember(dest => dest.EmailBody, opt => opt.Ignore())
                .ForMember(dest => dest.EmailAddress, opt => opt.Ignore())
                .ForMember(dest => dest.EmailSubject, opt => opt.Ignore());


        }
    }
    public static class DataMapper
    {
        public static IMapper Mapper { get; private set; }

        static DataMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<AutoMapperConfiguration>();
            });


            Mapper = config.CreateMapper();
        }
    }

}
