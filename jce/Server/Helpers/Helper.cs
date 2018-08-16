using System;
using System.Linq;
using System.Threading.Tasks;
using jce.Server.Exeptions;
using jce.Server.UserModel.Controllers.Resources;
using jce.Server.UserModel.Core;
using jce.Server.UserModel.Core.Models;

namespace jce.Server.Helpers
{
    public static class Helper
    {
            public static async Task<User> UpdateUserDatas(UserResource userResource, User user, IUserRepository repository){
            
            var users = await repository.GetAll();
            if (userResource.Email != user.Email)
            {
                // username has changed so check if the new username is already taken
                if (users.Any(x => x.Email == userResource.Email))
                    throw new AppException("Username " + userResource.Email + " is already taken");
            }

             // update password if it was entered
            if (string.IsNullOrWhiteSpace(userResource.Password)) return user;
            CreatePasswordHash(userResource.Password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return user;

        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
 
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                {
                    return false;
                }
            }
 
            return true;
        }
    }
}