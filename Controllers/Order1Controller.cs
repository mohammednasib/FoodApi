using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Order1Controller : ControllerBase
    {
        private readonly string _connectionString;

        public Order1Controller(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = new List<object>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("GetAllOrders", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(new
                {
                    Id = reader.GetInt32("Id"),
                    ClientId = reader.GetInt32("ClientId"),
                    FoodId = reader.GetInt32("FoodId"),
                    Quantity = reader.GetInt32("Quantity"),
                    OrderDate = reader.GetDateTime("OrderDate")
                });
            }

            return Ok(orders);
        }

        [HttpPost]
        public IActionResult AddOrder(int clientId, int foodId, int quantity)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("AddOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_clientId", clientId);
            cmd.Parameters.AddWithValue("@p_foodId", foodId);
            cmd.Parameters.AddWithValue("@p_quantity", quantity);
            cmd.Parameters.AddWithValue("@p_orderDate", DateTime.Now);

            cmd.ExecuteNonQuery();
            return Ok("Order placed successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, int clientId, int foodId, int quantity)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("UpdateOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id", id);
            cmd.Parameters.AddWithValue("@p_clientId", clientId);
            cmd.Parameters.AddWithValue("@p_foodId", foodId);
            cmd.Parameters.AddWithValue("@p_quantity", quantity);

            cmd.ExecuteNonQuery();
            return Ok("Order updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            using var cmd = new MySqlCommand("DeleteOrder", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_id", id);

            cmd.ExecuteNonQuery();
            return Ok("Order deleted.");
        }
    }
}
