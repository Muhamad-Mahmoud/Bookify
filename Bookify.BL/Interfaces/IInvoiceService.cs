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
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync(string? ownerId = null);
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<bool> AddInvoiceAsync(Invoice Invoice);
    }
}
