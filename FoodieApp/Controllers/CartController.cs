using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FoodieApp.Models;
using System.Collections.Generic;
using System; // INDISPENSABLE pour Guid et DateTime

namespace FoodieApp.Controllers
{
    public class CartController : Controller
    {
        private readonly FoodieDbContext _context;

        public CartController(FoodieDbContext context)
        {
            _context = context;
        }

        // 1. AFFICHER LE PANIER
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }

        // 2. AJOUTER AU PANIER
        public IActionResult Add(int id)
        {


            var product = _context.Products.Find(id);

            if (product == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Produit introuvable." });
                }
                return RedirectToAction("Index", "Home");
            }
            
            // Product exists logic continues below...
            if (product != null) // Keeping original structure for diff simplicity, effectively always true now
            {
                var cart = GetCartFromSession();
                var item = cart.Find(p => p.Product.ProductId == id);

                if (item != null)
                {
                    item.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = product, Quantity = 1 });
                }
                SaveCartToSession(cart);

                // AJOUT : On stocke un petit message de succès pour l'afficher
                // AJOUT : Support AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = product.Name + " ajouté au panier !" });
                }

                // MODIFICATION : On redirige vers la page précédente (Referer) pour ne pas perdre la position
                string referer = Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                {
                    return Redirect(referer);
                }
                return RedirectToAction("Index", "Home");
        }
        return RedirectToAction("Index", "Home");
    }

        // 3. DIMINUER LA QUANTITÉ
        public IActionResult Decrease(int id)
        {
            var cart = GetCartFromSession();
            var item = cart.Find(p => p.Product.ProductId == id);

            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
                SaveCartToSession(cart);
            }

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            return RedirectToAction("Index");
        }

        // 4. SUPPRIMER UN ARTICLE (C'est la méthode pour le bouton X)
        public IActionResult Remove(int id)
        {
            var cart = GetCartFromSession();

            // On cherche l'article à supprimer par son ID
            var item = cart.Find(p => p.Product.ProductId == id);

            if (item != null)
            {
                cart.Remove(item);       // On le retire de la liste
                SaveCartToSession(cart); // On sauvegarde le panier mis à jour
            }

            return RedirectToAction("Index"); // On recharge la page
        }

        // 4. VALIDER LA COMMANDE
        public IActionResult Checkout()
        {
            // Récupérer le panier
            var cart = GetCartFromSession();

            // Si panier vide, on retourne au menu
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index");
            }

            // Créer un numéro de commande unique
            string numeroCommande = "CMD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            // Boucler sur chaque article pour l'enregistrer en base
            foreach (var item in cart)
            {
                var order = new Order
                {
                    OrderNo = numeroCommande,
                    ProductId = item.Product.ProductId,
                    Quantity = item.Quantity,
                    Status = "En préparation",
                    OrderDate = DateTime.Now,
                    UserId = null
                };

                _context.Orders.Add(order);
            }

            // Sauvegarder dans SQL Server
            _context.SaveChanges();

            // Vider le panier
            HttpContext.Session.Remove("Cart");

            // Afficher la page de succès
            return View("OrderSuccess", numeroCommande);
        }

        // --- Méthodes privées (Helpers) ---
        private List<CartItem> GetCartFromSession()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(sessionCart)) return new List<CartItem>();
            return JsonConvert.DeserializeObject<List<CartItem>>(sessionCart) ?? new List<CartItem>();
        }

        private void SaveCartToSession(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }
    }
}