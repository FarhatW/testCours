using System.Collections.Generic;
using System.Collections.ObjectModel;
using jce.Server.UserModel.Core.Models;

namespace jce.Server.UserModel.Controllers.Resources
{
    public partial class UserResource
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LoginName { get; set; }
        public string Phone { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public int IdCe { get; set; }
        public bool? IsActif { get; set; }


        public bool? IsGroupingFamily { get; set; }
        public bool? IsHomeDelivery { get; set; }
        public int? AccessNumber { get; set; }
        public int  CreatedBy { get; set; }
        public bool IsLocked { get; set; }
        public string CommercialUniqueCode { get; set; }
        public UserAddressResource Address { get; set; }
        
       // public AdminResource AdminAttr { get; set; }

        public ICollection<Role> Roles { get; set; }
        public virtual ICollection<Child> Enfants { get; set; }
       
        public UserResource()
        {
            Roles = new Collection<Role>();
            Enfants = new Collection<Child>();
        }
    }
}