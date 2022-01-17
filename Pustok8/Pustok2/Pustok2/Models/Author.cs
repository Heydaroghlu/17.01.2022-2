using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class Author : BaseEntity
    {
        [StringLength(maximumLength: 50,ErrorMessage ="Umumi Uzunluq 50 den cox ola bilmez!")]
        public string FullName { get; set; }

        public List<Book> Books { get; set; }
    }
}
