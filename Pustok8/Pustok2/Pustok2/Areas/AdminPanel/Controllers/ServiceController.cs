using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pustok2.Models;

namespace Pustok2.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ServiceController : Controller
    {
        private DataContext _context;
        public ServiceController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Settings.ToList());
        }
        public IActionResult Update(int id)
        {
            return View(_context.Settings.FirstOrDefault(x=>x.Id==id));
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public IActionResult Update(Setting setting)
        {
            Setting oldsetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);
            oldsetting.HeaderLogo = setting.HeaderLogo;
            oldsetting.FooterLogo = setting.FooterLogo;
            oldsetting.Address = setting.Address;
            oldsetting.Email = setting.Email;
            oldsetting.SupportPhone = setting.SupportPhone;
            oldsetting.Phone = setting.Phone;
            _context.SaveChanges();
            return RedirectToAction("index");
            
        }
    }
}
