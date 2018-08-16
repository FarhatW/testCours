using System.ComponentModel.DataAnnotations;

namespace jce.Server.UserModel.Controllers.Resources
{
    public partial class UserResource
    {
        public class  UserAddressResource{

            
            public string Agency { get; set; }
            public string Service { get; set; }
            public string Society { get; set; }
            
            public string StreetNumber { get; set; }
            
            [Required]
            public string Address1 { get; set; }
            public string Address2 { get; set; }

            [Required]
            
            public string PostalCode { get; set; }

            [Required]
        
            public string City { get; set; }
            public string AddressExtra { get; set; }
        }
    }
}