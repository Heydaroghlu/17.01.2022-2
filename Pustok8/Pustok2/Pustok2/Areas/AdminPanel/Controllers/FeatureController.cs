using Microsoft.AspNetCore.Mvc;
using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class FeatureController : Controller
    {
        DataContext _context;
        public FeatureController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Features.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feature feature)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            _context.Features.Add(feature);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete2(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);
            _context.Features.Remove(feature);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            return View(_context.Features.FirstOrDefault(x => x.Id == id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Feature feature)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            Feature oldfeature = _context.Features.FirstOrDefault(x => x.Id == feature.Id);
            oldfeature.Title = feature.Title;
            oldfeature.Text = feature.Text;
            oldfeature.Icon = feature.Icon;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
