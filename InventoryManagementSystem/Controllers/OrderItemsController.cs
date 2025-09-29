using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly InventoryContext _context;

        public OrderItemsController(InventoryContext context)
        {
            _context = context;
        }

        // POST: OrderItems/Create (called from Order Create/Edit for simplicity)
        [HttpPost]
        public async Task<IActionResult> Create(int orderId, int inventoryItemId, int quantity)
        {
            var orderItem = new OrderItem { OrderId = orderId, InventoryItemId = inventoryItemId, Quantity = quantity };
            var item = await _context.InventoryItems.FindAsync(inventoryItemId);
            if (item != null && item.Quantity >= quantity)
            {
                item.Quantity -= quantity;
                _context.Update(item);
                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Orders", new { id = orderId });
            }
            return BadRequest("Insufficient stock or invalid item");
        }
    }
}
