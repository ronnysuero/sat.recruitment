using System.Threading.Tasks;
using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Domain.Interfaces
{
    public interface IUserService
    {
        void AssignMoneyByUserType(UserModel user);
        Task<Result> AddAsync(UserModel model);
    }
}