using Marquise_Web.Data;
using System;
using Utilities.Convert;
using AutoMapper;
using Marquise_Web.Model.SiteModel;
using MArquise_Web.Model.CRM;
using Marquise_Web.UI.areas.CRM.Models;
using Marquise_Web.UI.Models;

namespace Utilities.Map
{
    public class SiteAutoMapperConfiguration : Profile
    {
        public SiteAutoMapperConfiguration()
        {
            CreateMap<MessageViewModel, MessageModel>()
            .ForMember(dest => dest.FilePath, opt => opt.Ignore()) // تبدیل File به FilePath
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => DateConvert.ConvertPersianToGregorian(src.Birthday))); // تبدیل Birthday از string به DateTime

        // در صورت نیاز، تبدیل برعکس هم می‌توانید انجام دهید.
        CreateMap<MessageModel, MessageViewModel>()
            .ForMember(dest => dest.File, opt => opt.Ignore())
            .ForMember(dest => dest.FileName, opt => opt.Ignore())
            .ForMember(dest => dest.FileType, opt => opt.Ignore())
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => DateConvert.GetPersianDateTimeString(src.Birthday))); // تبدیل DateTime به string
        }

    }
}
