using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();
        Task<RoomType?> GetRoomTypeByIdAsync(int id);
        Task<bool> AddRoomTypeAsync(RoomType RoomType);
        Task<bool> UpdateRoomTypeAsync(RoomType RoomType);
        Task<bool> DeleteRoomTypeAsync(int id);
    }
}
