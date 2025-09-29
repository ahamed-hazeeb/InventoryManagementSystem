using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly InventoryContext _context;

        public OrdersController(InventoryContext context)
        {
            _context = context;
        }
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.InventoryItem).ToListAsync();
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.InventoryItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["InventoryItems"] = new SelectList(_context.InventoryItems, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, List<int> InventoryItemIds, List<int> Quantities)
        {
            if (ModelState.IsValid && InventoryItemIds.Count == Quantities.Count)
            {
                order.OrderItems = new List<OrderItem>();
                for (int i = 0; i < InventoryItemIds.Count; i++)
                {
                    var itemId = InventoryItemIds[i];
                    var qty = Quantities[i];
                    var item = await _context.InventoryItems.FindAsync(itemId);
                    if (item == null || item.Quantity < qty)
                    {
                        ModelState.AddModelError("", "Insufficient stock for item: " + item?.Name);
                        return View(order);
                    }
                    order.OrderItems.Add(new OrderItem { InventoryItemId = itemId, Quantity = qty });
                    item.Quantity -= qty;
                    _context.Update(item);
                }
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryItems"] = new SelectList(_context.InventoryItems, "Id", "Name");
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.InventoryItem).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null) return NotFound();
            ViewData["InventoryItems"] = new SelectList(_context.InventoryItems, "Id", "Name");
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryItems"] = new SelectList(_context.InventoryItems, "Id", "Name");
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.InventoryItem).FirstOrDefaultAsync(m => m.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                // Restore stock
                foreach (var orderItem in order.OrderItems)
                {
                    var item = await _context.InventoryItems.FindAsync(orderItem.InventoryItemId);
                    if (item != null)
                    {
                        item.Quantity += orderItem.Quantity;
                        _context.Update(item);
                    }
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
