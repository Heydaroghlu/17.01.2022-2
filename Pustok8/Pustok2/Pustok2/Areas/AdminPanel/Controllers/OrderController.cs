using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Areas.AdminPanel.ViewModels;
using Pustok2.Enum;
using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class OrderController : Controller
    {
        DataContext _context;
        public OrderController(DataContext context)
        {

            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var orders = _context.Orders.Include(x => x.AppUser).AsQueryable();

            return View(PagenatedList<Order>.Create(orders, page, 3));
        }
        public IActionResult Deatil(int id)
        {
            Order order = _context.Orders.Include(x => x.AppUser).Include(x => x.OrderItems).FirstOrDefault(x => x.Id == id);

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reject(Order order)
        {
            Order existOrder = _context.Orders.FirstOrDefault(x => x.Id == order.Id);

            if (order == null) return NotFound();

            if (string.IsNullOrWhiteSpace(order.Reject))
            {
                ModelState.AddModelError("", "Comment is Required");
                return View("detail", new { id = order.Id }); 
            }

            existOrder.Reject = order.Reject;
            existOrder.Status = (OrderStatusEnum)3;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
