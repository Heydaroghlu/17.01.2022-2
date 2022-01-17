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
    public class TagController : Controller
    {
        DataContext _context;
        public TagController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Tags.Include(x=>x.BookTags).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tag)
        {
              if(!ModelState.IsValid)
            {
                return View();
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            return View(_context.Tags.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Tag tag)
        {
            Tag oldtag = _context.Tags.FirstOrDefault(x => x.Id == tag.Id);
            if (oldtag==null)
            {
                return NotFound();
            }
            oldtag.Name = tag.Name;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete2(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
