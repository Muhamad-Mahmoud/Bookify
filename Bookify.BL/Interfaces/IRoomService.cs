using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync(Expression<Func<Room, bool>>? filter = null, string? includeProperties = null);
        Task<Room?> GetRoomByIdAsync(int id);
        Task<bool> AddRoomAsync(Room Room);
        Task<bool> UpdateRoomAsync(Room Room);
        Task<bool> DeleteRoomAsync(int id);
    }
}
