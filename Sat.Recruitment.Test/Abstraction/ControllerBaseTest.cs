using AutoMapper;

namespace Sat.Recruitment.Test.Abstraction
{
    public abstract class ControllerBaseTest
    {
        protected ControllerBaseTest()
        {
            Mapper = new MapperConfiguration(mc => mc.AddProfile(new Domain.AutoMapper())).CreateMapper();
        }

        protected IMapper Mapper { get; }
    }
}