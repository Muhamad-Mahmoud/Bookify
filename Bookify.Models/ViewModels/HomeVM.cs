using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Models.ViewModels
{
    public class HomeVM
    {
        // Section 1: Featured Hotels
        public IEnumerable<Hotel> FeaturedHotels { get; set; } = new List<Hotel>();

        // Section 2: Hotels in a Specific City (e.g., Giza)
        public string? FeaturedCityName { get; set; }
        public IEnumerable<Hotel> CityHotels { get; set; } = new List<Hotel>();

        // Section 3: Cities Gallery
        public IEnumerable<City> FeaturedCities { get; set; } = new List<City>();
    }
}
