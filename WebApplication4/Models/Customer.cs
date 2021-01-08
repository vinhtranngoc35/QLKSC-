using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        public string IdentityCard { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsDelete { get; set; }
        [InverseProperty("ID")]
        [ForeignKey("BookingDetail")]
        public int? BookingDetailId { get; set; }
        
        public virtual BookingDetail BookingDetail { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
