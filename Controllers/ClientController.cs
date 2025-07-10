using Microsoft.AspNetCore.Mvc;
using EmployeeApi.Data;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly FoodContext _context;

        public ClientController(FoodContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Clients.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            return Ok(client);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Client updatedClient)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            client.Name = updatedClient.Name;
            client.Email = updatedClient.Email;
            client.Phone = updatedClient.Phone;

            _context.SaveChanges();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            _context.SaveChanges();
            return Ok("Deleted");
        }
    }
}
