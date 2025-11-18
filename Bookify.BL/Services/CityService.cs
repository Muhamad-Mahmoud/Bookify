
using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using System.Linq.Expressions;

namespace Bookify.BL.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(Expression<Func<City, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Cities.GetAllAsync(filter, includeProperties );
        }

        public async Task<City?> GetCityByIdAsync(int id)
        {
            return await _unitOfWork.Cities.GetAsync(id);
        }

        public async Task<City> GetCityAsync(Expression<Func<City, bool>> filter)
        {
            return await _unitOfWork.Cities.GetAsync(filter);
        }

        public async Task<bool> AddCityAsync(City city)
        {
            if (city == null)
                return false;

            await _unitOfWork.Cities.AddAsync(city);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateCityAsync(City city)
        {
            if (city == null)
                return false;

            _unitOfWork.Cities.Update(city);
            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<bool> DeleteCityAsync(int id)
        {
            var City = await _unitOfWork.Cities.GetAsync(id);
            if (City == null)
                return false;

            _unitOfWork.Cities.Remove(City);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
