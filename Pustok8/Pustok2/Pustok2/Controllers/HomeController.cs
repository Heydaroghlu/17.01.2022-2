using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pustok2.Models;
using Pustok2.ViewModels;
/*using Pustok2.Models;
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeViewModel HomeVM = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                Features = _context.Features.ToList(),
                FeaturedBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsFeatured).Take(20).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.IsNew).Take(20).ToList(),
                DiscountedBooks = _context.Books.Include(x => x.Author).Include(x => x.BookImages).Where(x => x.DiscountPercent > 0).Take(20).ToList()

            };
            return View(HomeVM);
        }
        public IActionResult Partial()
        {
            return PartialView("_BookSlider", _context.Books.Include(x => x.Author).Include(x => x.BookImages).Take(20).ToList());
        }
        public IActionResult Detail(int id)
        {
            Book book = _context.Books.Include(x => x.BookImages).ToList().FirstOrDefault(x => x.Id == id);
            return View(book);
        }
        public IActionResult GetBook(int id)
        {
            Book book = _context.Books.Include(x => x.BookImages).Include(x => x.Genre).Include(x => x.BookTags).ThenInclude(x => x.Tag).FirstOrDefault(x => x.Id == id);
            return PartialView("_BookModal", book);
        }
        public IActionResult Register()
        {
            return View();
        }


    }
}
