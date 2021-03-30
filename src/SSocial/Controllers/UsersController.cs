using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public UsersController(RepositoryContext context,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<UserDetails>> GetUsers()
        {
            var usersWithRoles = _userManager.GetUsersInRoleAsync(Role.Supervisor)
                .Result.Select(c => new UserDetails
                {
                    Email = c.Email,
                    Role = Role.Supervisor,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    UserId = c.Id
                }).Concat(_userManager.GetUsersInRoleAsync(Role.Mechanic).Result
                    .Select(c => new UserDetails
                {
                    Email = c.Email,
                    Role = Role.Mechanic,
                    FirstName = c.FirstName,
                    UserId = c.Id
                }));

            return usersWithRoles.ToList();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDetails>> GetUser(Guid id)
        {
            var user = await  _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return BadRequest(new { Mesage = "User not found" });
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            return new UserDetails
            {
                Email = user.Email,
                Role = role,
                FirstName = user.FirstName,
                UserId = user.Id
            };
        }
        
        [HttpGet]
        [Route("Role/{name}")]
        public ActionResult<IEnumerable<UserDetails>> GetUsersInRole(string name)
        {
            var role = _roleManager.Roles.FirstOrDefault(e => e.Name == name);
            if( role == null)
                return new BadRequestObjectResult(new { Message = "Role not found" });
            return _userManager.GetUsersInRoleAsync(role.Name).Result.Select(c => new UserDetails
            {
                Email = c.Email,
                Role = role.Name,
                FirstName = c.FirstName,
                UserId = c.Id
            }).ToList();
        }
    }
}