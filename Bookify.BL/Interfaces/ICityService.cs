using Bookify.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<City>> GetAllCitiesAsync(Expression<Func<City, bool>>? filter = null, string? includeProperties = null);
        Task<City?> GetCityByIdAsync(int id);
        Task<City> GetCityAsync(Expression<Func<City, bool>> filter);
        Task<bool> AddCityAsync(City City);
        Task<bool> UpdateCityAsync(City City);
        Task<bool> DeleteCityAsync(int id);
        
        Task<string?> UploadCityImage(IFormFile imageFile, int cityId, string? oldImagePath = null);
    }
}
