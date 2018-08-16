using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace jce.Server.UserModel.Core.Models
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}