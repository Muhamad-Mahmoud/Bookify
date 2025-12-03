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
    public class HotelImageService : IHotelImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelImage>> GetAllHotelImagesAsync()
        {
            return await _unitOfWork.HotelImages.GetAllAsync();
        }

        public async Task<HotelImage?> GetHotelImageByIdAsync(int id)
        {
            return await _unitOfWork.HotelImages.GetAsync(id);
        }

        public async Task<bool> AddHotelImageAsync(HotelImage hotelImage)
        {
            if (hotelImage == null)
                return false;

            await _unitOfWork.HotelImages.AddAsync(hotelImage);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateHotelImageAsync(HotelImage hotelImage)
        {
            if (hotelImage == null)
                return false;

            _unitOfWork.HotelImages.Update(hotelImage);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteHotelImageAsync(int id)
        {
            var hotelImage = await _unitOfWork.HotelImages.GetAsync(id);
            if (hotelImage == null)
                return false;

            _unitOfWork.HotelImages.Remove(hotelImage);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
