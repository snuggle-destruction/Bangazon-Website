using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.ViewComponents
{
    public class RecentProductsViewModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }

    public class RecentProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecentProductsViewComponent(ApplicationDbContext c, UserManager<ApplicationUser> userManager)
        {
            _context = c;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Get the current, authenticated user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            // Instantiate view model
            RecentProductsViewModel model = new RecentProductsViewModel();

            // Determine the 20 most recent products created
            var products = await _context.Product
                .OrderByDescending(p => p.ProductId)
                .Take(20)
                .ToListAsync();

            // Render template bound to OrderCountViewModel
            model.Products = products;
            return View(model);
        }
    }
}
