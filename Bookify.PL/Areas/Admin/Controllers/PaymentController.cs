using Bookify.BL.Interfaces;
using Bookify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Owner")]
    public class PaymentController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public PaymentController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index()
        {
            string? ownerId = null;
            if (!User.IsInRole("Admin"))
            {
                ownerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            }

            var allInvoices = await _invoiceService.GetAllInvoicesAsync(ownerId);
            var paidInvoices = allInvoices.Where(i => i.Status == InvoiceStatus.Paid);
            return View(paidInvoices);
        }
    }
}
