using Bookify.BL.Interfaces;
using Bookify.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class PaymentService : IPaymentService
    {
        public Session CreateCheckoutSession(Reservation reservation, RoomType roomType, string domain)
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)((reservation.TotalPrice) * 100), 
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Room Type: {roomType.Name} ({reservation.CheckInDate:MMM dd} - {reservation.CheckOutDate:MMM dd})",

                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{domain}Customer/Booking/Confirmation?sessionId={{CHECKOUT_SESSION_ID}}&reservationId={reservation.Id}",
                CancelUrl = $"{domain}Customer/Booking/Cancel?reservationId={reservation.Id}"
            };

            var service = new SessionService();
            return service.Create(options);
        }
    }
}