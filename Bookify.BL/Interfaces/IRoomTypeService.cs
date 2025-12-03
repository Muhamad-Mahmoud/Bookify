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
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(Expression<Func<RoomType, bool>>? filter = null, string? includeProperties = null);
        Task<IEnumerable<RoomType>> GetRoomTypesWithAvailabilityAsync(int hotelId, DateTime checkIn, DateTime checkOut);
        Task<RoomType?> GetRoomTypeByIdAsync(int id);
        Task<bool> AddRoomTypeAsync(RoomType RoomType);
        Task<bool> UpdateRoomTypeAsync(RoomType RoomType);
        Task<bool> DeleteRoomTypeAsync(int id);
        
        /// <summary>
        /// Uploads and sets the main image for a room type
        /// </summary>
        /// <param name="MainImageFile">The main image file to upload</param>
        /// <param name="RoomTypeId">The room type ID</param>
        /// <returns>The URL path of the uploaded image, or null if failed</returns>
        Task<string?> UploadMainImage(IFormFile MainImageFile, int RoomTypeId);
        
        /// <summary>
        /// Adds gallery images for a room type
        /// </summary>
        /// <param name="Images">List of image files to upload</param>
        /// <param name="RoomTypeId">The room type ID</param>
        Task<bool> AddRoomImages(List<IFormFile> Images, int RoomTypeId);
        
        /// <summary>
        /// Deletes all gallery images for a room type
        /// </summary>
        /// <param name="roomTypeId">The room type ID</param>
        Task<bool> DeleteRoomImagesAsync(int roomTypeId);
        
        /// <summary>
        /// Updates gallery images for a room type
        /// </summary>
        /// <param name="Images">New list of image files</param>
        /// <param name="RoomTypeId">The room type ID</param>
        Task<bool> UpdateRoomImages(List<IFormFile> Images, int RoomTypeId);
    }
}

