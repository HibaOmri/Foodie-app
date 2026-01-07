using Microsoft.AspNetCore.Mvc;
using FoodieApp.Models;
using System;
using System.Linq;

namespace FoodieApp.Controllers
{
    public class TestController : Controller
    {
        private readonly FoodieDbContext _context;

        public TestController(FoodieDbContext context)
        {
            _context = context;
        }

        public IActionResult Seed()
        {
            try
            {
                // FORCE SEEDING LOGIC HERE
                var burgerCat = _context.Categories.FirstOrDefault(c => c.Name == "Burger");
                if (burgerCat == null) return Content("Erreur: Catégorie 'Burger' introuvable.");

                if (!_context.Products.Any(p => p.Name == "Test Burger"))
                {
                    _context.Products.Add(new Product
                    {
                        Name = "Test Burger",
                        Description = "Burger test créé manuellement",
                        Price = 99,
                        Category = burgerCat,
                        IsActive = true,
                        ImageUrl = "/images/f1.png",
                        CreateDate = DateTime.Now
                    });
                    _context.SaveChanges();
                    return Content("SUCCÈS : 'Test Burger' ajouté ! Allez voir le menu.");
                }

                return Content("INFO : 'Test Burger' existe déjà.");
            }
            catch (Exception ex)
            {
                return Content("ERREUR CRITIQUE : " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
