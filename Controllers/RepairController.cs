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
    public class RepairController : ControllerBase
    {
        private readonly DataContext _context;

        public RepairController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Repair
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRepairDto>>> GetRepair()
        {
            return await _context.Repair.Select(e => new GetRepairDto()
            {
                Mechanic = e.Mechanic.Id,
                Details = e.Details,
                Severity = e.Severity,
                ArrivalTime = e.ArrivalTime,
                DepartureTime = e.DepartureTime,
                IsFixed = e.IsFixed,
                RepairId = e.RepairId
            }).ToListAsync();
        }

        // GET: api/Repair/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRepairDto>> GetRepair(Guid id)
        {
            var repair = await _context.Repair.FindAsync(id);

            if (repair == null)
            {
                return NotFound();
            }

            return new GetRepairDto()
            {
                Mechanic = repair.Mechanic.Id,
                Details = repair.Details,
                Severity = repair.Severity,
                ArrivalTime = repair.ArrivalTime,
                DepartureTime = repair.DepartureTime,
                IsFixed = repair.IsFixed,
                RepairId = repair.RepairId
            };
        }

        // PUT: api/Repair/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepair(Guid id, GetRepairDto repairDto)
        {
            if (id != repairDto.RepairId)
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
                RepairId = repairDto.RepairId
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
        public async Task<ActionResult<GetRepairDto>> PostRepair(GetRepairDto repairDto)
        {
            var repair = new Repair()
            {
                //TODO: finalize
                // Mechanic = await _context.Users.FindAsync(repairDto.Mechanic),
                Details = repairDto.Details,
                Severity = repairDto.Severity,
                ArrivalTime = repairDto.ArrivalTime,
                DepartureTime = repairDto.DepartureTime,
                IsFixed = repairDto.IsFixed,
                RepairId = repairDto.RepairId
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
