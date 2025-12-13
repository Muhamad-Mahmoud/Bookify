using Bookify.BL.Interfaces;
using Bookify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bookify.BL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<Customer> _userManager;

        public CustomerService(UserManager<Customer> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> DeleteCustomerAsync(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
                return false;

            var result = await _userManager.DeleteAsync(customer);
            return result.Succeeded;
        }
    }
}
