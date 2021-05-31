using System.Threading.Tasks;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Data;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.Services;
using Sat.Recruitment.Test.Abstraction;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersControllerTest : ControllerBaseTest
    {
        private readonly IUserService _userService;

        public UsersControllerTest()
        {
            _userService = new UserService(Mapper, new UserMemoryCache());
        }

        [Fact]
        public async Task CreateUserAsync()
        {
            var userController = new UsersController(_userService);

            var result = await userController.CreateUser(
                "Mike",
                "mike@gmail.com",
                "Av. Juan G",
                "+349 1122354215",
                "Normal",
                124
            );

            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public async Task VerifyIfUserAlreadyExistsAsync()
        {
            var userController = new UsersController(_userService);

            var result = await userController.CreateUser(
                "Agustina",
                "Agustina@gmail.com",
                "Av. Juan G",
                "+349 1122354215",
                "Normal",
                124
            );


            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}