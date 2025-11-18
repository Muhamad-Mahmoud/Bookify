using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(int id);
        Task<bool> AddHotelAsync(Hotel Hotel);
        Task<bool> UpdateHotelAsync(Hotel Hotel);
        Task<bool> DeleteHotelAsync(int id);
    }
}
