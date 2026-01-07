using System;

namespace FoodieApp.Models // Assure-toi que c'est bien le nom de ton projet
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}