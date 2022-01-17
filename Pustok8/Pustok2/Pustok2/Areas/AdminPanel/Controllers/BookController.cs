using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookController : Controller
    {
        private DataContext _context;
        public BookController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Books.Include(x => x.BookImages).Include(x=>x.Author).Include(x=>x.Genre).Include(x => x.BookTags).ThenInclude(X => X.Tag).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete2(int id)
        {
            Author deleteauthor = _context.Authors.FirstOrDefault(x => x.Id == id);
            _context.Authors.Remove(deleteauthor);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
   



    }
}
