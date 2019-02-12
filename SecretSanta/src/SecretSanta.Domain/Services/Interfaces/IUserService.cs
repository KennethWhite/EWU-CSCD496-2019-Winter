using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> FetchAll();
        Task<User> GetById(int id);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
    }
}
