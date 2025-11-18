using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Models.ViewModels
{
    public class CityVM
    {
        public City? City { get; set; }
        public List<SelectListItem>? Countries { get; set; }
    }
}

