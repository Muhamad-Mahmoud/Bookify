using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Bookify.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Add(City city)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetAllCountriesAsync();
                var countryList = countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.CountryName
                }).ToList();
                ViewBag.Countries = countryList;
                return View(city);
            }

            try
            {
                var isCityExist = await _cityService.GetCityAsync(u => u.Name == city.Name && u.CountryId == city.CountryId);
                if (isCityExist == null)
                {
                    await _cityService.AddCityAsync(city);
                    TempData["success"] = "City added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Fail"] = "City Already Exist";
            }
            catch
            {
                TempData["Fail"] = "City Already Exist";
            }

            var countriesList = await _countryService.GetAllCountriesAsync();
            var countryListRetry = countriesList.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CountryName
            }).ToList();
            ViewBag.Countries = countryListRetry;

            return View(city);
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
        public async Task<IActionResult> Edit(int id, CityVM vm)
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


