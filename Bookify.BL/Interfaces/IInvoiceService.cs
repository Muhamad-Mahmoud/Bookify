using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<bool> AddInvoiceAsync(Invoice Invoice);
        Task<bool> UpdateInvoiceAsync(Invoice Invoice);
        Task<bool> DeleteInvoiceAsync(int id);
    }
}
