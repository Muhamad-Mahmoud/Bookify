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
        public Session CreateCheckoutSession(Room room, string domain)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)((room.Price) * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Room: {room.RoomName}",
                                Images = new List<string>
                                {
                                    room.ImageURL ?? ""
                                }
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"{domain}Payment/Success?roomId={room.Id}",
                CancelUrl = $"{domain}Payment/Cancel"
            };

            var service = new SessionService();
            return service.Create(options);
        }

    }
}