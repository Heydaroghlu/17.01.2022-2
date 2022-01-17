using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok2.Models;
using Pustok2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pustok2.Services;

namespace Pustok2.Controllers
{
    public class BookController:Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public BookController(DataContext context,UserManager<AppUser> userManager,IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }
        public IActionResult Index(int? genreId,int page=1)
        {
            var books = _context.Books.Include(x => x.Genre).Include(x => x.Author).Include(x => x.BookImages).AsQueryable();
            ViewBag.GenreId = genreId;
            /*  if (genreId != null)
                  books = books.Where(x => x.GenreId == genreId);*/
            TempData["Min"] = books.Min(x => x.SalePrice);
            TempData["Max"] = books.Max(x => x.SalePrice);
            BookViewModel bookVM = new BookViewModel
            {

                Genres = _context.Genres.Include(x => x.Books).ToList(),

                PagenatedBooks = PagenatedList<Book>.Create(books, page, 2)



            };
            return View(bookVM);
        }
        public IActionResult Detail(int id)
        {

            Book book = _context.Books
                            .Include(x => x.BookImages).Include(x => x.Genre)
                            .Include(x => x.BookTags).ThenInclude(x => x.Tag)
                            .Include(x => x.Author).Include(x => x.BookComments)
                            .FirstOrDefault(x => x.Id == id);
            if (book == null) return NotFound();
            BookDetailViewModel bookDetailVM = new BookDetailViewModel
            {
                Book = book,
                Comment = new BookComment { BookId = book.Id },
                RelatedBooks = _context.Books
                .Include(x => x.BookImages).Include(x => x.Author)
                .Where(x => x.GenreId == book.GenreId).OrderByDescending(x => x.Id).Take(5).ToList()
            };
            return View(bookDetailVM);
        }
        [HttpPost]
        public async Task<IActionResult> Comment(BookComment comment)
        {
           
            Book book = _context.Books
                          .Include(x => x.BookImages).Include(x => x.Genre)
                          .Include(x => x.BookTags).ThenInclude(x => x.Tag)
                          .Include(x => x.Author).Include(x => x.BookComments)
                          .FirstOrDefault(x => x.Id == comment.BookId);
            if (book == null) return NotFound();
            BookDetailViewModel bookDetailVM = new BookDetailViewModel
            {
                Book = book,
                Comment = comment,
                RelatedBooks = _context.Books
                .Include(x => x.BookImages).Include(x => x.Author)
                .Where(x => x.GenreId == book.GenreId).OrderByDescending(x => x.Id).Take(5).ToList()
            };


            if (!ModelState.IsValid)
            {
                return View("Detail",bookDetailVM);
            }
            if(!_context.Books.Any(X=>X.Id==comment.BookId))
            {
                return NotFound();
            }
            if(!User.Identity.IsAuthenticated)
            {
                if(string.IsNullOrWhiteSpace(comment.Email))
                {
                    return NotFound();
                }
                if (string.IsNullOrWhiteSpace(comment.FullName))
                {
                    return NotFound();
                }
            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                comment.AppUserId = user.Id;
            }
            AppUser user2 = await _userManager.FindByNameAsync(User.Identity.Name);
            comment.Status = false;
            comment.FullName = user2.FullName;
            comment.Email = user2.Email;
            comment.CommentTime = System.DateTime.Now;
            _context.BookComments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("detail", new { Id = comment.BookId }); ;
           }
        public IActionResult SetSession(int id)
        {
            HttpContext.Session.SetString("bookId", id.ToString());
            return Content("elave olundy");
        }
        public IActionResult GetSession()
        {
            string Getses = HttpContext.Session.GetString("bookId");
            return Content(Getses);
        }
        public IActionResult SetCookie(int id)
        {
            HttpContext.Response.Cookies.Append("BookIdCookie", id.ToString());
            return Content("Cookie-ye elave olundu!");
        }
        public IActionResult ShowCookie()
        {
            string CookieStr = HttpContext.Request.Cookies["BookIdCookie"];
            return Content(CookieStr);
        }
     
        public async Task<IActionResult> AddBasket(int id)
        {
            if (!_context.Books.Any(x => x.Id == id))
            {
                return NotFound();
            }
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            if (user != null && user.IsAdmin == false)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == user.Id && x.BookId == id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        BookId = id,
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }

