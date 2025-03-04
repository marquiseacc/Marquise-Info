using AutoMapper;
using MArquise_Web.Model.CRM;
using Marquise_Web.UI.areas.CRM.Models;

namespace Utilities.Map
{
    public class CRMAutoMapperConfiguration : Profile
    {
        public CRMAutoMapperConfiguration()
        {
            CreateMap<UserModel, LoginViewModel>();

            CreateMap<LoginViewModel, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())  
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore()) 
                .ForMember(dest => dest.Name, opt => opt.Ignore()) 
                .ForMember(dest => dest.CRMId, opt => opt.Ignore()); 
        }
    }
}
