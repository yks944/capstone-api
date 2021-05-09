using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Models;

namespace Auth_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicineController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicine
        [HttpGet("GetAllMedicine/{cateid}")]
        public async Task<ActionResult<IEnumerable<MedicineModel>>> GetMedicineModel(string cateid)
        {
            var med_list =  await _context.MedicineModel.ToListAsync();
            med_list = med_list.Where(med => med.Category_Id == cateid).ToList();
            return med_list;
        }

        // GET: api/Medicine/5
        [HttpGet("GetMedicine/{cateid}/{medid}")]
        public async Task<ActionResult<MedicineModel>> GetMedicineModel(string cateid,string medid)
        {
            var medicineModel = await _context.MedicineModel.FindAsync(medid);

            if (medicineModel == null)
            {
                return NotFound();
            }

            return medicineModel;
        }

        // PUT: api/Medicine/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("EditMedicine/{id}")]
        public async Task<IActionResult> PutMedicineModel(string id, MedicineModel medicineModel)
        {
            if (id != medicineModel.Medicine_Id)
            {
                return BadRequest();
            }

            _context.Entry(medicineModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineModelExists(id))
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

        // POST: api/Medicine
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("AddMedicine")]
        public async Task<ActionResult<MedicineModel>> PostMedicineModel(MedicineModel medicineModel)
        {
            _context.MedicineModel.Add(medicineModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MedicineModelExists(medicineModel.Medicine_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(medicineModel);
        }

        // DELETE: api/Medicine/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MedicineModel>> DeleteMedicineModel(string id)
        {
            var medicineModel = await _context.MedicineModel.FindAsync(id);
            if (medicineModel == null)
            {
                return NotFound();
            }

            _context.MedicineModel.Remove(medicineModel);
            await _context.SaveChangesAsync();

            return medicineModel;
        }

        private bool MedicineModelExists(string id)
        {
            return _context.MedicineModel.Any(e => e.Medicine_Id == id);
        }
    }
}
