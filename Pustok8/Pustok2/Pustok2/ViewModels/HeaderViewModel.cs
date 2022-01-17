using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class HeaderViewModel
    {
        public List<Genre> Genres { get; set; }
        public List<Book> Books { get; set; }
        public AppUser AppUser { get; set; }
    }
}
