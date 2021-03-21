using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSocial.Data;
using SSocial.Dtos;
using SSocial.Models;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RequestController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequest()
        {
            var requestList = await _context.Request
                .AsQueryable()
                .AsSplitQuery()
                .Select(r => new RequestDto
                {
                    Description = r.Description,
                    Log = r.LogId,
                    Machine = r.MachineId,
                    Priority = r.Priority,
                    Supervisor = r.SupervisorId,
                    CreatedAt = r.CreatedAt,
                    ProblemCode = r.ProblemCode,
                    RequestId = r.RequestId
                }).ToListAsync();
            return requestList;
        }

        // GET: api/Request/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetRequest(Guid id)
        {
            var request = await _context.Request.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return new RequestDto
            {
                Description = request.Description,
                Priority = request.Priority,
                //TODO: finzalize
                // Supervisor = request.Supervisor.Id,
                CreatedAt = request.CreatedAt,
                RequestId = request.RequestId
            };
        }

        // PUT: api/Request/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(Guid id, RequestDto request)
        {
            if (id != request.RequestId)
            {
                return BadRequest();
            }

            var savedRequest = _mapper.Map<Request>(request);

            var saveRequest = new Request
            {
                Description = request.Description,
                Priority = request.Priority,
                //TODO: finalize
                // Supervisor = await _context.Users.FindAsync(request.Supervisor),
                CreatedAt = request.CreatedAt,
                RequestId = request.RequestId
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestDto>> PostRequest(RequestDto request)
        {
            var savedRequest = _mapper.Map<Request>(request);
            savedRequest.Log = await _context.Logs.FindAsync(request.Log);
            savedRequest.Machine = await _context.Machines.FindAsync(request.Machine);
            savedRequest.Supervisor = await _context.Users.FindAsync(request.Supervisor);
            savedRequest.CreatedAt = DateTime.Now;
            
            await _context.Request.AddAsync(savedRequest);
            await _context.SaveChangesAsync();

            request.RequestId = savedRequest.RequestId;
            return CreatedAtAction("GetRequest", new { id = savedRequest.RequestId }, request);
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
