using Utilities.Convert;
using AutoMapper;
using Marquise_Web.UI.Models;
using Marquise_Web.Model.DTOs.SiteModel;
using Marquise_Web.UI.areas.CRM.Models;
using MArquise_Web.Model.DTOs.CRM;

namespace Utilities.Map
{
    public class UIAutoMapperConfiguration : Profile
    {
        public UIAutoMapperConfiguration()
        {
            CreateMap<MessageViewModel, MessageDTO>()
            .ForMember(dest => dest.FilePath, opt => opt.Ignore()) // تبدیل File به FilePath
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.ToPersianDateTimeString())); // تبدیل Birthday از string به DateTime

        // در صورت نیاز، تبدیل برعکس هم می‌توانید انجام دهید.
        CreateMap<MessageDTO, MessageViewModel>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.FileName, opt => opt.Ignore())
            .ForMember(dest => dest.FileType, opt => opt.Ignore())
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.ToPersianDateTimeString())); // تبدیل DateTime به string

            CreateMap<TicketDto, TicketVM>();
            CreateMap<StaffDto, StaffInfo>();
            CreateMap<TicketDetailDto, TicketDetailVm>();
            CreateMap<AnswerDto, ShowAnswerVM>();
            CreateMap<NewTicketVM, NewTicketDto>();
            CreateMap<AnswerVM, NewAnswerDto>();
            CreateMap<CloseTicket, CloseTicketDto>();

            CreateMap<AccountDto, AccountVM>();
            CreateMap<ContactDto, ContactVM>();

            CreateMap<ContractDto, ContractVM>();
            CreateMap<ContractDto, ContractDetailVm>()
                .ForMember(dest => dest.tamdidStatus, opt => opt.Ignore());

            CreateMap<InvoiceDto, InvoiceVM>();
            CreateMap<InvoiceDetailDto, InvoiceDetailVm>();
            CreateMap<PaymentDto, PaymentVM>();

            CreateMap<QuoteDto, QuoteVM>();
            CreateMap<QuoteDetailDto, QuoteDetailVm>();
        }

    }
    public static class UIDataMapper
    {
        public static IMapper Mapper { get; private set; }

        static UIDataMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {



                cfg.AddProfile<UIAutoMapperConfiguration>();
            });


            Mapper = config.CreateMapper();
        }
    }
}
