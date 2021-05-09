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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet("GetOrderList/{username}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrderModel(string username)
        {
            var list =  await _context.OrderModel.ToListAsync();
            list = list.Where(o => o.Username == username).ToList();
            return list;
        }

        // GET: api/Order/5
        [HttpGet("GetOrderDetails/{oid}/{username}")]
        public async Task<ActionResult<OrderModel>> GetOrderModel(string oid,string username)
        {
            var orderModel = await _context.OrderModel.FindAsync(oid,username);

            if (orderModel == null)
            {
                return NotFound();
            }

            return orderModel;
        }

        [HttpPost("OrderMedicine")]
        public async Task<ActionResult<OrderModel>> PostOrderModel(OrderModel orderModel)
        {
            _context.OrderModel.Add(orderModel);
            try
            {
                await _context.SaveChangesAsync();
                var medicinemodel = await _context.MedicineModel.FindAsync(orderModel.Pid);
                var totalstock = medicinemodel.Medicine_Qty;
                var boughtmedicine = orderModel.Quantity;
                var leftstock = totalstock - boughtmedicine;
                medicinemodel.Medicine_Qty = leftstock;
                _context.Entry(medicinemodel).State = EntityState.Modified;
                 await _context.SaveChangesAsync();

            }
            catch (DbUpdateException)
            {
                if (OrderModelExists(orderModel.Oid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(orderModel);
        }

     
        private bool OrderModelExists(string id)
        {
            return _context.OrderModel.Any(e => e.Oid == id);
        }
    }
}
