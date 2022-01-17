using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class AppUser:IdentityUser
        
    {
        public bool IsAdmin { get; set; }
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }
      
        [Required]
        public DateTime BorthDate { get; set; }
        public List<BookComment> BookComments { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<Order> Orders { get; set; }
    }
}
