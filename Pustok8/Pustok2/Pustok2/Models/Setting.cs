using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class Setting : BaseEntity
    {
        public string HeaderLogo { get; set; }
        public string FooterLogo { get; set; }
        public string SupportPhone { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
