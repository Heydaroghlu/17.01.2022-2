using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class CheckoutViewModel
    {
        public Order Order { get; set; }
        public List<CheckoutItemViewModel> CheckoutItems { get; set; }
    }
}
