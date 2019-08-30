using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;

namespace Bangazon.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var applicationDbContext = _context.Order
                        .Include(o => o.User)
                        .Include(o => o.OrderProducts)
                        .Where(o => user.Id == o.UserId && o.DateCompleted == null);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .ThenInclude(pt => pt.ProductType)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var currentUserId = GetCurrentUserAsync().GetAwaiter().GetResult().Id;

            ViewData["PaymentTypes"] = _context.PaymentType.Count(x => x.UserId == currentUserId);
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "Description");

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,DateCreated,DateCompleted,UserId,PaymentTypeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentType, "PaymentTypeId", "AccountNumber", order.PaymentTypeId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.PaymentType)
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Order)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderId == id);
            var order = await _context.Order
            .Include(o => o.PaymentType)
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Order)
            .FirstOrDefaultAsync(m => m.OrderId == id);
            var orderedProducts =  order.OrderProducts;

            _context.OrderProduct.RemoveRange(orderedProducts);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        private Task<ApplicationUser> GetUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private async Task<bool> WasCreatedByUser(Order order)
        {
            var user = await GetUserAsync();
            return order.UserId == user.Id;
        }

        public async Task<IActionResult> GetOrderHistory()
        {
            var user = await GetCurrentUserAsync();

            var orderHistoryList = await _context.Order
                .Where(o => o.UserId == user.Id && o.DateCompleted != null)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.PaymentType)
                .ToListAsync();

            return View(orderHistoryList);
        }

        public async Task<IActionResult> ReportsIndex()
        {
            return View();
        }

        public async Task<IActionResult> IncompleteOrders()
        {
            var applicationDbContext = _context.Order
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .Where(o => o.DateCompleted == null);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> MultipleOrders()
        {
            var applicationDbContext = _context.Order
            .Include(o => o.User)
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> AbandonedProducts()
        {
            var abandoned = _context.Order
            .Include(o => o.OrderProducts)
            .ThenInclude(op => op.Product)
            .ThenInclude(p => p.ProductType)
            .Where(o => o.DateCompleted == null)
            //.Select(o => new { count = o.OrderProducts.Count() })
            .ToList();
            //.OrderByDescending(o => o.OrderProducts.Count);
            return View(abandoned);
        }

        public async Task<Order> CreateOrder()
        {
            var order = new Order();
            order.UserId = GetCurrentUserAsync().GetAwaiter().GetResult().Id;
            _context.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IActionResult> AddToCart(int ProductId)
        {
            var currentUserId = GetCurrentUserAsync().GetAwaiter().GetResult().Id;
            var order = _context.Order.FirstOrDefault(x => x.UserId == currentUserId && x.DateCompleted == null);

            if(order == null)
            {
                order = CreateOrder().GetAwaiter().GetResult();
            }

            var orderProduct = new OrderProduct();
            orderProduct.OrderId = order.OrderId;
            orderProduct.ProductId = ProductId;
            _context.Add(orderProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CompleteOrder(int OrderId, int PaymentTypeId)
        {
            var order = _context.Order.FirstOrDefault(x => x.OrderId == OrderId);
            if(order != null)
            {
                order.PaymentTypeId = PaymentTypeId;
                order.DateCompleted = DateTime.Now;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("Create", "PaymentTypes");
            }

            return RedirectToAction("GetOrderHistory");


        }
    }
}
