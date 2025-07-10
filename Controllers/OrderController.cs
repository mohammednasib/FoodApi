using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly FoodContext _context;

        public OrderController(FoodContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Orders.Include(o => o.Client).Include(o => o.Food).ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var order = _context.Orders.Include(o => o.Client).Include(o => o.Food).FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            // You might want to validate ClientId and FoodId existence here
            _context.Orders.Add(order);
            _context.SaveChanges();
            return Ok(order);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Order updatedOrder)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            order.ClientId = updatedOrder.ClientId;
            order.FoodId = updatedOrder.FoodId;
            order.Quantity = updatedOrder.Quantity;
            order.OrderDate = updatedOrder.OrderDate;

            _context.SaveChanges();
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok("Deleted");
        }
    }
}
