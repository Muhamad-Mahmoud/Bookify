using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return View(countries);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }

            try
            {
                var isCountryExist = await _countryService.GetCountryAsync(u => u.CountryName == country.CountryName);
                if (isCountryExist == null)
                {
                    await _countryService.AddCountryAsync(country);
                    TempData["success"] = "Country added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Fail"] = "Country Already Exist";
            }
            catch
            {
                TempData["Fail"] = "Country Already Exist";
            }

            return View(country);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            ViewBag.CountriesList = AllCountries.CountryList;
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.CountriesList = AllCountries.CountryList;
                return View(country);
            }

            var result = await _countryService.UpdateCountryAsync(country);
            if (result)
            {
                TempData["success"] = "Country updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update country.";
            return View(country);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            if (country != null)
            {
                var result = await _countryService.DeleteCountryAsync(id);
                if (result)
                {
                    TempData["success"] = "Country deleted successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["error"] = "Failed to delete country.";
            return RedirectToAction(nameof(Index));
        }
    }
}
