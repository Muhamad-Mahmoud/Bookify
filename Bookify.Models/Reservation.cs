using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bookify.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Customer is required")]
        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        [ValidateNever]
        public Customer? Customer { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters")]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Check-in date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        [Compare(nameof(CheckInDate), ErrorMessage = "Check-out date must be after check-in date")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Total price is required")]
        [Range(0, 999999.99, ErrorMessage = "Total price must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [ValidateNever]
        [Display(Name = "Reserved Rooms")]
        public ICollection<ReservedRoom> RoomReserved { get; set; } = new List<ReservedRoom>();

        [ValidateNever]
        public Invoice? Invoice { get; set; }
    }
}
