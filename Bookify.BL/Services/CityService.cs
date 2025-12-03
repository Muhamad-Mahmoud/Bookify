
using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Bookify.BL.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CityService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync(Expression<Func<City, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Cities.GetAllAsync(filter, includeProperties);
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
            var city = await _unitOfWork.Cities.GetAsync(id);
            if (city == null)
                return false;

            // Delete city image if exists
            if (!string.IsNullOrEmpty(city.Image))
            {
                DeleteCityImageFile(city.Image);
            }

            _unitOfWork.Cities.Remove(city);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<string?> UploadCityImage(IFormFile imageFile, int cityId, string? oldImagePath = null)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = $"city_{cityId}_{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string cityImagesPath = Path.Combine("images", "Cities");
            string finalPath = Path.Combine(wwwRootPath, cityImagesPath);

            // Create directory if it doesn't exist
            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            // Delete old image if exists
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                DeleteCityImageFile(oldImagePath);
            }

            string fullFilePath = Path.Combine(finalPath, fileName);

            // Save the new image
            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the relative URL path 
            return $"/images/Cities/{fileName}";
        }

        private void DeleteCityImageFile(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string cleanPath = imagePath.TrimStart('\\', '/').Replace("/", "\\");
            string fullPath = Path.Combine(wwwRootPath, cleanPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
