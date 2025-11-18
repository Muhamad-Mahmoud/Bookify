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
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HotelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync()
        {
            return await _unitOfWork.Hotels.GetAllAsync();
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id)
        {
            return await _unitOfWork.Hotels.GetAsync(id);
        }

        public async Task<bool> AddHotelAsync(Hotel Hotel)
        {
            if (Hotel == null)
                return false;

            await _unitOfWork.Hotels.AddAsync(Hotel);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateHotelAsync(Hotel Hotel)
        {
            if (Hotel == null)
                return false;

            _unitOfWork.Hotels.Update(Hotel);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteHotelAsync(int id)
        {
            var Hotel = await _unitOfWork.Hotels.GetAsync(id);
            if (Hotel == null)
                return false;

            _unitOfWork.Hotels.Remove(Hotel);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
