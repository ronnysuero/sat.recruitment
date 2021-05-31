using AutoMapper;
using Sat.Recruitment.Data.Entities;
using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Domain
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }
    }
}