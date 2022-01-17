using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles ="SuperAdmin,Admin")]

    public class CommentController : Controller
    {
        private DataContext _context;
        public CommentController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.BookComments.Include(x => x.Book).ThenInclude(x => x.Author).ToList());
        }
        public IActionResult CommentUpdate(int id)
        {
            return View(_context.BookComments.Include(x => x.Book).ThenInclude(x => x.Author).FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CommentUpdate(BookComment bookComment)
        {
            BookComment oldComment = _context.BookComments.FirstOrDefault(x => x.Id == bookComment.Id);
            if (oldComment == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            oldComment.FullName = bookComment.FullName;
            oldComment.Rate = bookComment.Rate;
            oldComment.Status = bookComment.Status;
            oldComment.CommentTime = bookComment.CommentTime;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete2(int id)
        {
            BookComment comment = _context.BookComments.FirstOrDefault(x => x.Id == id);
            _context.BookComments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
