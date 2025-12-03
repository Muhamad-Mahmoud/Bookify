using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHotelImageService _hotelImageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HotelService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IHotelImageService hotelImageService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _hotelImageService = hotelImageService;
        }

        public async Task<IEnumerable<Hotel>> GetAllHotelsAsync(Expression<Func<Hotel, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.Hotels.GetAllAsync(filter, includeProperties);
        }

        public async Task<Hotel?> GetHotelByIdAsync(int id, string? includeProperties = null)
        {
            return await _unitOfWork.Hotels.GetAsync( h => h.Id == id,includeProperties );
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

            // Delete all hotel images
            await DeleteHotelImagesAsync(id);

            _unitOfWork.Hotels.Remove(Hotel);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<string?> UploadMainImage(IFormFile MainImageFile, int HotelId)
        {
            if (MainImageFile == null || MainImageFile.Length == 0)
                return null;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = $"main_{Guid.NewGuid()}{Path.GetExtension(MainImageFile.FileName)}";
            string hotelPath = Path.Combine("images", "Hotels", $"Hotel-{HotelId}");
            string finalPath = Path.Combine(wwwRootPath, hotelPath);

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            string fullFilePath = Path.Combine(finalPath, fileName);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                await MainImageFile.CopyToAsync(fileStream);
            }

            // Return the URL path 
            return $"/images/Hotels/Hotel-{HotelId}/{fileName}";
        }

        public async Task<bool> DeleteHotelImagesAsync(int hotelId)
        {
            string root = _webHostEnvironment.WebRootPath;
            string hotelFolder = Path.Combine(root, "images", "Hotels", $"Hotel-{hotelId}");

            // Remove All Images From DB
            var images = await _unitOfWork.HotelImages.GetAllAsync(i => i.HotelId == hotelId);
            _unitOfWork.HotelImages.RemoveRange(images);
            await _unitOfWork.SaveAsync();

            // Remove All Imaes From Root 
            if (Directory.Exists(hotelFolder))
                Directory.Delete(hotelFolder, true);

            return true;
        }

        public async Task<bool> AddHotelImages(List<IFormFile> Images, int HotelId)
        {
            if (Images == null || Images.Count == 0)
                return false;

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            foreach (IFormFile file in Images)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string hotelPath = Path.Combine("images", "Hotels", $"Hotel-{HotelId}");
                string finalPath = Path.Combine(wwwRootPath, hotelPath);

                if (!Directory.Exists(finalPath))
                    Directory.CreateDirectory(finalPath);

                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                HotelImage hotelImage = new()
                {
                    ImageUrl = $"/images/Hotels/Hotel-{HotelId}/{fileName}",
                    HotelId = HotelId
                };

                await _hotelImageService.AddHotelImageAsync(hotelImage);
            }

            return true;
        }

        public async Task<bool> UpdateHotelImages(List<IFormFile> Images, int HotelId)
        {
            if (Images == null || Images.Count == 0)
                return true;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string hotelPath = Path.Combine("images", "Hotels", $"Hotel-{HotelId}");
            string finalPath = Path.Combine(wwwRootPath, hotelPath);

            // Delete old images from database
            var existingImages = await _unitOfWork.HotelImages.GetAllAsync(hi => hi.HotelId == HotelId);
            foreach (var existingImage in existingImages)
            {
                await _hotelImageService.DeleteHotelImageAsync(existingImage.Id);
            }

            // Delete old image files from disk
            if (Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, true);
            }

            // Create new directory
            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            // Upload new images
            foreach (IFormFile file in Images)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                HotelImage hotelImage = new()
                {
                    ImageUrl = $"/images/Hotels/Hotel-{HotelId}/{fileName}",
                    HotelId = HotelId
                };

                await _hotelImageService.AddHotelImageAsync(hotelImage);
            }

            return true;
        }
    }
}
