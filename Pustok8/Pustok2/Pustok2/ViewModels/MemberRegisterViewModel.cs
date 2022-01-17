using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required]
        [StringLength(maximumLength:20)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string UserName { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email{get;set;}
        [Required]
        [StringLength(maximumLength:30,MinimumLength =10)]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BorthDate { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength=8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 8)]
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}
