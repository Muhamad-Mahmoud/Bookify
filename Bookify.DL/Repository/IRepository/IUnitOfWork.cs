using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public ICityRepository Cities { get; }
        public ICompanyRepository Companies { get; }
        public ICountryRepository Countries { get; }
        public ICustomerRepository Customers { get; }
        public IHotelRepository Hotels { get; }
        public IInvoiceRepository Invoices { get; }
        public IReservationRepository Reservations { get; }
        public IRoomRepository Rooms { get; }
        public IRoomTypeRepository RoomTypes { get; }
        public IReservedRoomRepository reservedRooms { get; }

        public Task SaveAsync();
    }
}
