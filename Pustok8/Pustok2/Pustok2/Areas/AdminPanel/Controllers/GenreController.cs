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
    public class GenreController:Controller
    {
        DataContext _context;
        public GenreController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View(_context.Genres.Include(x=>x.Books).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete2(int id)
        {
            Genre deletegenre = _context.Genres.FirstOrDefault(x => x.Id == id);
            _context.Genres.Remove(deletegenre);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            Genre genre = _context.Genres.FirstOrDefault(x => x.Id == id);
            return View(genre);
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public IActionResult Update(Genre genre)
        {
            Genre oldgenre = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (oldgenre == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            oldgenre.Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
