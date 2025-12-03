using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(string? ownerId = null)
        {
            if (string.IsNullOrEmpty(ownerId)) // Admin
            {
                return await _unitOfWork.Invoices.GetAllAsync(includeProperties: "Customer,Hotel,Reservation");
            }
            else // Owner
            {
                return await _unitOfWork.Invoices.GetAllAsync(
                    i => i.Hotel.OwnerId == ownerId,
                    includeProperties: "Customer,Hotel,Reservation"
                );
            }
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _unitOfWork.Invoices.GetAsync(id);
        }

        public async Task<bool> AddInvoiceAsync(Invoice invoice)
        {
            if (invoice == null)
                return false;

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
