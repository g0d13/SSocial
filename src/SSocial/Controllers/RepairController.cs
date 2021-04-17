using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public RepairController(RepositoryContext context)
        {
            _context = context;
        }

        // GET: api/Repair
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepairDto>>> GetRepair()
        {
            return await _context.Repair.Select(e => new RepairDto()
            {
                Mechanic = e.Mechanic.Id,
                Details = e.Details,
                Severity = e.Severity,
                ArrivalTime = e.ArrivalTime,
                DepartureTime = e.DepartureTime,
                IsFixed = e.IsFixed,
                Id = e.RepairId,
                Log = e.LogId
            }).ToListAsync();
        }

        // GET: api/Repair/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RepairDto>> GetRepair(Guid id)
        {
            var repair = await _context.Repair.FindAsync(id);

            if (repair == null)
            {
                return NotFound();
            }

            return new RepairDto()
            {
                Mechanic = repair.Mechanic.Id,
                Details = repair.Details,
                Severity = repair.Severity,
                ArrivalTime = repair.ArrivalTime,
                DepartureTime = repair.DepartureTime,
                IsFixed = repair.IsFixed,
                Id = repair.RepairId
            };
        }

        // PUT: api/Repair/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepair(Guid id, RepairDto repairDto)
        {
            if (id != repairDto.Id)
            {
                return BadRequest();
            }

            var repair = new Repair()
            {
                //TODO: Finalize this
                // Mechanic = await _context.Users.FindAsync(repairDto.Mechanic),
                Details = repairDto.Details,
                Severity = repairDto.Severity,
                ArrivalTime = repairDto.ArrivalTime,
                DepartureTime = repairDto.DepartureTime,
                IsFixed = repairDto.IsFixed,
                RepairId = repairDto.Id
            };

            _context.Entry(repair).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepairExists(id))
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

        // POST: api/Repair
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RepairDto>> PostRepair(RepairDto repairDto)
        {
            var repair = new Repair()
            {
                Details = repairDto.Details,
                Severity = repairDto.Severity,
                ArrivalTime = repairDto.ArrivalTime,
                DepartureTime = repairDto.DepartureTime,
                IsFixed = repairDto.IsFixed,
                RepairId = repairDto.Id,
                Log = await _context.Logs.FindAsync(repairDto.Log),
                Mechanic = await _context.Users.FindAsync(repairDto.Mechanic),
                
            };
            await _context.Repair.AddAsync(repair);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRepair", new { id = repair.RepairId }, repairDto);
        }

        // DELETE: api/Repair/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepair(Guid id)
        {
            var repair = await _context.Repair.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }

            _context.Repair.Remove(repair);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RepairExists(Guid id)
        {
            return _context.Repair.Any(e => e.RepairId == id);
        }
    }
}
