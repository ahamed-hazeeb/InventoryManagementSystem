namespace InventoryManagementSystem.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int InventoryItemId { get; set; }
        public int Quantity { get; set; }
        public InventoryItem InventoryItem { get; set; } = null!;
        public Order Order { get; set; } = null!;   
    }
}