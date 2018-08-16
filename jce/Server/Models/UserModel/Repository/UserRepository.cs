using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jce.Server.Helpers;
using jce.Server.Persistence;
using jce.Server.UserModel.Core;
using jce.Server.UserModel.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace jce.Server.UserModel.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly JceDbContext context;
        public UserRepository(JceDbContext context)
        {
            this.context = context;

        }

        public  async Task<User> Authenticate(string Email, string password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(password))
                return null;
 
            var user =   await context.Users
                .Include(r => r.Roles)
                    .ThenInclude(ur => ur.Role).SingleOrDefaultAsync(x => x.Email == Email);
 
            // check if username exists
            if (user == null)
                return null;
 
            // check if password is correct
            return !Helper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
 
            // authentication successful
        }
    

        public async Task<IEnumerable<User>> GetAll()
        {
            return  await context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id,  bool includeRelated = true)
        {
            

            if(!includeRelated)
                return await context.Users.FindAsync(id);

            return await context.Users
                .Include(r => r.Roles)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(v => v.Id == id);
        }
 
        public void Add(User user)
        {
            context.Users.Add(user);
        }
 
        public void Delete(User user)
        {
            context.Users.Remove(user);
        }
 
 
    }
    
    
}