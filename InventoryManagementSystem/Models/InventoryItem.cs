using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
    }
}