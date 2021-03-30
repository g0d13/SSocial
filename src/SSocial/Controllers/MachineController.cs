using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SSocial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public MachineController(RepositoryContext context)
        {
            _context = context;
        }

        // GET: api/Machine
        [HttpGet]
        public ActionResult<IEnumerable<MachineDto>> GetMachines()
        {
            var machines = _context.Machines
                .AsQueryable()
                .Select(e => new MachineDto
                {
                    Brand = e.Model,
                    Identifier = e.Identifier,
                    Model = e.Model,
                    MachineId = e.MachineId,
                    Category = e.Category.CategoryId
                });
            return machines.ToList();
        }

        // GET: api/Machine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineDto>> GetMachine(Guid id)
        {

            var machine = await _context.Machines
                .AsQueryable()
                .Where(e => e.MachineId == id)
                .Select(e => new MachineDto
                {
                    Brand = e.Model,
                    Identifier = e.Identifier,
                    Model = e.Model,
                    MachineId = e.MachineId,
                    Category = e.Category.CategoryId
                })
                .FirstOrDefaultAsync();

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        // PUT: api/Machine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(Guid id, MachineDto machine)
        {
            if (id != machine.MachineId)
            {
                return BadRequest();
            }
            //TODO: Update this
            _context.Entry(machine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Machine
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MachineDto>> PostMachine(MachineDto machine)
        {
            var machineMapped = new Machine
            {
                Identifier = machine.Identifier,
                Model = machine.Model,
                Brand = machine.Brand,
                Category = await _context.Categories.FindAsync(machine.Category),
            };
            await _context.Machines.AddAsync(machineMapped);
            await _context.SaveChangesAsync();
            
            machine.MachineId = machineMapped.MachineId;
            return CreatedAtAction("GetMachine", new { id = machineMapped.MachineId }, machine);
        }

        // DELETE: api/Machine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(Guid id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineExists(Guid id)
        {
            return _context.Machines.Any(e => e.MachineId == id);
        }
    }
}
