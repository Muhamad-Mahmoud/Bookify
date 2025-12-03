using Bookify.BL.Interfaces;
using Bookify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Owner")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
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

            var invoices = await _invoiceService.GetAllInvoicesAsync(ownerId);
            return View(invoices);
        }
    }
}
