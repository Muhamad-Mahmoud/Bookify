using Bookify.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> GetAllHotelsAsync(Expression<Func<Hotel, bool>>? filter = null, string? includeProperties = null);
        Task<Hotel?> GetHotelByIdAsync(int id, string? includeProperties = null);
        Task<bool> AddHotelAsync(Hotel Hotel);
        Task<bool> UpdateHotelAsync(Hotel Hotel);
        Task<bool> DeleteHotelAsync(int id);
        Task<string?> UploadMainImage(IFormFile MainImageFile, int HotelId);
        Task<bool> AddHotelImages(List<IFormFile> Images, int HotelId);
        Task<bool> DeleteHotelImagesAsync(int hotelId);
        Task<bool> UpdateHotelImages(List<IFormFile> Images, int HotelId);
    }
}

