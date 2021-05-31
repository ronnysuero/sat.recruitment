using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Data.Entities;

namespace Sat.Recruitment.Data.Interfaces
{
    public interface IUserMemoryCache
    {
        protected ICollection<User> Data { get; }
        Task FillAsync();
        Task AddAsync(User user);
        bool Any(Func<User, bool> predicate);
        bool Any();
    }
}