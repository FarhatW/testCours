using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using jce.Server.Persistence;
using jce.Server.UserModel.Controllers.Resources;
using jce.Server.UserModel.Core;
using jce.Server.UserModel.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jce.Server.UserModel.Controllers
{
    [Route("/api/roles")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository respository;
        private readonly IUnitOfWork unitOfWork;

        private readonly JceDbContext context;
        private readonly IMapper mapper;
        public RoleController(JceDbContext context, IMapper mapper, IRoleRepository respository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.respository = respository;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await respository.GetRoles();
            if (roles == null)
                return NotFound();

            var result = mapper.Map<List<Role>, List<RoleResource>>(roles);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRolesById(int id)
        {
            var role = await respository.GetRoleById(id);
            if (role == null)
                return NotFound();

            var result = mapper.Map<Role, RoleResource>(role);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {

            var role = await respository.GetRoleById(id, includeRelated: false);
            if (role == null)
                return NotFound();

            respository.Remove(role);
            await unitOfWork.CompleteAsync();
            
            return Ok(id);
        }

        [Authorize(Roles = "ADMIN , COMMERCIAL")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]RoleResource roleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = mapper.Map<RoleResource, Role>(roleResource);
            respository.Add(role);

            await unitOfWork.CompleteAsync();

            role =  await respository.GetRoleById(role.Id);

            var result = mapper.Map<Role, RoleResource>(role);
            return Ok(result);
        }

    }
}