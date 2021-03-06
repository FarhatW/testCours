using System.ComponentModel.DataAnnotations.Schema;

namespace jce.Server.UserModel.Core.Models
{
     [Table("UserRoles")]
    public class UserRole
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}