                _context.SaveChanges();

            }
            else
            {
                Book book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);
                BookImage bookImage = _context.BookImages.FirstOrDefault(x => x.BookId == id);
                List<CookieBasketItemViewModel> basketItems = new List<CookieBasketItemViewModel>();
                string cookieExists = HttpContext.Request.Cookies["basketItemsList"];
                if (cookieExists != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(cookieExists);
                }
                CookieBasketItemViewModel item = basketItems.FirstOrDefault(x => x.BookId == id);
                if (item != null)
                {
                    item.BookId = id;
                    item.Count++;
                    item.BookTitle = book.Name;
                    item.BookDesc = book.Desc;
                    item.BookPrice = book.CostPrice;
                    item.BookImage = bookImage.Image;
                }
                else
                {
                    item = new CookieBasketItemViewModel
                    {
                        BookId = id,
                        Count = 1,
                        BookTitle = book.Name,
                        BookDesc = book.Desc,
                        BookPrice = book.CostPrice,
                        BookImage = bookImage.Image

                    };
                    basketItems.Add(item);
                }
                var basketItemsStr = JsonConvert.SerializeObject(basketItems);
                HttpContext.Response.Cookies.Append("basketItemsList", basketItemsStr);
            }
                
            return RedirectToAction("index", "home");
        }
        public IActionResult ShowBasket()
        {
            string basketItemsStr = HttpContext.Request.Cookies["basketItemsList"];
            List<CookieBasketItemViewModel> booksList = new List<CookieBasketItemViewModel>();
            if (basketItemsStr != null)
            {
                booksList = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketItemsStr);

            }
            return Json(booksList);
        }
        public IActionResult RemoveBasket(int id)
        {
            BasketItem book = _context.BasketItems.FirstOrDefault(x => x.BookId == id);
            if(book==null)
            {
                return NotFound();
            }
            _context.BasketItems.Remove(book);
            _context.SaveChanges();
            return View();

        }
        public async Task<IActionResult> Checkout()
        {
            CheckoutViewModel checkoutVM = new CheckoutViewModel
            {
                CheckoutItems = await _getCheckoutItems(),
                Order = new Order()
            };

            return View(checkoutVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> Order(Order order)
        {
            AppUser user = null;
            List<CheckoutItemViewModel> checkoutItems = await _getCheckoutItems();
            if (User.Identity.IsAuthenticated)
            {
                user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            if (checkoutItems.Count == 0)
            {
                ModelState.AddModelError("", "Not selected product");
            }

            if (user == null && string.IsNullOrWhiteSpace(order.Email))
            {
                ModelState.AddModelError("Email", "Email is Required");
            }

            if (user == null && string.IsNullOrWhiteSpace(order.FullName))
            {
                ModelState.AddModelError("FullName", "FullName is Required");
            }

            if (!ModelState.IsValid)
            {
                return View("Checkout", new CheckoutViewModel() { CheckoutItems = checkoutItems, Order = order });
            }

            if (user != null)
            {
                order.Email = user.Email;
                order.FullName = user.FullName;
                order.AppUserId = user.Id;
            }

            var lastOrder = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault();
            order.CodePrefix = order.FullName[0].ToString().ToUpper() + order.Email[0].ToString().ToUpper();
            order.CodeNumber = lastOrder == null ? 1001 : lastOrder.CodeNumber + 1;
            order.AppUserId = user?.Id;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.Status = Enum.OrderStatusEnum.Panging;
            order.OrderItems = new List<OrderItem>();
            foreach (var item in checkoutItems)
            {
                OrderItem orderItem = new OrderItem()
                {
                    BookId = item.Book.Id,
                    Count = item.Count,
                    CostPrice = item.Book.CostPrice,
                    SalePrice = item.Book.SalePrice,
                    DiscountPercent = item.Book.DiscountPercent
                };
                order.TotalAmount += orderItem.DiscountPercent > 0 ? (orderItem.SalePrice * (1 - orderItem.DiscountPercent / 100)) * orderItem.Count : orderItem.SalePrice * orderItem.Count;

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
            if (user != null)
            {
                _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == user.Id));
                _context.SaveChanges();
            }
            else
            {
                HttpContext.Response.Cookies.Delete("basketItemList");
            }

            _emailService.Send(order.Email, "Pustok", "Tesekkurler");

            return RedirectToAction("profile", "account");
        }
        private async Task<List<CheckoutItemViewModel>> _getCheckoutItems()
        {
            List<CheckoutItemViewModel> checkoutItems = new List<CheckoutItemViewModel>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.Book).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in basketItems)
                {
                    CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                    {
                        Book = item.Book,
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            else
            {
                string basketItemsStr = HttpContext.Request.Cookies["basketItemList"];
                if (basketItemsStr != null)
                {
                    List<CookieBasketItemViewModel> basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemViewModel>>(basketItemsStr);

                    foreach (var item in basketItems)
                    {
                        CheckoutItemViewModel checkoutItem = new CheckoutItemViewModel
                        {
                            Book = _context.Books.FirstOrDefault(x => x.Id == item.BookId),
                            Count = item.Count
                        };
                        checkoutItems.Add(checkoutItem);
                    }
                }
            }
             

            return checkoutItems;
        }

        public IActionResult TrackOrder()
        {
            
            return View();
            
        }


        [HttpPost]
        public IActionResult TrackOrder(OrderTrackCodeViewModel trackCode)
        {
            if (!ModelState.IsValid) return View();

            trackCode.Order = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Book).FirstOrDefault(x => (x.CodePrefix + x.CodeNumber) == trackCode.Code);

            if (trackCode.Order == null)
            {
                ModelState.AddModelError("Code", "There is not any order with this code");
                return View();
            }

            return View(trackCode);
        }



    }
}

