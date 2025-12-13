using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        [ForeignKey(nameof(HotelId))]
        [ValidateNever]
        public Hotel? Hotel { get; set; }

        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        [ValidateNever]
        public Customer? Customer { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Review text is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters")]
        public string ReviewText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Link to the invoice to verify that user has actually booked
        public int? InvoiceId { get; set; }

        [ForeignKey(nameof(InvoiceId))]
        [ValidateNever]
        public Invoice? Invoice { get; set; }
    }
}
