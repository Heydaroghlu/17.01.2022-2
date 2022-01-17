using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Models;
using Pustok2.Services;
using Pustok2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager,DataContext context,SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _emailService = emailService;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Register(MemberRegisterViewModel memberVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(memberVM.UserName);
            if(user!=null)
            {
                ModelState.AddModelError("UserName", "Bele bir User Movcutdur!!!");
                return View();
            }
            user = await _userManager.FindByEmailAsync(memberVM.Email);
            if(user!=null)
            {
                ModelState.AddModelError("UserName", "Bele bir Email Movcutdur!!!");
                return View();
            }
            user = new AppUser
            {
                Email = memberVM.Email,
                FullName = memberVM.FullName,
                UserName = memberVM.UserName,
                PhoneNumber=memberVM.PhoneNumber,
                BorthDate=memberVM.BorthDate,
                IsAdmin=false
                
            };
            var result = await _userManager.CreateAsync(user, memberVM.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("index", "home");
        }
        public IActionResult Login()
        {
            return View();
        }
       [HttpPost]
       [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(MemmerLoginViewModel memberLogin, string returnUrl)

        {
           
            AppUser member = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == memberLogin.UserName.ToUpper());
            if(member==null || member.IsAdmin==true)
            {
                ModelState.AddModelError("", "Ad veya sifre yanslihdir!!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(member, memberLogin.Password,false,false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Ad veya sifre yanslihdir!!");
                return View();
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Book");


        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = new MemberUpdateViewModel
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    UserName = user.UserName
                },
                Orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Book).Where(x => x.AppUserId == user.Id).ToList()
            };
            return View(profileVM);
        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(MemberUpdateViewModel memberVM)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            MemberProfileViewModel profileVM = new MemberProfileViewModel
            {
                Member = memberVM
            };

            if (!ModelState.IsValid)
            {
                return View(profileVM);
            }

            if (member.Email != memberVM.Email && _userManager.Users.Any(x => x.NormalizedEmail == memberVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email has already been taken");
                return View(profileVM);
            }

            if (member.UserName != memberVM.UserName && _userManager.Users.Any(x => x.NormalizedUserName == memberVM.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username has already been taken");
                return View(profileVM);
            }

            member.Email = memberVM.Email;
            member.FullName = memberVM.FullName;
            member.UserName = memberVM.UserName;


            var result = await _userManager.UpdateAsync(member);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(profileVM);
            }







            if (!string.IsNullOrWhiteSpace(memberVM.Password) && !string.IsNullOrWhiteSpace(memberVM.RepeatPassword))
            {
                if (memberVM.Password != memberVM.RepeatPassword)
                {
                    return View(profileVM);
                }

                if (!await _userManager.CheckPasswordAsync(member, memberVM.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is no correct");
                    return View(profileVM);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(member, memberVM.CurrentPassword, memberVM.Password);

                if (!passwordResult.Succeeded)
                {
                    foreach (var item in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(profileVM);
                }
            }


            await _signInManager.SignInAsync(member,true);

            return View(profileVM);
        }
        public IActionResult Forgot()
        {
            return View();
        }
       [HttpPost]
       public async Task<IActionResult> Forgot(ForgotPasswordViewModel forgotPassword)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if(user==null)
            {
                ModelState.AddModelError("Email", "Bele bir Email yoxdur!!!");
                return View();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("resetpassword", "account", new { email = user.Email, token},Request.Scheme);
            _emailService.Send(user.Email,"Change Password", "<a href='" + url + "'>Change Password</a>");
            return Ok(new { url });
        }
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            return View(resetPassword);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordViewModel resetPassword)
        {
            if(!ModelState.IsValid)
            {
                return View("ResetPassword",resetPassword);
            }
            AppUser user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if(user==null)
            {
                ModelState.AddModelError("Email", "Bele bir istifadeci yoxdur!!!");
                return View();
            }
            var result =await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if(!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View("ResetPassword", resetPassword);
            }
            return RedirectToAction("login");

        }
    }
    


}
