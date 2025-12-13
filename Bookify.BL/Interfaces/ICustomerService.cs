using Bookify.Models;

namespace Bookify.BL.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(string id);
        Task<bool> DeleteCustomerAsync(string id);
    }
}
