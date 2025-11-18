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
    public class RoomService :IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _unitOfWork.Rooms.GetAllAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _unitOfWork.Rooms.GetAsync(id);
        }

        public async Task<bool> AddRoomAsync(Room room)
        {
            if (room == null)
                return false;

            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateRoomAsync(Room room)
        {
            if (room == null)
                return false;

            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _unitOfWork.Rooms.GetAsync(id);
            if (room == null)
                return false;

            _unitOfWork.Rooms.Remove(room);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
