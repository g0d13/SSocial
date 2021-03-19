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
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            return await _context.Categories.Select(c =>
                new CategoryDto()
                {
                    Machines = c.Machines.Select(e => e.MachineId).ToList(),
                    Name = c.Name,
                    CategoryId = c.CategoryId
                }).ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto()
            {
                Machines = category.Machines.Select(c => c.MachineId).ToList(),
                Name = category.Name,
                CategoryId = category.CategoryId
            };

            return categoryDto;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, CategoryDto category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            var machines =
                category.Machines.Select(e => _context.Machines.Find(e)).ToList();
            var categoryDb = new Category()
            {
                CategoryId = category.CategoryId,
                Machines = machines,
                Name = category.Name
            };

            _context.Entry(categoryDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto category)
        {
            var categoryDb = new Category()
            {
                Machines = category.Machines?.Select(e => _context.Machines.Find(e)).ToList(),
                Name = category.Name,
                CategoryId = category.CategoryId
            };
            await _context.Categories.AddAsync(categoryDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = categoryDb.CategoryId }, category);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
