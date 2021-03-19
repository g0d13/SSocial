using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class MachineController : ControllerBase
    {
        private readonly DataContext _context;

        public MachineController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Machine
        [HttpGet]
        public ActionResult<IEnumerable<GetMachineDto>> GetMachines()
        {
            var machines = _context.Machines.Select(e => new GetMachineDto
            {
                Brand = e.Model,
                Model = e.Model,
                MachineId = e.MachineId
            });
            return machines.ToList();
        }

        // GET: api/Machine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CreateMachineDto>> GetMachine(Guid id)
        {
            
            var machine = await (from b in _context.Machines 
                select new CreateMachineDto
                {
                    Brand = b.Brand,
                    Model = b.Model,
                    MachineId = b.MachineId,
                    Log = b.Log.LogId
                }).SingleOrDefaultAsync(b=>b.MachineId == id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        // PUT: api/Machine/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(Guid id, CreateMachineDto machine)
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
        public async Task<ActionResult<CreateMachineDto>> PostMachine(CreateMachineDto machine)
        {
            var newMachine = new Machine
            {  
                Brand = machine.Brand,
                Log = _context.Logs.Find(machine.Log),
                Model = machine.Model
            };
            _context.Machines.Add(newMachine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = newMachine.MachineId }, newMachine);
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
