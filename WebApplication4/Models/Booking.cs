using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Booking
    {   
        [Key]
        public int ID { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CreateTime { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool Status { get; set; }
        [System.ComponentModel.DefaultValue(0)]
        public int? Total { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public bool? IsDelete { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
        public int NumberRoom { get; set; }
        public int? PriceRoom { get; set; }
        public int? ExtraPrice { get; set; }
        
    }
}
