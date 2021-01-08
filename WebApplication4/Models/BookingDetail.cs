using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class BookingDetail
    {
        [Key]
        public int ID { get; set; }
        
        public DateTime CheckInExpected { get; set; }
    
        public DateTime CheckOutExpected { get; set; }

        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int? Price { get; set; }
        
        public int? NumberDate { get; set; }
        public int? NumberDateDiscount { get; set; }
        public int? PriceDiscount { get; set; }
        public int? Discount { get; set; }
        public int? Total { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public bool? IsDelete { get; set; }
        public int? PriceRoom { get; set; }
        public int? PriceExtra { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
