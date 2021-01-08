using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class CreateBooking
    {
        public Customer Customer { get; set; }
        public Booking Booking { get; set; }
        public int[] TypeRoomID { get; set; }
        public int[] NumberRoom { get; set; }
        public int[] PriceRoom { get; set; }
    }
}
