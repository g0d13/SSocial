using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SSocial.Hubs;


namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager; 
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IMapper _mapper;

        public UsersController(RepositoryContext context,
            UserManager<User> userManager,
            IMapper mapper,
            IUserConnectionManager userConnectionManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userConnectionManager = userConnectionManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDetails>> GetUsers()
        {
            var supervisorUsers = _userManager.GetUsersInRoleAsync(Role.Supervisor)
                .Result.AsQueryable().ProjectTo<UserDetails>(_mapper.ConfigurationProvider).ToList();
            supervisorUsers.ForEach(c => c.Role = Role.Supervisor);

            var mechanicUsers = _userManager.GetUsersInRoleAsync(Role.Mechanic)
                .Result.AsQueryable().ProjectTo<UserDetails>(_mapper.ConfigurationProvider).ToList();
            mechanicUsers.ForEach(c => c.Role = Role.Mechanic);
            var users = supervisorUsers.Concat(mechanicUsers).ToList();
            return users;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDetails>> GetUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new {Mesage = "User not found"});
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            var returnUser = _mapper.Map<UserDetails>(user);
            returnUser.Role = role;

            return returnUser;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UserDetails>> UpdateUser(Guid id, RegisterUserDto userDetails)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.Email = userDetails.Email;
            user.UserName = userDetails.Email;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDetails.Password);
            await _userManager.UpdateAsync(user);
            var result = _mapper.Map<UserDetails>(user);
            return result;
        }

        [HttpGet]
        [Route("Role/{name}")]
        public ActionResult<IEnumerable<UserDetails>> GetUsersInRole(string name)
        {
            var role = _roleManager.Roles.FirstOrDefault(e => e.Name == name);
            if( role == null)
                return NotFound(new { Mesage = "Role not found" });
            var users = _userManager.GetUsersInRoleAsync(role.Name)
                .Result.AsQueryable().ProjectTo<UserDetails>(_mapper.ConfigurationProvider).ToList();
            users.ForEach(c => c.Role = role.Name);
            return users;
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("connected")]
        public ActionResult<IEnumerable<string>> GetConnectedUsers()
        {
            var connected = _userConnectionManager.GetOnlineUsers();
            return connected;
        }
    }
}