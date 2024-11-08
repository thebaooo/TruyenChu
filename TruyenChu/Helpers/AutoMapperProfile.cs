using AutoMapper;
using NuGet.Protocol;
using TruyenChu.Data;
using TruyenChu.ViewModels;

namespace TruyenChu.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {

            CreateMap<RegisterViewModels, User>();
        }
    }
}
