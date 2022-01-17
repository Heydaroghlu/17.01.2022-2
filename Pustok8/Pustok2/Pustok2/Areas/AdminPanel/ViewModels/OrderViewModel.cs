using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Areas.AdminPanel.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItem { get; set; }
    }
}
