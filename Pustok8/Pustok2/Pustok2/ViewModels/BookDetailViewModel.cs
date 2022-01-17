using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class BookDetailViewModel
    {
        public BookComment Comment { get; set; }

        public Book Book { get; set; }
        public List<Book> RelatedBooks { get; set; }
    }
}
