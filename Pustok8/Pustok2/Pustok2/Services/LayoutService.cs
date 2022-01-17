using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok2.Models;
using Pustok2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Services
{
    public class LayoutService
    {
        private readonly DataContext _context;
       
        private readonly IHttpContextAccessor _contextAccessor;
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signManager;
        public LayoutService(IHttpContextAccessor contextAccessor,DataContext context, UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _signManager = signInManager;
        }
        public List<CookieBasketItemViewModel> GetBasketItems()
        {
            var itemsStr = _contextAccessor.HttpContext.Request.Cookies["basketItemsList"];

            List<CookieBasketItemViewModel> items = new List<CookieBasketItemViewModel>();
            if (itemsStr != null)
            {
                items = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(itemsStr);
            }
            
            return items;
        }
        public List<BasketItem> GetBasketItems2()
        {
            
            List<BasketItem> items = _context.BasketItems.Include(x=>x.Book).Include(x => x.AppUser).Include(X=>X.Book.BookImages).ToList();
            
            return items;
        }
        public async Task<List<Setting>> GetSettings()
        {
            return await _context.Settings.ToListAsync();
        }
     
     
    }
}
