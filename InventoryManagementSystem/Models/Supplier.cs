using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string ContactInfo { get; set; } = null!;
        public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();
    }
}