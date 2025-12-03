using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookify.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; } = string.Empty;

        [ForeignKey(nameof(CustomerId))]
        [ValidateNever]
        public Customer? Customer { get; set; }

        public int ReservationId { get; set; }
        
        [ForeignKey(nameof(ReservationId))]
        [ValidateNever]
        public Reservation? Reservation { get; set; }

        [Required(ErrorMessage = "Invoice amount is required")]
        [Range(0, 999999.99, ErrorMessage = "Invoice amount must be between 0 and 999,999.99")]
        [DataType(DataType.Currency)]
        [Display(Name = "Invoice Amount")]
        public decimal InvoiceAmount { get; set; }

        [Required(ErrorMessage = "Issue date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Issued At")]
        public DateTime IssuedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Paid At")]
        public DateTime? PaidAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Canceled At")]
        public DateTime? CanceledAt { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public InvoiceStatus Status { get; set; }

        public int HotelId { get; set; }
        [ForeignKey(nameof(HotelId))]
        [ValidateNever]
        public Hotel Hotel { get; set; }

        [Display(Name = "Payment Intent ID")]
        public string? PaymentIntentId { get; set; }

    }

    public enum InvoiceStatus
    {
        Pending,
        Paid,
        Canceled
    }
}
