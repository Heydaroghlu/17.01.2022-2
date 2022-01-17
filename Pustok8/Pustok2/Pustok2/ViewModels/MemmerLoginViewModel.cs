using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.ViewModels
{
    public class MemmerLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 50), MinLength(3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 30), MinLength(8)]
        public string Password { get; set; }
    }
}
