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
    public class RoomImageService : IRoomImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RoomImage>> GetAllRoomImagesAsync()
        {
            return await _unitOfWork.RoomImages.GetAllAsync();
        }

        public async Task<RoomImage?> GetRoomImageByIdAsync(int id)
        {
            return await _unitOfWork.RoomImages.GetAsync(id);
        }

        public async Task<bool> AddRoomImageAsync(RoomImage roomImage)
        {
            if (roomImage == null)
                return false;

            await _unitOfWork.RoomImages.AddAsync(roomImage);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateRoomImageAsync(RoomImage roomImage)
        {
            if (roomImage == null)
                return false;

            _unitOfWork.RoomImages.Update(roomImage);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteRoomImageAsync(int id)
        {
            var roomImage = await _unitOfWork.RoomImages.GetAsync(id);
            if (roomImage == null)
                return false;

            _unitOfWork.RoomImages.Remove(roomImage);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
