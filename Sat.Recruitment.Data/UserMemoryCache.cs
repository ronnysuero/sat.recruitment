using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Data.Entities;
using Sat.Recruitment.Data.Interfaces;

namespace Sat.Recruitment.Data
{
    public class UserMemoryCache : IUserMemoryCache
    {
        private readonly string _path = $"{Directory.GetCurrentDirectory()}/Files/Users.txt";
        private ICollection<User> _data;

        public UserMemoryCache()
        {
            _ = FillAsync();
        }

        ICollection<User> IUserMemoryCache.Data => _data;

        public async Task FillAsync()
        {
            _data = new List<User>();

            var fileStream = new FileStream(_path, FileMode.Open);

            using var reader = new StreamReader(fileStream);

            while (reader.Peek() >= 0)
            {
                var line = (await reader.ReadLineAsync())?.Split(",");

                if (line is {Length: 6})
                    _data.Add(new User
                    {
                        Name = line[0],
                        Email = line[1],
                        Phone = line[2],
                        Address = line[3],
                        UserType = line[4],
                        Money = decimal.Parse(line[5])
                    });
            }
        }

        public async Task AddAsync(User user)
        {
            _data.Add(user);

            // Append text to an existing file named "Users.txt".
            await using var outputFile = new StreamWriter(_path, true);

            await outputFile.WriteLineAsync(
                $"\n{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserType},{user.Money}"
            );
        }

        public bool Any(Func<User, bool> predicate)
        {
            return _data.Any(predicate);
        }

        public bool Any()
        {
            return _data.Any();
        }
    }
}