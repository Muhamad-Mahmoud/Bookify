using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IRoomImageService
    {
        Task<IEnumerable<RoomImage>> GetAllRoomImagesAsync();
        Task<RoomImage?> GetRoomImageByIdAsync(int id);
        Task<bool> AddRoomImageAsync(RoomImage roomImage);
        Task<bool> UpdateRoomImageAsync(RoomImage roomImage);
        Task<bool> DeleteRoomImageAsync(int id);
    }
}
