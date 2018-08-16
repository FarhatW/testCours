using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace jce.Server.UserModel.Core.Models
{
     [Table("Users")]
    public class User 
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }
         [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(255)]
        public string LoginName { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int IdCe { get; set; }
        public bool? IsActif { get; set; }
       
       //Adress
        public string Agency { get; set; }
        public string Service { get; set; }
        public string Society { get; set; }
        public string StreetNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string AddressExtra { get; set; }
        public DateTime CreatedDate { get; set; }
        
        [Required]
        public int  CreatedBy { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        
        public virtual ICollection<Child> Enfants { get; set; }
     
        public int? AccessNumber { get; set; }
        public bool IsLocked { get; set; }

        //TODO:to virefy
        public bool? IsGroupingFamily { get; set; }
        //a déplacer
        public bool? IsHomeDelivery { get; set; }

        public string CommercialUniqueCode { get; set; }
        
        public User()
        {
            Roles = new Collection<UserRole>();
            Enfants = new Collection<Child>();
        }
    }
}