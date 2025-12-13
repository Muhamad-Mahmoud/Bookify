// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bookify.Models;
using Bookify.DL.Repository.IRepository;

namespace Bookify.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IWebHostEnvironment environment,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _unitOfWork = unitOfWork;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public int ReservationCount { get; set; }
        public int ReviewCount { get; set; }
        public int FavoriteCount { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Address")]
            [StringLength(200)]
            public string Address { get; set; }

            [Display(Name = "Profile Image")]
            public IFormFile ProfileImage { get; set; }
        }

        private async Task LoadAsync(Customer user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Email = email;
            ProfileImageUrl = user.PersonalImgUrl ?? "/images/default-avatar.png";
            
            // Fetch real stats
            var reservations = await _unitOfWork.Reservations.GetAllAsync(r => r.CustomerId == user.Id);
            ReservationCount = reservations.Count();

            var reviews = await _unitOfWork.Reviews.GetAllAsync(r => r.CustomerId == user.Id);
            ReviewCount = reviews.Count();
            
            FavoriteCount = 0; // Feature implementation coming soon

            Input = new InputModel
            {
                Name = user.Name,
                PhoneNumber = phoneNumber,
                Address = user.Address
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Update Name
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            // Update Phone
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error: Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Update Address
            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            // Upload Profile Image
            if (Input.ProfileImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = $"{user.Id}_{Guid.NewGuid()}{Path.GetExtension(Input.ProfileImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfileImage.CopyToAsync(fileStream);
                }

                user.PersonalImgUrl = $"/images/profiles/{uniqueFileName}";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Error: Unexpected error when trying to update profile.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated successfully!";
            return RedirectToPage();
        }
    }
}
