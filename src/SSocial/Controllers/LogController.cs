using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSocial.Data;
using SSocial.Dtos;
using SSocial.Models;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public LogController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/Log
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogs()
        {
            var logMapper = await _context.Logs
                .ProjectTo<LogDto>(_mapper.ConfigurationProvider).ToListAsync();

            return logMapper;
        }

        // GET: api/Log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogDto>> GetLog(Guid id)
        {
            var log = await (from b in _context.Logs
                select new LogDto()
                {
                    LogId = b.LogId,
                    Machines = (b.Machines.Select(x => x.MachineId)).ToList(),
                    Mechanic = b.Mechanic.Id,
                    Name = b.Name
                }).SingleAsync(e => e.LogId == id);

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // PUT: api/Log/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLog(Guid id, LogDto log)
        {
            if (id != log.LogId)
            {
                return BadRequest();
            }

            var newLog = new Log()
            {
                LogId = log.LogId,
                Name = log.Name,
                // Mechanic = await _context.Users.FindAsync(log.Mechanic.ToString()),
                //TODO: UPDATE THis
                Machines = _context.Machines.Select(e => e).ToList()
            };

            _context.Entry(newLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Log
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LogDto>> PostLog(LogDto createLog)
        {
            var user = await _userManager.FindByIdAsync(createLog.Mechanic.ToString());
            
            var newLog = new Log
            {
                Name = createLog.Name,
                Machines = createLog.Machines.Select(e => _context.Machines.Find(e)).ToList(),
                Mechanic = user
            };
            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetLog", new { id = newLog.LogId }, createLog);
        }
 
        // DELETE: api/Log/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(Guid id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LogExists(Guid id)
        {
            return _context.Logs.Any(e => e.LogId == id);
        }
    }
}