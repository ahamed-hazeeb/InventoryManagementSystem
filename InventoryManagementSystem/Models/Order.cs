using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Required]
        public string Status { get; set; } = "Pending"; // e.g., "Pending", "Shipped"
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}