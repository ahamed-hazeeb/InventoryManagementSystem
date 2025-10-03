namespace InventoryManagementSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
        public List<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    }
}