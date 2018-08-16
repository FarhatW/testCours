using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using static jce.Server.UserModel.Controllers.Resources.UserResource;

namespace jce.Server.UserModel.Controllers.Resources
{
    public class SaveUserResource
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string LoginName { get; set; }

        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public UserAddressResource Address { get; set; }

        public int  CreatedBy { get; set; }
        public string CommercialUniqueCode { get; set; }
        public int IdCe { get; set; }
        public ICollection<int> Roles { get; set; }

        public SaveUserResource()
        {
            Roles = new Collection<int>();
        }
    }
}