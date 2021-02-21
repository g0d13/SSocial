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
    public class LogController : ControllerBase
    {
        private readonly DataContext _context;

        public LogController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Log
        [HttpGet]
        public ActionResult<IEnumerable<GetLogDto>> GetLogs()
        {
            var logs = (from l in _context.Logs
                select new GetLogDto()
                {
                    LogId = l.LogId,
                    Machines = (l.Machines.Select(x => x.MachineId)).ToList(),
                    Mechanic = l.Mechanic.Id,
                    Name = l.Name
                }).ToList();
            return logs;
        }

        // GET: api/Log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetLogDto>> GetLog(Guid id)
        {
            var log = await (from b in _context.Logs
                select new GetLogDto()
                {
                    LogId = b.LogId,
                    Machines = b.Machines.ToList().ConvertAll(e => _context.Machines.Find(e).MachineId),
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
        public async Task<IActionResult> PutLog(Guid id, Log log)
        {
            if (id != log.LogId)
            {
                return BadRequest();
            }

            _context.Entry(log).State = EntityState.Modified;

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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Log
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateLogDto>> PostLog(CreateLogDto createLog)
        {
            var list = _context.Users.ToList();
            var newLog = new Log
            {
                Name = createLog.Name,
                Machines = createLog.Machines.ToList().ConvertAll(e => _context.Machines.Find(e)),
                Mechanic = _context.Users.FirstOrDefault(id => id.Id == createLog.Mechanic.ToString()),
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
