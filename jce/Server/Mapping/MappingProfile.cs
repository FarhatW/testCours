using System.Linq;
using AutoMapper;
using jce.Server.UserModel.Controllers.Resources;
using jce.Server.UserModel.Core.Models;
using static jce.Server.UserModel.Controllers.Resources.UserResource;

namespace jce.Server.Mapping
{
    public class MappingProfile: Profile
    {

    public MappingProfile()
        {
             //Domaine to API Resource

            CreateMap<Role, RoleResource>();

            CreateMap<User, UserResource>()
            .ForMember(u => u.Password, opt => opt.Ignore())
            .ForMember(u => u.Token, opt => opt.Ignore())
            .ForMember(adR => adR.Address, opt => opt.MapFrom(ad => new UserAddressResource{
                    Address1 = ad.Address1,
                    Address2 = ad.Address2,
                    Agency =ad.Agency,
                    Service = ad.Service,
                    Society = ad.Society,
                    PostalCode = ad.PostalCode,
                    City = ad.City,
                    StreetNumber = ad.StreetNumber,
                    AddressExtra =ad.AddressExtra
                    
                }))
   
            
             .ForMember(vr => vr.Roles, opt => opt.MapFrom(v => v.Roles.Select(vf => new Role{Id = vf.Role.Id, Name = vf.Role.Name})));


            CreateMap<User, SaveUserResource>()
            .ForMember(adR => adR.Address, opt => opt.MapFrom(ad => new UserAddressResource{
                    Address1 = ad.Address1,
                    Address2 = ad.Address2,
                    Agency =ad.Agency,
                    Service = ad.Service,
                    Society = ad.Society,
                    PostalCode = ad.PostalCode,
                    City = ad.City,
                    StreetNumber = ad.StreetNumber,
                    AddressExtra =ad.AddressExtra
                }))
  
          
             .ForMember(vr => vr.Roles, opt => opt.MapFrom(v => v.Roles.Select(vf => vf.RoleId)));


            //API Resource to Domaine
            CreateMap<RoleResource, Role>();

            CreateMap<SaveUserResource, User>()
            .ForMember(u => u.Id, opt => opt.Ignore())
            
            .ForMember(u => u.Address1, opt => opt.MapFrom(ur => ur.Address.Address1))
            .ForMember(u => u.Address2, opt => opt.MapFrom(ur => ur.Address.Address2))
            .ForMember(u => u.Agency, opt => opt.MapFrom(ur => ur.Address.Agency))
            .ForMember(u => u.Service, opt => opt.MapFrom(ur => ur.Address.Service))
            .ForMember(u => u.Society, opt => opt.MapFrom(ur => ur.Address.Society))
            .ForMember(u => u.PostalCode, opt => opt.MapFrom(ur => ur.Address.PostalCode))
            .ForMember(u => u.City, opt => opt.MapFrom(ur => ur.Address.City))
            .ForMember(u => u.StreetNumber, opt => opt.MapFrom(ur => ur.Address.StreetNumber))
            .ForMember(v => v.Roles, opt => opt.Ignore())
             .AfterMap((ur, u) => {
                        var removedFeatures = u.Roles.Where(f => !ur.Roles.Contains(f.RoleId)).ToList();
                        foreach (var f in removedFeatures){
                            u.Roles.Remove(f);
                        }
                            
                        var addedFeatures =  ur.Roles.Where(id => u.Roles.All(f => f.RoleId != id)).Select(id=> new UserRole{RoleId = id}).ToList();
                        foreach (var f in addedFeatures){
                            u.Roles.Add(f);
                        }
                           
                });
            

            
        }
    
        
    }
}