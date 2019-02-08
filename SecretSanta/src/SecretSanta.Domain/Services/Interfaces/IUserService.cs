using SecretSanta.Domain.Models;
using System.Collections.Generic;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IUserService
    {
        User Find(int id);
        User AddUser(User user);

        User UpdateUser(User user);

        List<User> FetchAll();

        bool DeleteUser(int userId);
    }
}
