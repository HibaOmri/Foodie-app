using FoodieApp.Models; // Important pour trouver ta BDD
using Microsoft.EntityFrameworkCore; // Important pour SQL Server

namespace FoodieApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Ajouter les services (Controllers + Vues)
            builder.Services.AddControllersWithViews();

            // 2. AJOUT CRUCIAL : Connecter la Base de Données
            // (Comme tu as fait un Scaffold, la connexion est déjà configurée dans FoodieDbContext.cs)
            builder.Services.AddDbContext<FoodieDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)));

            // 3. AJOUT CRUCIAL : Activer la mémoire pour le Panier (Session)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // 2b. AJOUT : Authentication (Cookie)
            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Login";
                });

            var app = builder.Build();

            // AJOUT: Seeding de la base de données avec retry pour Docker
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var maxRetries = 10;
                var delay = TimeSpan.FromSeconds(5);

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        var context = services.GetRequiredService<FoodieDbContext>();
                        context.Database.EnsureCreated(); // Crée la DB si elle n'existe pas
                        SeedData.Initialize(services);
                        logger.LogInformation("Base de données initialisée avec succès.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, $"Tentative {i + 1}/{maxRetries} - La DB n'est pas encore prête. Nouvelle tentative dans {delay.TotalSeconds}s...");
                        if (i == maxRetries - 1)
                        {
                            logger.LogError(ex, "Impossible de se connecter à la base de données après plusieurs tentatives.");
                            throw;
                        }
                        Thread.Sleep(delay);
                    }
                }
            }

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            
            app.UseSession();

            // IMPORTANT: Ordre : Auth -> Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Start();

            Console.WriteLine("\n\n##################################################");
            Console.WriteLine("   ✅ APPLICATION DEMARRÉE !");
            foreach (var url in app.Urls)
            {
                Console.WriteLine($"   👉 ACCESSIBLE SUR : {url}");
            }
            Console.WriteLine("##################################################\n\n");

            app.WaitForShutdown();
        }
    }
}
