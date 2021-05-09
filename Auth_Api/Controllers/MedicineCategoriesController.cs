using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace Auth_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicineCategoriesController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("Checker")]
        public IActionResult GetData()
        {
            //return Ok();
            var str = "hello";
            return Ok(new { str});
        }

        // GET: api/MedicineCategories
        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<MedicineCategory>>> GetMedicineCategorie()
        {
            return await _context.MedicineCategorie.ToListAsync();
        }

        // GET: api/MedicineCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineCategory>> GetMedicineCategory(string id)
        {
            var medicineCategory = await _context.MedicineCategorie.FindAsync(id);

            if (medicineCategory == null)
            {
                return NotFound();
            }

            return Ok(medicineCategory);
        }

        // PUT: api/MedicineCategories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicineCategory(string id, MedicineCategory medicineCategory)
        {
            if (id != medicineCategory.Category_Id)
            {
                return BadRequest();
            }

            _context.Entry(medicineCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineCategoryExists(id))
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

        // POST: api/MedicineCategories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("AddCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MedicineCategory>> PostMedicineCategory(MedicineCategory medicineCategory)
        {
            _context.MedicineCategorie.Add(medicineCategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MedicineCategoryExists(medicineCategory.Category_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMedicineCategory", new { id = medicineCategory.Category_Id }, medicineCategory);
        }

        // DELETE: api/MedicineCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicineCategory>> DeleteMedicineCategory(string id)
        {
            var medicineCategory = await _context.MedicineCategorie.FindAsync(id);
            if (medicineCategory == null)
            {
                return NotFound();
            }

            _context.MedicineCategorie.Remove(medicineCategory);
            await _context.SaveChangesAsync();

            return medicineCategory;
        }

        private bool MedicineCategoryExists(string id)
        {
            return _context.MedicineCategorie.Any(e => e.Category_Id == id);
        }
    }
}
