using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSocial.Data;
using SSocial.Dtos;


namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UsersController(DataContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
                    Username = c.UserName,
                    UserId = Guid.Parse(c.Id)
                }).Concat(_userManager.GetUsersInRoleAsync(Role.Mechanic).Result
                    .Select(c => new UserDetails
                {
                    Email = c.Email,
                    Role = Role.Mechanic,
                    Username = c.UserName,
                    UserId = Guid.Parse(c.Id)
                }));

            return usersWithRoles.ToList();
        }

        [HttpGet]
        [Route("{name}")]
        public ActionResult<IEnumerable<UserDetails>> GetUsersInRole(string name)
        {
            var role = _roleManager.Roles.FirstOrDefault(e => e.Name == name);
            if( role == null)
                return new BadRequestObjectResult(new { Message = "Role was not found" });
            return _userManager.GetUsersInRoleAsync(role.Name).Result.Select(c => new UserDetails
            {
                Email = c.Email,
                Role = role.Name,
                Username = c.UserName,
                UserId = Guid.Parse(c.Id)
            }).ToList();
        }
    }
}