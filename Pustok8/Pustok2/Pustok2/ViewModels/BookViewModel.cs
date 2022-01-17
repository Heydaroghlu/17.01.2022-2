using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class BookViewModel
    { 
        public List<Genre> Genres { get; set; }
        public PagenatedList<Book> PagenatedBooks { get; set; }
    }
}
