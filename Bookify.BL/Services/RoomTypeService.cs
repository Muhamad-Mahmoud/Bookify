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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomImageService _roomImageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomTypeService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IRoomImageService roomImageService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _roomImageService = roomImageService;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(Expression<Func<RoomType, bool>>? filter = null, string? includeProperties = null)
        {
            return await _unitOfWork.RoomTypes.GetAllAsync(filter, includeProperties);
        }

        // Get All Room Types With Availability 
        public async Task<IEnumerable<RoomType>> GetRoomTypesWithAvailabilityAsync(
                int hotelId, DateTime checkIn, DateTime checkOut)
        {
            var roomTypes = await _unitOfWork.RoomTypes.GetAllAsync(
                rt => rt.HotelId == hotelId,
                includeProperties: "Rooms"
            );

            var overlappingReservations = await _unitOfWork.Reservations.GetAllAsync(
                r => r.Status != ReservationStatus.Cancelled
                     && r.CheckInDate < checkOut
                     && r.CheckOutDate > checkIn,
                includeProperties: "RoomReserved"
            );

            foreach (var rt in roomTypes)
            {
                var roomIds = rt.Rooms.Select(r => r.Id).ToList();

                var bookedRoomIds = overlappingReservations
                    .SelectMany(r => r.RoomReserved)
                    .Where(rr => roomIds.Contains(rr.RoomId))
                    .Select(rr => rr.RoomId)
                    .ToHashSet();

                rt.AvailableRoomsCount = roomIds.Count(id => !bookedRoomIds.Contains(id));
            }

            return roomTypes;
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

            // Delete all room type images
            await DeleteRoomImagesAsync(id);

            _unitOfWork.RoomTypes.Remove(roomType);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<string?> UploadMainImage(IFormFile MainImageFile, int RoomTypeId)
        {
            if (MainImageFile == null || MainImageFile.Length == 0)
                return null;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = $"main_{Guid.NewGuid()}{Path.GetExtension(MainImageFile.FileName)}";
            string productPath = Path.Combine("images", "RoomTypes", $"RoomType-{RoomTypeId}");
            string finalPath = Path.Combine(wwwRootPath, productPath);

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            string fullFilePath = Path.Combine(finalPath, fileName);

            using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
            {
                await MainImageFile.CopyToAsync(fileStream);
            }

            // Return the relative URL path
            return Path.Combine("\\", productPath, fileName);
        }

        public async Task<bool> DeleteRoomImagesAsync(int roomTypeId)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string roomPath = Path.Combine("images", "RoomTypes", $"RoomType-{roomTypeId}");
            string finalPath = Path.Combine(wwwRootPath, roomPath);

            bool dbDeleted = true;

            // Delete images from database
            var roomImages = await _unitOfWork.RoomImages.GetAllAsync(ri => ri.RoomTypeId == roomTypeId);

            foreach (var roomImage in roomImages)
            {
                bool deleted = await _roomImageService.DeleteRoomImageAsync(roomImage.Id);
                if (!deleted)
                {
                    dbDeleted = false;
                }
            }

            // Delete folder from disk 
            if (Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, true);
            }

            return dbDeleted;
        }


        public async Task<bool> AddRoomImages(List<IFormFile> Images, int RoomTypeId)
        {
            if (Images == null || Images.Count == 0)
                return false;

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            foreach (IFormFile file in Images)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine("images", "RoomTypes", $"RoomType-{RoomTypeId}");
                string finalPath = Path.Combine(wwwRootPath, productPath);

                if (!Directory.Exists(finalPath))
                    Directory.CreateDirectory(finalPath);

                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                RoomImage roomImage = new()
                {
                    ImageUrl = Path.Combine("\\", productPath, fileName),
                    RoomTypeId = RoomTypeId
                };

                await _roomImageService.AddRoomImageAsync(roomImage);
            }

            return true;
        }

        public async Task<bool> UpdateRoomImages(List<IFormFile> Images, int RoomTypeId)
        {
            if (Images == null || Images.Count == 0)
                return true;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string productPath = Path.Combine("images", "RoomTypes", $"RoomType-{RoomTypeId}");
            string finalPath = Path.Combine(wwwRootPath, productPath);

            // Delete old images from database
            var existingImages = await _unitOfWork.RoomImages.GetAllAsync(ri => ri.RoomTypeId == RoomTypeId);
            foreach (var existingImage in existingImages)
            {
                await _roomImageService.DeleteRoomImageAsync(existingImage.Id);
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

                RoomImage roomImage = new()
                {
                    ImageUrl = Path.Combine("\\", productPath, fileName),
                    RoomTypeId = RoomTypeId
                };

                await _roomImageService.AddRoomImageAsync(roomImage);
            }

            return true;
        }
    }
}
