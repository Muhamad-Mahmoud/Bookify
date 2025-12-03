using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Bookify.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role)]
    public class CityController : Controller
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        public CityController(ICityService cityService, ICountryService countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _cityService.GetAllCitiesAsync(includeProperties:"Country");
            return View(cities);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            var countryList = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName
            }).ToList();

            CityVM viewModel = new CityVM
            {
                City = new City(),
                Countries = countryList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CityVM viewModel, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetAllCountriesAsync();
                var countryList = countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CountryName
                }).ToList();
                viewModel.Countries = countryList;
                return View(viewModel);
            }

            try
            {
                var isCityExist = await _cityService.GetCityAsync(u => u.Name == viewModel.City.Name && u.CountryId == viewModel.City.CountryId);
                if (isCityExist == null)
                {
                    var result = await _cityService.AddCityAsync(viewModel.City);
                    if (result)
                    {
                        // Upload image if provided
                        if (ImageFile != null && ImageFile.Length > 0)
                        {
                            var imagePath = await _cityService.UploadCityImage(ImageFile, viewModel.City.Id);
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                viewModel.City.Image = imagePath;
                                await _cityService.UpdateCityAsync(viewModel.City);
                            }
                        }
                        
                        TempData["success"] = "City added successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["error"] = "Failed to add city.";
                }
                else
                {
                    TempData["error"] = "City Already Exist";
                }
            }
            catch
            {
                TempData["error"] = "Failed to add city.";
            }

            var countriesList = await _countryService.GetAllCountriesAsync();
            var countryListRetry = countriesList.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName
            }).ToList();
            viewModel.Countries = countryListRetry;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var city = await _cityService.GetCityByIdAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            var countries = await _countryService.GetAllCountriesAsync();
            var countryList = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName
            }).ToList();

            var vm = new CityVM
            {
                City = city,
                Countries = countryList
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CityVM vm, IFormFile? ImageFile)
        {
            if (id != vm.City?.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetAllCountriesAsync();
                vm.Countries = countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CountryName
                }).ToList();
                return View(vm);
            }

            if (vm.City != null)
            {
                // Upload new image if provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Pass the old image path to delete it
                    var imagePath = await _cityService.UploadCityImage(ImageFile, vm.City.Id, vm.City.Image);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        vm.City.Image = imagePath;
                    }
                }
                
                var result = await _cityService.UpdateCityAsync(vm.City);
                if (result)
                {
                    TempData["success"] = "City updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["error"] = "Failed to update city.";
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _cityService.GetCityByIdAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _cityService.GetCityByIdAsync(id);
            if (city != null)
            {
                var result = await _cityService.DeleteCityAsync(id);
                if (result)
                {
                    TempData["success"] = "City deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["error"] = "Failed to delete city.";
            return RedirectToAction(nameof(Index));
        }
    }
}


