using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Utility;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();

                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "abrarf@gmail.com",
                        Email = "abrarf@gmail.com",
                        Name = "Abrar Farooqui",
                        NormalizedUserName = "ABRAR@GMAIL.COM",
                        NormalizedEmail = "ABRAR@GMAIL.COM",
                        PhoneNumber = "1234567890",
                    }, "Anjuman@123").GetAwaiter().GetResult();

                    ApplicationUser user = _context.ApplicationUser.FirstOrDefault(u => u.Email == "abrarf@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
                

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
