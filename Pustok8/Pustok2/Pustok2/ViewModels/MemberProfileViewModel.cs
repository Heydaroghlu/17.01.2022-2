using Pustok2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class MemberProfileViewModel
    {
        public MemberUpdateViewModel Member { get; set; }
        public List<Order> Orders { get; set; }
        /* public string Username { get; set; }
         public string Fullname { get; set; }
         public string Email { get; set; }
         public DateTime BorthDate { get; set; }
         public string PhoneNumber { get; set; }
         [DataType(DataType.Password)]
         public string NewPassword { get; set; }
         [DataType(DataType.Password), Compare(nameof(NewPassword))]
         [StringLength(maximumLength: 20, MinimumLength = 8)]
         public string ConfirmNewPassword { get; set; }
         [DataType(DataType.Password)]
         [StringLength(maximumLength: 20, MinimumLength = 8)]
         public string CurrentPassword { get; set; }*/
    }
}
