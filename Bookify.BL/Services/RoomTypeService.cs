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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _unitOfWork.RoomTypes.GetAllAsync();
        }

        public async Task<RoomType?> GetRoomTypeByIdAsync(int id)
        {
            return await _unitOfWork.RoomTypes.GetAsync(id);
        }

        public async Task<bool> AddRoomTypeAsync(RoomType roomType)
        {
            if (roomType == null)
                return false;

            await _unitOfWork.RoomTypes.AddAsync(roomType);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateRoomTypeAsync(RoomType roomType)
        {
            if (roomType == null)
                return false;

            _unitOfWork.RoomTypes.Update(roomType);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteRoomTypeAsync(int id)
        {
            var roomType = await _unitOfWork.RoomTypes.GetAsync(id);
            if (roomType == null)
                return false;

            _unitOfWork.RoomTypes.Remove(roomType);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
