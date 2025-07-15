using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using EmployeeApi.Models;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Food1Controller : ControllerBase
    {
        private readonly string _connectionString;

        public Food1Controller(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet]
        public IActionResult GetFoods()
        {
            var foods = new List<Food>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("GetAllFoods", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                foods.Add(new Food
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDouble("Price"),
                    IsAvailable = reader.GetBoolean("IsAvailable")
                });
            }

            return Ok(foods);
        }

        [HttpGet("{id}")]
        public IActionResult GetFoodById(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("GetFoodById", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var food = new Food
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDouble("Price"),
                    IsAvailable = reader.GetBoolean("IsAvailable")
                };

                return Ok(food);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult AddFood([FromBody] Food food)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("AddFood", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_name", food.Name);
            cmd.Parameters.AddWithValue("@p_price", food.Price);
            cmd.Parameters.AddWithValue("@p_isAvailable", food.IsAvailable);

            cmd.ExecuteNonQuery();
            return Ok("Food added successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFood(int id, [FromBody] Food food)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("UpdateFood", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id", id);
            cmd.Parameters.AddWithValue("@p_name", food.Name);
            cmd.Parameters.AddWithValue("@p_price", food.Price);
            cmd.Parameters.AddWithValue("@p_isAvailable", food.IsAvailable);

            cmd.ExecuteNonQuery();
            return Ok("Food updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFood(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("DeleteFood", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_id", id);

            cmd.ExecuteNonQuery();
            return Ok("Food deleted successfully.");
        }
    }
}
