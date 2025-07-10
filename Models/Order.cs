using System.Text.Json.Serialization;

namespace EmployeeApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [JsonIgnore] // 💥 THIS AVOIDS CYCLIC JSON ERROR
        public Client? Client { get; set; }

        public Food? Food { get; set; }
    }
}
