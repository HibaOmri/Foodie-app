using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodieApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

namespace FoodieApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodieDbContext _context;

        public HomeController(FoodieDbContext context)
        {
            _context = context;
        }

        // 1. Affiche la page d'accueil avec les produits (Menu)
        public async Task<IActionResult> Index()
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'FoodieDbContext.Products' is null.");
            }

            var produits = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(produits);
        }

        // 2. Gère la Réservation
        [HttpPost]
        public async Task<IActionResult> BookTable(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreateDate = DateTime.Now;
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                TempData["BookingSuccess"] = "Votre table a été réservée avec succès !";

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // 3. Page Menu filtré par catégorie
        public async Task<IActionResult> Menu(string category)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                 query = query.Where(p => p.Category.Name.Contains(category));
                 ViewData["CategoryName"] = category;
                 ViewData["Title"] = "Menu - " + category;
            }
            else
            {
                ViewData["CategoryName"] = "Tout";
                ViewData["Title"] = "Notre Menu";
            }

            var products = await query.ToListAsync();
            return View(products);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
