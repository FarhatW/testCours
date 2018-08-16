using System.Collections.Generic;
using System.Threading.Tasks;
using jce.Server.UserModel.Controllers.Resources;
using jce.Server.UserModel.Core.Models;

namespace jce.Server.UserModel.Core
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id, bool includeRelated = true);
        void Add(User user);
        void Delete(User user);
    }
}