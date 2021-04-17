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
using Microsoft.EntityFrameworkCore;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public LogController(RepositoryContext context, 
            UserManager<User> userManager,
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
            var logsTest = await _context.Logs
                .AsQueryable()
                .ProjectTo<LogDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return logsTest;
        }

        // GET: api/Log/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LogDto>> GetLog(Guid id)
        {
            var log = await _context.Logs.AsQueryable()
                .Where(e => e.LogId == id)
                .ProjectTo<LogDto>(_mapper.ConfigurationProvider).SingleAsync();

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }
        
        [HttpPut("{id}")]
        // if works don't modify
        public async Task<ActionResult<LogDto>> PutLog(Guid id, LogForCreationDto log)
        {
            var logForUpdate = _context.Logs
                .Include(c=> c.Categories)
                .FirstOrDefault(l => l.LogId == log.LogId);
            
            logForUpdate?.Categories.Clear();
            
            var categories = 
                log.Categories.Select(c => _context.Categories.FirstOrDefault(x => x.CategoryId == c)).ToList();
            categories.ForEach(i =>
            {
                logForUpdate?.Categories.Add(i);
            });
            
            logForUpdate!.MechanicId = log.Mechanic;
            logForUpdate!.Name = log.Name;
            logForUpdate!.Details = log.Details;
            
            _context.Update(logForUpdate!);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<LogDto>(logForUpdate);
        }

        // POST: api/Log
        [HttpPost]
        public async Task<ActionResult<LogDto>> PostLog(LogForCreationDto createLog)
        {
            var user = await _userManager.FindByIdAsync(createLog.Mechanic.ToString());
            var newLog = new Log
            {
                Name = createLog.Name,
                MechanicId = createLog.Mechanic,
                Details = createLog.Details,
                Categories = createLog.Categories.Select(c => _context.Categories.Find(c)).ToList()
            };
            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
            
            createLog.LogId = newLog.LogId;
            var outputLog = _mapper.Map<LogDto>(newLog);
            return CreatedAtAction("GetLog", new { id = createLog.LogId }, outputLog);
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
