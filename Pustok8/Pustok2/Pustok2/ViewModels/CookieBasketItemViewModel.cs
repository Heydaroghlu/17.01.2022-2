using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class CookieBasketItemViewModel
    {
        public int BookId { get; set; }
        public string BookImage { get; set; }
        public string BookTitle { get; set; }
        public string BookDesc { get; set; }
        public decimal BookPrice { get; set; }
        public int Count { get; set; }

    }
}
