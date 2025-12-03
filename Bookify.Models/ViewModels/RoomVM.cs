using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Models.ViewModels
{
    public class RoomVM
    {
        public Room room { get; set; }

        public List<SelectListItem>? RoomTypeList { get; set; }
    }
}
