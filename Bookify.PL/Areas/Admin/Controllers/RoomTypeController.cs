using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Bookify.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Owner")]
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _roomTypeService;
        private readonly IHotelService _hotelService;

        public RoomTypeController(IRoomTypeService roomTypeService, IHotelService hotelService)
        {
            _roomTypeService = roomTypeService;
            _hotelService = hotelService;
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<RoomType> roomTypes;
            if (User.IsInRole("Admin"))
            {
                roomTypes = await _roomTypeService.GetAllRoomTypesAsync(includeProperties: "Hotel");
            }
            else
            {
                var userId = GetUserId();
                roomTypes = await _roomTypeService.GetAllRoomTypesAsync(rt => rt.Hotel.OwnerId == userId, includeProperties: "Hotel");
            }
            return View(roomTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var Hotels = await _hotelService.GetAllHotelsAsync(
                u => u.OwnerId == GetUserId() && u.Status == HotelStatus.Approved
            );
            var HotelList = Hotels.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList();

            RoomTypeVM viewModel = new RoomTypeVM
            {
                RoomType = new RoomType(),
                Hotels = HotelList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoomTypeVM viewModel, IFormFile? MainImageFile, List<IFormFile>? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                var result = await _roomTypeService.AddRoomTypeAsync(viewModel.RoomType);
                if (result)
                {
                    // Upload main image 
                    if (MainImageFile != null && MainImageFile.Length > 0)
                    {
                        var mainImageUrl = await _roomTypeService.UploadMainImage(MainImageFile, viewModel.RoomType.Id);
                        if (mainImageUrl != null)
                        {
                            viewModel.RoomType.MainImage = mainImageUrl;
                            await _roomTypeService.UpdateRoomTypeAsync(viewModel.RoomType);
                        }
                    }

                    // Upload gallery images 
                    if (GalleryImages != null && GalleryImages.Count > 0)
                    {
                        await _roomTypeService.AddRoomImages(GalleryImages, viewModel.RoomType.Id);
                    }

                    TempData["success"] = "Room Type added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Failed to add room type.";
            }

            var Hotels = await _hotelService.GetAllHotelsAsync(u => u.OwnerId == GetUserId());
            viewModel.Hotels = Hotels.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            var Hotels = await _hotelService.GetAllHotelsAsync(
                u => u.OwnerId == GetUserId() && u.Status == HotelStatus.Approved
            );
            var HotelList = Hotels.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == roomType.HotelId
            }).ToList();

            RoomTypeVM viewModel = new RoomTypeVM
            {
                RoomType = roomType,
                Hotels = HotelList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomTypeVM viewModel, IFormFile? MainImageFile, List<IFormFile>? GalleryImages)
        {
            if (id != viewModel.RoomType?.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                // Upload new main image 
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    var mainImageUrl = await _roomTypeService.UploadMainImage(MainImageFile, viewModel.RoomType.Id);
                    if (mainImageUrl != null)
                    {
                        viewModel.RoomType.MainImage = mainImageUrl;
                    }
                }

                var result = await _roomTypeService.UpdateRoomTypeAsync(viewModel.RoomType);
                if (result)
                {
                    // Update gallery images
                    if (GalleryImages != null && GalleryImages.Count > 0)
                    {
                        await _roomTypeService.UpdateRoomImages(GalleryImages, viewModel.RoomType.Id);
                    }

                    TempData["success"] = "Room Type updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Failed to update room type.";
            }

            var Hotels = await _hotelService.GetAllHotelsAsync(u => u.OwnerId == GetUserId());
            viewModel.Hotels = Hotels.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == viewModel.RoomType?.HotelId
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (roomType != null)
            {
                var result = await _roomTypeService.DeleteRoomTypeAsync(id);
                if (result)
                {
                    TempData["success"] = "Room Type deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["error"] = "Failed to delete room type.";
            return RedirectToAction(nameof(Index));
        }
    }
}
