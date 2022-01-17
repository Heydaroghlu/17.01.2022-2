using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.Controllers
    
{
    
    [Area("AdminPanel")]
    [Authorize(Roles ="SuperAdmin,Admin")]
    public class DashboardController: Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
      
        /*public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser
            {
                UserName = "Admin",
                FullName = "Admin"
            };
            var result = await _userManager.CreateAsync(appUser, "Admin123");
            return Ok(result);
        }*/
    }
    
}
