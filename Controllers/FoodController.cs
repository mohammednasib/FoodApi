using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Data;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly FoodContext _context;

        public FoodController(FoodContext context)
        {
            _context = context;
        }

        // GET /food
        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Foods.ToList());

        // GET /food/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var food = _context.Foods.Find(id);
            if (food == null) return NotFound();
            return Ok(food);
        }

        // POST /food
        [HttpPost]
        public IActionResult Create(Food food)
        {
            _context.Foods.Add(food);
            _context.SaveChanges();
            return Ok(food);
        }

        // PUT /food/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Food updatedFood)
        {
            var food = _context.Foods.Find(id);
            if (food == null) return NotFound();

            food.Name = updatedFood.Name;
            food.Price = updatedFood.Price;
            food.IsAvailable = updatedFood.IsAvailable;

            _context.SaveChanges();
            return Ok(food);
        }

        // DELETE /food/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var food = _context.Foods.Find(id);
            if (food == null) return NotFound();

            _context.Foods.Remove(food);
            _context.SaveChanges();
            return Ok("Deleted");
        }
    }
}
