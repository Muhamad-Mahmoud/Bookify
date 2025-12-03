using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IHotelImageService
    {
        Task<IEnumerable<HotelImage>> GetAllHotelImagesAsync();
        Task<HotelImage?> GetHotelImageByIdAsync(int id);
        Task<bool> AddHotelImageAsync(HotelImage hotelImage);
        Task<bool> UpdateHotelImageAsync(HotelImage hotelImage);
        Task<bool> DeleteHotelImageAsync(int id);
    }
}
