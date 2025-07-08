using Microsoft.EntityFrameworkCore;
using EmployeeApi.Models; // Adjust this if your namespace is different

namespace EmployeeApi.Data
{
    public class FoodContext : DbContext
    {
        public FoodContext(DbContextOptions<FoodContext> options) : base(options) { }

        public DbSet<Food> Foods { get; set; }
    }
}
