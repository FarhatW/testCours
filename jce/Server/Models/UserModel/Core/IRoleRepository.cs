using System.Collections.Generic;
using System.Threading.Tasks;
using jce.Server.UserModel.Core.Models;

namespace jce.Server.UserModel.Core
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleById(int id, bool includeRelated = true);
        Task<List<Role>> GetRoles();
        void Add(Role role);
        void Remove(Role role);
    }
}