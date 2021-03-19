using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly DataContext _context;

        public RequestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequest()
        {
            return await _context.Request.Select(c => new RequestDto
            {
                Description = c.Description,
                Priority = c.Priority,
                //TODO: Finalize this
                // Supervisor = c.Supervisor.Id,
                CreatedAt = c.CreatedAt,
                EditedAt = c.EditedAt,
                RequestId = c.RequestId
            }).ToListAsync();
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
                EditedAt = request.EditedAt,
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

            var saveRequest = new Request
            {
                Description = request.Description,
                Priority = request.Priority,
                //TODO: finalize
                // Supervisor = await _context.Users.FindAsync(request.Supervisor),
                CreatedAt = request.CreatedAt,
                EditedAt = request.EditedAt,
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
            var saveRequest = new Request
            {
                Description = request.Description,
                Priority = request.Priority,
                // TODO: finalize
                // Supervisor = await _context.Users.FindAsync(request.Supervisor),
                CreatedAt = request.CreatedAt,
                EditedAt = request.EditedAt,
            };
            _context.Request.Add(saveRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = saveRequest.RequestId }, saveRequest);
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
