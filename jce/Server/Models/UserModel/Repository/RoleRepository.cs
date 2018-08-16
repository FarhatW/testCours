using System.Collections.Generic;
using System.Threading.Tasks;
using jce.Server.Persistence;
using jce.Server.UserModel.Core;
using jce.Server.UserModel.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace jce.Server.UserModel.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly JceDbContext context;
        public RoleRepository(JceDbContext context)
        {
            this.context = context;

        }

        public void Add(Role role)
        {
           context.Roles.Add(role);
        }

        public async Task<Role> GetRoleById(int id, bool includeRelated = true)
        {
            return await context.Roles.FindAsync(id);
        }

        public async Task<List<Role>> GetRoles()
        {
            return await context.Roles.ToListAsync();
        }

        public void Remove(Role role)
        {
           context.Roles.Remove(role);
        }
    }
}