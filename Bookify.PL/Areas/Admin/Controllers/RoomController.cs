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
    [Authorize(Roles = SD.Admin_Role + "," + SD.Owner_Role)]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IRoomTypeService roomTypeService, IHotelService hotelService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Room> Rooms;
            if (User.IsInRole(SD.Admin_Role))
            {
                Rooms = await _roomService.GetAllRoomsAsync(includeProperties: "RoomType.Hotel");
            }
            else
            {
                var userId = GetUserId();
                Rooms = await _roomService.GetAllRoomsAsync(r => r.RoomType.Hotel.OwnerId == userId, includeProperties: "RoomType.Hotel");
            }
            return View(Rooms);
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            IEnumerable<RoomType> RoomTypes;
            if (User.IsInRole(SD.Admin_Role))
            {
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            }
            else
            {
                var userId = GetUserId();
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync(rt => rt.Hotel.OwnerId == userId);
            }
            var RoomTypeList = RoomTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList();

            RoomVM viewModel = new RoomVM
            {
                room = new Room(),
                RoomTypeList = RoomTypeList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(RoomVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var addResult = await _roomService.AddRoomAsync(viewModel.room);
                if (addResult)
                {
                    TempData["success"] = "Room added successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Failed to add room.";
                }
            }

            var RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            var RoomTypeList = RoomTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList();

            viewModel.RoomTypeList = RoomTypeList;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            IEnumerable<RoomType> RoomTypes;
            if (User.IsInRole(SD.Admin_Role))
            {
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            }
            else
            {
                var userId = GetUserId();
                RoomTypes = await _roomTypeService.GetAllRoomTypesAsync(rt => rt.Hotel.OwnerId == userId);
            }
            var RoomTypeList = RoomTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == room.RoomTypeId
            }).ToList();

            RoomVM viewModel = new RoomVM
            {
                room = room,
                RoomTypeList = RoomTypeList
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomVM viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _roomService.UpdateRoomAsync(viewModel.room);
                if (result)
                {
                    TempData["success"] = "Room updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Failed to update room.";
            }

            var RoomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            var RoomTypeList = RoomTypes.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == viewModel.room?.RoomTypeId
            }).ToList();

            viewModel.RoomTypeList = RoomTypeList;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                TempData["error"] = "Room not found.";
                return RedirectToAction(nameof(Index));
            }

            // Delete room
            var roomDeleted = await _roomService.DeleteRoomAsync(id);

            if (roomDeleted)
            {
                TempData["success"] = "Room deleted successfully.";
            }
            else
            {
                TempData["error"] = "Failed to delete room.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
