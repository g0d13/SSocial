using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        public UsersController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public ActionResult<IEnumerable<UserDetails>> GetUsers()
        {
            var users = from b in _context.Users
                select new UserDetails()
                {
                    Email = b.Email,
                    UserId = Guid.Parse(b.Id),
                    Username = b.UserName,
                };
            return users.ToList();
        }
    }
}