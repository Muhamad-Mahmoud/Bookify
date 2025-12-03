using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Models.ViewModels
{
    public class HotelVM
    {
        public Hotel hotel {  get; set; }   

        public List<SelectListItem>? CityList { get; set; }
    }
}
