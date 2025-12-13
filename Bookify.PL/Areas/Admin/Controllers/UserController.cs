using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role)]
    public class UserController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly UserManager<Bookify.Models.Customer> _userManager;

        public UserController(ICustomerService customerService, UserManager<Bookify.Models.Customer> userManager)
        {
            _customerService = customerService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _customerService.GetAllCustomersAsync();
            
            // Get roles for each user
            var usersWithRoles = new List<(Bookify.Models.Customer User, IList<string> Roles)>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersWithRoles.Add((user, roles));
            }

            return View(usersWithRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _customerService.GetCustomerByIdAsync(id);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = string.Join(", ", roles);

            return View(user);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (result)
            {
                TempData["success"] = "User deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete user.";
            return RedirectToAction(nameof(Index));
        }
    }
}
