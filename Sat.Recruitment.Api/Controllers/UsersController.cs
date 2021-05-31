using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Domain;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Domain.Validators;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("/create-user")]
        /*
          I didn't change that to CreateUser(UserModel model) because is highly probably any app in frontend is consuming that endpoint and sending the value as query params
         */
        public async Task<Result> CreateUser(
            string name,
            string email,
            string address,
            string phone,
            string userType,
            decimal money
        )
        {
            var model = new UserModel
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = money
            };

            var results = await new UserValidator().ValidateAsync(model);

            if (!results.IsValid)
                return new Result
                {
                    IsSuccess = false,
                    Errors = string.Join(", ", results.Errors.Select(s => s.ErrorMessage))
                };

            return await _userService.AddAsync(model);
        }
    }
}