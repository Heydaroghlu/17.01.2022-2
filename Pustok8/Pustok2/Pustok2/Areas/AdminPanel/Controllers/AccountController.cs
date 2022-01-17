using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Areas.AdminPanel.ViewModels;
using Pustok2.Models;
using Pustok2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
 
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager,DataContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Login()
        {
          /*  AppUser user= _userManager.FindByNameAsync("Admin").Result;
          var result= _userManager.AddToRoleAsync(user, "Admin").Result;*/
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel loginVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser admin = await _userManager.Users.FirstOrDefaultAsync(x=>x.NormalizedUserName==loginVM.UserName.ToUpper() && x.IsAdmin==true);
            if(admin==null)
            {
                ModelState.AddModelError("", "Username or Pasword is incorrect!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(admin, loginVM.Password, false, false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Pasword is incorrect!");
                return View();
            }
            return RedirectToAction("index", "dashboard");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
        



        /*  public async Task<IActionResult> CreateRole()
    {
        IdentityRole role1 = new IdentityRole("SuperAdmin");
        IdentityRole role2 = new IdentityRole("Admin");
        IdentityRole role3 = new IdentityRole("Member");
       await _roleManager.CreateAsync(role1);
       await _roleManager.CreateAsync(role2);
       await _roleManager.CreateAsync(role3);
        return Ok();
    }*/
    }
}
