using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeApi.Models;


namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Client1Controller : ControllerBase
    {
        private readonly string _connectionString;

        public Client1Controller(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = new List<object>();
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var cmd = new MySqlCommand("CALL GetAllClients();", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clients.Add(new
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    Phone = reader.GetString("Phone")
                });
            }
            return Ok(clients);
        }

        [HttpPost]
        public IActionResult AddClient([FromBody] Client client)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var cmd = new MySqlCommand("CALL AddClient(@name, @email, @phone);", conn);
            cmd.Parameters.AddWithValue("@name", client.Name);
            cmd.Parameters.AddWithValue("@email", client.Email);
            cmd.Parameters.AddWithValue("@phone", client.Phone);

            cmd.ExecuteNonQuery();
            return Ok("Client added successfully");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int id, [FromBody] Client client)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var cmd = new MySqlCommand("CALL UpdateClient(@id, @name, @email, @phone);", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", client.Name);
            cmd.Parameters.AddWithValue("@email", client.Email);
            cmd.Parameters.AddWithValue("@phone", client.Phone);
            cmd.ExecuteNonQuery();
            return Ok("Client updated");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            using var cmd = new MySqlCommand("CALL DeleteClient(@id);", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            return Ok("Client deleted");
        }
    }
}
