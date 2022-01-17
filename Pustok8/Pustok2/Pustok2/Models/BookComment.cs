using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class BookComment
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        [StringLength(maximumLength: 500)]
        public string Text { get; set; }
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        public string AppUserId { get; set; }
        [Required]
        [Range(1,5)]
        public int Rate { get; set; }
        public bool Status { get; set; }
        public DateTime CommentTime { get; set; }
        public Book Book { get; set; }
        public AppUser AppUser { get; set; }
      
    }
}
