using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _unitOfWork.Countries.GetAllAsync();
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            return await _unitOfWork.Countries.GetAsync(id);
        }
        public async Task<Country> GetCountryAsync(Expression<Func<Country, bool>> filter)
        {

            return await _unitOfWork.Countries.GetAsync(filter);
        }

        public async Task<bool> AddCountryAsync(Country country)
        {
            if (country == null)
                return false;

            await _unitOfWork.Countries.AddAsync(country);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateCountryAsync(Country country)
        {
            if (country == null)
                return false;

            _unitOfWork.Countries.Update(country);
            await _unitOfWork.SaveAsync();
            return true;
        }

       
        public async Task<bool> DeleteCountryAsync(int id)
        {
            var country = await _unitOfWork.Countries.GetAsync(id);
            if (country == null)
                return false;

            _unitOfWork.Countries.Remove(country);
            await _unitOfWork.SaveAsync();
            return true;
        }


    }
}
