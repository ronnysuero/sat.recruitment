using AutoMapper;
using Sat.Recruitment.Data.Entities;
using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Test.Abstraction
{
    public abstract class ControllerBaseTest
    {
        protected ControllerBaseTest()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.CreateMap<User, UserModel>().ReverseMap(); }
            );

            Mapper = configuration.CreateMapper();
        }

        protected IMapper Mapper { get; }
    }
}