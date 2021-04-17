using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SSocial.Hubs;


namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotifHub> _hubContext;
        private readonly IUserConnectionManager _userConnectionManager;

        public RequestController(
            RepositoryContext context, IMapper mapper,IUserConnectionManager userConnectionManager, IHubContext<NotifHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
        }

        // GET: api/Request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequest()
        {
            var requestList = await _context.Request.AsQueryable().ProjectTo<RequestDto>(_mapper.ConfigurationProvider).ToListAsync();
            return requestList;
        }

        [HttpGet("test")]
        public ActionResult SendNumber()
        {

            var onlineUsers = _userConnectionManager.GetOnlineUsers();
            return Ok(new {Message = "Hola"});
        }

        // GET: api/Request/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetRequest(Guid id)
        {
            var categoryMapper = await _context.Request
                .Where(e => e.RequestId == id)
                .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
                .FirstAsync();
            
            if (categoryMapper == null)
            {
                return NotFound();
            }

            return categoryMapper;
        }

        // PUT: api/Request/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(Guid id, RequestForCreationDto requestForCreation)
        {
            if (id != requestForCreation.Id)
            {
                return BadRequest();
            }

            var savedRequest = _mapper.Map<Request>(requestForCreation);

            var saveRequest = new Request
            {
                Description = requestForCreation.Description,
                Priority = requestForCreation.Priority,
                SupervisorId = requestForCreation.Supervisor,
                CreatedAt = requestForCreation.CreatedAt,
                RequestId = requestForCreation.Id
            };

            _context.Entry(saveRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Request
        [HttpPost]
        public async Task<ActionResult<RequestForCreationDto>> PostRequest(RequestForCreationDto requestForCreation)
        {
            var supervisor = HttpContext.User.FindFirst(ClaimTypes.Actor)?.Value;
            if (supervisor == null)
            {
                return BadRequest("");
            }

            var savedRequest = new Request
            {
                Description = requestForCreation.Description,
                ProblemCode = requestForCreation.ProblemCode,
                Log = await _context.Logs.FindAsync(requestForCreation.Log),
                Machine = await _context.Machines.FindAsync(requestForCreation.Machine),
                SupervisorId = Guid.Parse(supervisor),
                CreatedAt =  DateTime.Now,
                Priority = requestForCreation.Priority,
            };
            
            await _context.Request.AddAsync(savedRequest);
            await _context.SaveChangesAsync();

            var userToNotify = savedRequest.Log.MechanicId;
            var makeNotif = _userConnectionManager.GetUserConnections(userToNotify.ToString());
            foreach (var connection in makeNotif.Where(connection => !string.IsNullOrWhiteSpace(connection)))
            {
                await _hubContext.Clients.Client(connection).SendAsync("ReceiveNotification", savedRequest.RequestId);
            }
            
            requestForCreation.Id = savedRequest.RequestId;
            return CreatedAtAction("GetRequest", new { id = savedRequest.RequestId }, requestForCreation);
        }

        // DELETE: api/Request/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Request.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(Guid id)
        {
            return _context.Request.Any(e => e.RequestId == id);
        }
    }
    
}
