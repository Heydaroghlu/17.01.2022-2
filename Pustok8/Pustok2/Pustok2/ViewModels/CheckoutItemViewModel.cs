using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class CheckoutItemViewModel
    {
        public Book Book { get; set; }
        public int Count { get; set; }
    }
}
