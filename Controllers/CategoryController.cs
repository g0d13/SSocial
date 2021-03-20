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
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CategoryController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categoryMapper = await _context.Categories
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();

            return categoryMapper;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var categoryMapper = await _context.Categories.Where(e => e.CategoryId == id)
                .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (categoryMapper == null)
            {
                return NotFound();
            }

            return categoryMapper;
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
            var categoryMapped = CreateCategory(category);


            _context.Entry(categoryMapped).State = EntityState.Modified;

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
            var categoryMapped = CreateCategory(category);
            await _context.Categories.AddAsync(categoryMapped);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = categoryMapped.CategoryId }, category);
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

        private Category CreateCategory(CategoryDto category)
        {
            var categoryMapped = _mapper.Map<Category>(category);
            categoryMapped.Machines = category.Machines?.Select(e => _context.Machines.Find(e)).ToList();
            return categoryMapped;
        }
    }
}
