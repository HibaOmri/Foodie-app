using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodieApp.Models;

namespace FoodieApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly FoodieDbContext _context;

        public ProductsController(FoodieDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var foodieDbContext = _context.Products.Include(p => p.Category);
            return View(await foodieDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,Quantity,ImageUrl,CategoryId,IsActive,CreateDate")] Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imageFile.FileName);
                    var uploadPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new System.IO.FileStream(uploadPath, System.IO.FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.ImageUrl = "/images/" + fileName;
                }
                else
                {
                    // No image uploaded, assign default based on Category
                    var category = await _context.Categories.FindAsync(product.CategoryId);
                    if (category != null)
                    {
                        var lowerName = category.Name.ToLower();
                        if (lowerName.Contains("burger")) product.ImageUrl = "/images/default_burger.png";
                        else if (lowerName.Contains("pizza")) product.ImageUrl = "/images/f3.png";
                        else if (lowerName.Contains("pâtes") || lowerName.Contains("pasta")) product.ImageUrl = "/images/f4.png";
                        else if (lowerName.Contains("frites") || lowerName.Contains("fries")) product.ImageUrl = "/images/f5.png";
                        else product.ImageUrl = "/images/f1.png"; // Fallback
                    }
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Quantity,ImageUrl,CategoryId,IsActive,CreateDate")] Product product, IFormFile? imageFile)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(imageFile.FileName);
                        var uploadPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new System.IO.FileStream(uploadPath, System.IO.FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        product.ImageUrl = "/images/" + fileName;
                    }
                    else
                    {
                        // Keep existing ImageUrl if no new file is uploaded
                        // We rely on the hidden input in the view or fetching from DB.
                        // Ideally checking if it's null/empty here might be needed if the view doesn't send it back.
                        // For now we assume the hidden field does its job, but let's be safe and fetch if null.
                        if (string.IsNullOrEmpty(product.ImageUrl))
                        {
                             var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                             if (existingProduct != null)
                             {
                                 product.ImageUrl = existingProduct.ImageUrl;
                             }
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
