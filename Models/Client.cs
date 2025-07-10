namespace EmployeeApi.Models
{
    public class Client
    {
        public int Id { get; set; }  // Primary key, auto-increment
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Navigation property for related orders
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
