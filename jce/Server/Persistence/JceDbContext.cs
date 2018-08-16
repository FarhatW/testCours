using jce.Server.UserModel.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace jce.Server.Persistence
{
    public class JceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CE> CEs { get; set; }
        public DbSet<Child> Childs { get; set; }
        public DbSet<Role> Roles { get; set; }

         public JceDbContext(DbContextOptions<JceDbContext> options) 
          : base(options)
        {
        }

        protected   override   void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<UserRole>().HasKey(ur => new {ur.RoleId, ur.UserId });
        }
    }
}