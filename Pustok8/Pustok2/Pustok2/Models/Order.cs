using Pustok2.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok2.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Phone { get; set; }
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        public string Address1 { get; set; }
        [StringLength(maximumLength: 250)]
        public string Address2 { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string City { get; set; }
        [StringLength(maximumLength: 5)]
        public string CodePrefix { get; set; }
        public int CodeNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Zipcode { get; set; }
        [StringLength(maximumLength: 350)]
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatusEnum Status { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public OrderStatusDeliveryEnum DeliveryStatus { get; set; }
        public string Reject { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
