using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Sat.Recruitment.Data.Entities;
using Sat.Recruitment.Data.Interfaces;
using Sat.Recruitment.Domain.Interfaces;
using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserMemoryCache _userMemoryCache;

        public UserService(IMapper mapper, IUserMemoryCache userMemoryCache)
        {
            _mapper = mapper;
            _userMemoryCache = userMemoryCache;
        }

        public void AssignMoneyByUserType(UserModel model)
        {
            switch (model.UserType?.ToLower())
            {
                case "normal":
                {
                    if (model.Money is > 10 and < 100)
                    {
                        var gif = model.Money * Convert.ToDecimal(0.8);
                        model.Money += model.Money * gif;
                    }

                    if (model.Money > 100)
                    {
                        //If new user is normal and has more than USD100
                        var gif = model.Money * Convert.ToDecimal(0.12);
                        model.Money += gif;
                    }

                    break;
                }
                case "superuser":
                {
                    if (model.Money > 100)
                    {
                        var gif = model.Money * Convert.ToDecimal(0.20);
                        model.Money += gif;
                    }

                    break;
                }
                case "premium":
                {
                    if (model.Money > 100)
                    {
                        var gif = model.Money * 2;
                        model.Money += gif;
                    }

                    break;
                }
            }
        }

        public async Task<Result> AddAsync(UserModel model)
        {
            var isDuplicated = _userMemoryCache.Any(item =>
                item.Email == model.Email || item.Phone == model.Phone ||
                item.Name == model.Name && item.Address == model.Address
            );

            if (isDuplicated)
            {
                Debug.WriteLine("The user is duplicated");

                return new Result
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }

            //assign money by user role
            AssignMoneyByUserType(model);

            var entity = _mapper.Map<User>(model);

            await _userMemoryCache.AddAsync(entity);

            Debug.WriteLine("User Created");

            return new Result
            {
                IsSuccess = true,
                Errors = "User Created"
            };
        }
    }
}