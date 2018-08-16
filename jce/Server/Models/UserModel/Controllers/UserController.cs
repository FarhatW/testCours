using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using jce.Server.Helpers;
using jce.Server.UserModel.Controllers.Resources;
using jce.Server.UserModel.Core;
using jce.Server.UserModel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace jce.Server.UserModel.Controllers
{
    // [Authorize]
    [Route("api/users/")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthSetting _optionsUser;


        public UserController(IMapper mapper, IUserRepository repository, IUnitOfWork unitOfWork, IOptions<AuthSetting> options)
        {
            this._optionsUser = options.Value;
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserResource userResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _repository.Authenticate(userResource.Email, userResource.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_optionsUser.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                  
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                
            };
          
           
            foreach (var item in user.Roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, item.Role.Name));
            }
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //user.Token = tokenString;
            

            // return basic user info (without password) and token to store client side
           // var claims =  tokenDescriptor.Subject.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            var result  = Mapper.Map<User, UserResource>(user);
            result.Token = tokenString;
            return Ok( result);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]SaveUserResource userResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
          
             // validation
            if (string.IsNullOrWhiteSpace(userResource.Password))
                return BadRequest("Password is required");

            if(userResource.CreatedBy <= 0)
                return BadRequest("CreatedBy is required");    
               
            var users = await _repository.GetAll();
            if (users.Any(x => x.Email == userResource.Email))
                return BadRequest("Email " + userResource.Email + " is already taken");
                
            // map dto to entity
            var user = _mapper.Map<SaveUserResource, User>(userResource);

            Helper.CreatePasswordHash(userResource.Password, out var passwordHash, out var passwordSalt);
 
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _repository.Add(user);
            await _unitOfWork.CompleteAsync();

           user = await _repository.GetById(user.Id);

            var result = _mapper.Map<User, UserResource>(user);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetAll();
            var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
                return NotFound();

            var result = _mapper.Map<User, UserResource>(user);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]UserResource userResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _repository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            var updatedUser = await Helper.UpdateUserDatas(userResource, user, _repository);
            // map dto to entity and set id
            _mapper.Map<UserResource, User>(userResource, updatedUser);

            await _unitOfWork.CompleteAsync();

            user = await _repository.GetById(user.Id);
            var result = _mapper.Map<User, UserResource>(user);

            return Ok(result);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repository.GetById(id);

            if(user == null){
                return NotFound();
            }
            _repository.Delete(user);
            await _unitOfWork.CompleteAsync();
            return Ok(id);
        }
    }
}