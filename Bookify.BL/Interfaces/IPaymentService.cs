using Bookify.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IPaymentService
    {
        public Session CreateCheckoutSession(Reservation reservation, RoomType roomType, string domain);
    }
}
