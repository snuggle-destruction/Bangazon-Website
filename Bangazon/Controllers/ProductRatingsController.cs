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
    public class ProductRatingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ProductRatingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ProductRatings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductRating.Include(p => p.Product).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProductRatings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productRating = await _context.ProductRating
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (productRating == null)
            {
                var nullProductRating = new ProductRating();
                nullProductRating.Product = await _context.Product
                .Include(p => p.ProductType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
                nullProductRating.User = await GetCurrentUserAsync();

                return View(nullProductRating);
            }

            return View(productRating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RateProduct(int productId, int rating)
        {
            var productRating = new ProductRating();
            var user = await GetCurrentUserAsync();
            productRating.UserId = user.Id;
            productRating.ProductId = productId;
            productRating.Rating = rating;

            _context.Add(productRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductRatings/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description");
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: ProductRatings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductRatingId,ProductId,UserId,Rating")] ProductRating productRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", productRating.ProductId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", productRating.UserId);
            return View(productRating);
        }

        // GET: ProductRatings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productRating = await _context.ProductRating.FindAsync(id);
            if (productRating == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", productRating.ProductId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", productRating.UserId);
            return View(productRating);
        }

        // POST: ProductRatings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductRatingId,ProductId,UserId,Rating")] ProductRating productRating)
        {
            if (id != productRating.ProductRatingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductRatingExists(productRating.ProductRatingId))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Description", productRating.ProductId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", productRating.UserId);
            return View(productRating);
        }

        // GET: ProductRatings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productRating = await _context.ProductRating
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductRatingId == id);
            if (productRating == null)
            {
                return NotFound();
            }

            return View(productRating);
        }

        // POST: ProductRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productRating = await _context.ProductRating.FindAsync(id);
            _context.ProductRating.Remove(productRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductRatingExists(int id)
        {
            return _context.ProductRating.Any(e => e.ProductRatingId == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}
