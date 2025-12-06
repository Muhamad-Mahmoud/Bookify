using Bookify.DL.Data;
using Bookify.Models;
using Bookify.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.DbInitializer
{
    public class DbInitializer : IDbInitializer  // Server Ready
    {
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BookifyDbContext _bookifyDbContext;

        public DbInitializer(UserManager<Customer> userManager,
            RoleManager<IdentityRole> roleManager,
            BookifyDbContext bookifyDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _bookifyDbContext = bookifyDbContext;
        }
        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_bookifyDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _bookifyDbContext.Database.Migrate();
                }
            }
            catch (Exception ex) { }



            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Cust_Role).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Cust_Role)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Owner_Role)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Admin_Role)).GetAwaiter().GetResult();


                //if roles are not created, then we will create admin user as well
                var adminUser = new Customer
                {
                    UserName = "admin@Bookify.com",
                    Email = "admin@Bookify.com",
                    Name = "Muhammad Mahmoud",
                    PhoneNumber = "1112223333",
                    Address = "test 123 Ave",
                    EmailConfirmed = true  
                };
                _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();

                Customer user = _bookifyDbContext.Customers.FirstOrDefault(u => u.Email == "admin@Bookify.com");
                _userManager.AddToRoleAsync(user, SD.Admin_Role).GetAwaiter().GetResult();

            }

            return;
        }
    }
}
