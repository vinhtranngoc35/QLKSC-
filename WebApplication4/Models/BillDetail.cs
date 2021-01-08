using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class BillDetail
    {
        [Key]
        public int ID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? Total { get; set; }
        public int BookingDetailId { get; set; }
        [ForeignKey("BookingDetailId")]
        public BookingDetail BookingDetail { get; set; }
        
    }
}
