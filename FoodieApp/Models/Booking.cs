using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodieApp.Models // Assure-toi que c'est le même namespace que les autres
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int? GuestCount { get; set; } // Le ? veut dire que ça peut être vide

        public DateTime? BookingDate { get; set; }

        public DateTime CreateDate { get; set; }
    }
}