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
    public class AuthorController : Controller
    {
        private DataContext _context;
        public AuthorController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.PageCount = (int)Math.Ceiling(Convert.ToDouble(_context.Authors.Count()) / 8);
            ViewBag.SelectPage = page;
            return View(_context.Authors.Include(x=>x.Books).Skip((page-1)*8).Take((8)).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Author author)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            _context.Authors.Add(author);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
       /* public IActionResult Delete(int id)
        {
            Author author = null;
            author = _context.Authors.FirstOrDefault(x => x.Id == id);
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return RedirectToAction("index");
        }*/
      /* public IActionResult Delete(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id);
            return View(author);
        }*/
        public IActionResult Delete2(int id)
        {
            Author deleteauthor = _context.Authors.FirstOrDefault(x => x.Id == id);
            _context.Authors.Remove(deleteauthor);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Author author)
        {
            Author deleteauthor = _context.Authors.FirstOrDefault(x => x.Id == author.Id);
            _context.Authors.Remove(deleteauthor);
            _context.SaveChanges();
            return RedirectToAction("index");
        }*/
        public IActionResult Update(int id)
        {
            Author author = _context.Authors.FirstOrDefault(x => x.Id == id);
            return View(author);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]

        public IActionResult Update(Author author)
        {
           Author oldauthor=_context.Authors.FirstOrDefault(x => x.Id == author.Id);
            if(oldauthor==null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            oldauthor.FullName = author.FullName;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
