using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace FoodieApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FoodieDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FoodieDbContext>>()))
            {
                // Ensure the database exists
                context.Database.EnsureCreated();

                // Look for any products.
                // 1. Seed Categories (Ensure they exist)
                var burgerCat = context.Categories.FirstOrDefault(c => c.Name == "Burger") ?? new Category { Name = "Burger", IsActive = true, CreateDate = DateTime.Now };
                var pizzaCat = context.Categories.FirstOrDefault(c => c.Name == "Pizza") ?? new Category { Name = "Pizza", IsActive = true, CreateDate = DateTime.Now };
                var pastaCat = context.Categories.FirstOrDefault(c => c.Name == "Pâtes") ?? new Category { Name = "Pâtes", IsActive = true, CreateDate = DateTime.Now };
                var friesCat = context.Categories.FirstOrDefault(c => c.Name == "Frites") ?? new Category { Name = "Frites", IsActive = true, CreateDate = DateTime.Now };

                if (!context.Categories.Any(c => c.Name == "Burger"))
                {
                    context.Categories.AddRange(burgerCat, pizzaCat, pastaCat, friesCat);
                    context.SaveChanges();
                }

                // 1b. Seed Admin User
                if (!context.Users.Any(u => u.Username == "admin"))
                {
                    context.Users.Add(new User
                    {
                        Name = "Administrateur",
                        Username = "admin",
                        Password = "admin123", // In real app: HASH THIS!
                        Role = "Admin",
                        CreateDate = DateTime.Now,
                        Email = "admin@foodie.com"
                    });
                    context.SaveChanges();
                }

                // 2. Seed Products
                // Helper to upsert product
                void UpsertProduct(string name, string desc, decimal price, Category cat, string img)
                {
                    var p = context.Products.FirstOrDefault(x => x.Name == name);
                    if (p == null)
                    {
                        context.Products.Add(new Product
                        {
                            Name = name,
                            Description = desc,
                            Price = price,
                            Category = cat,
                            IsActive = true,
                            ImageUrl = img,
                            CreateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        // Update image if it changed (fixing the duplicate image issue)
                        p.ImageUrl = img;
                        // p.Price = price; // Optional: update price if needed
                    }
                }

                // 2. Seed Products (Upsert logic)
                UpsertProduct("Tasty Burger", "Burger délicieux avec fromage", 10, burgerCat, "/images/f1.png");
                UpsertProduct("Double Burger", "Double viande pour les gourmands", 14, burgerCat, "/images/f2.png");
                UpsertProduct("Chicken Burger", "Poulet croustillant et sauce maison", 12, burgerCat, "/images/f7.png"); 
                UpsertProduct("Veggie Burger", "100% Végétal et gourmand", 11, burgerCat, "/images/f8.png"); // New image

                UpsertProduct("Pizza Margherita", "Classique tomate fromage", 12, pizzaCat, "/images/f3.png");
                UpsertProduct("Pizza Pepperoni", "Avec du saucisson piquant", 13, pizzaCat, "/images/f6.png");
                UpsertProduct("Pizza 4 Fromages", "Mozzarella, Gorgonzola, Chèvre, Parmesan", 13, pizzaCat, "/images/o2.jpg"); // Use Offer image for variety
                UpsertProduct("Pizza Végétarienne", "Légumes frais de saison", 12, pizzaCat, "/images/f3.png"); // Re-use f3 (Margherita-like) is acceptable, or reuse f6.

                UpsertProduct("Pâtes Carbonara", "Sauce crémeuse et lardons", 11, pastaCat, "/images/f4.png");
                UpsertProduct("Pâtes Bolognaise", "Sauce tomate à la viande de boeuf", 11, pastaCat, "/images/f9.png"); // New image
                UpsertProduct("Pâtes Pesto", "Basilic frais, pignons et parmesan", 10, pastaCat, "/images/f4.png"); // Reuse f4 is okay for now

                UpsertProduct("Frites Maison", "Croustillantes et dorées", 5, friesCat, "/images/f5.png");
                UpsertProduct("Potatoes", "Quartiers de pommes de terre épicés", 6, friesCat, "/images/f10.png"); // New Generated Image
                UpsertProduct("Frites Cheddar", "Frites fraiches avec sauce cheddar", 7, friesCat, "/images/f11.png"); // New Generated Image

                context.SaveChanges();
            }
        }
    }
}
