using Bookify.DL.Data;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository.IRepository
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        void Update(Invoice invoice);
    }
}
