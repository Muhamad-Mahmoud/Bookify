using Bookify.BL.Interfaces;
using Bookify.BL.Services;
using Bookify.DL.Repository.IRepository;
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
    [Authorize(Roles = SD.Admin_Role + "," + SD.Owner_Role)]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly ICityService _cityService;
        public HotelController(IHotelService hotelService, ICityService cityService)
        {
            _hotelService = hotelService;
            _cityService = cityService;
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Hotel> hotels;

            if (User.IsInRole("Admin"))
            {
                hotels = await _hotelService.GetAllHotelsAsync(includeProperties: "City,Owner");
            }
            else
            {
                hotels = await _hotelService.GetAllHotelsAsync(
                    u => u.OwnerId == GetUserId(),
                    includeProperties: "City"
                );
            }

            return View(hotels);
        }

        [HttpGet]
        public async Task <IActionResult> Add()
        {
            var Cities = await _cityService.GetAllCitiesAsync();
            var CityList = Cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            HotelVM viewModel = new HotelVM
            {
                hotel = new Hotel(),
                CityList = CityList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(HotelVM viewModel, IFormFile? MainImageFile, List<IFormFile>? GalleryImages)
        {
            if (ModelState.IsValid)
            {
                viewModel.hotel.OwnerId = GetUserId();
                var result = await _hotelService.AddHotelAsync(viewModel.hotel);
                if (result)
                {
                    // Upload main image if provided
                    if (MainImageFile != null && MainImageFile.Length > 0)
                    {
                        var mainImageUrl = await _hotelService.UploadMainImage(MainImageFile, viewModel.hotel.Id);
                        if (mainImageUrl != null)
                        {
                            viewModel.hotel.MainImage = mainImageUrl;
                            await _hotelService.UpdateHotelAsync(viewModel.hotel);
                        }
                    }

                    // Upload gallery images if provided
                    if (GalleryImages != null && GalleryImages.Count > 0)
                    {
                        await _hotelService.AddHotelImages(GalleryImages, viewModel.hotel.Id);
                    }

                    TempData["success"] = "Hotel added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Failed to add hotel.";
            }

            var Cities = await _cityService.GetAllCitiesAsync();
            viewModel.CityList = Cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id, includeProperties: "GalleryImages,City");

            var Cities = await _cityService.GetAllCitiesAsync();
            var CityList = Cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();

            HotelVM viewModel = new HotelVM
            {
                hotel = hotel,
                CityList = CityList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelVM viewModel, IFormFile? MainImageFile, List<IFormFile>? GalleryImages)
        {

            if (ModelState.IsValid)
            {
                // Upload new main image
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    var mainImageUrl = await _hotelService.UploadMainImage(MainImageFile, viewModel.hotel.Id);
                    if (mainImageUrl != null)
                    {
                        viewModel.hotel.MainImage = mainImageUrl;
                    }
                }

                var result = await _hotelService.UpdateHotelAsync(viewModel.hotel);
                if (result)
                {
                    // Update gallery images 
                    if (GalleryImages != null && GalleryImages.Count > 0)
                    {
                        await _hotelService.UpdateHotelImages(GalleryImages, viewModel.hotel.Id);
                    }

                    TempData["success"] = "Hotel updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Failed to update hotel.";
            }

            var Cities = await _cityService.GetAllCitiesAsync();
            viewModel.CityList = Cities.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == viewModel.hotel?.CityId
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);

            var result = await _hotelService.DeleteHotelAsync(id);
            if (result)
            {
                TempData["success"] = "Hotel deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete hotel.";
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = SD.Admin_Role)]
        public async Task<IActionResult> Approve(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);

            hotel.Status = HotelStatus.Approved;
            await _hotelService.UpdateHotelAsync(hotel);

            TempData["success"] = "Hotel approved successfully.";
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = SD.Admin_Role)]
        public async Task<IActionResult> Reject(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);

            hotel.Status = HotelStatus.Rejected;
            await _hotelService.UpdateHotelAsync(hotel);

            TempData["success"] = "Hotel Rejected.";
            return RedirectToAction(nameof(Index));
        }
    }
}
