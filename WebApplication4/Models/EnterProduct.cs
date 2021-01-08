using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class EnterProduct
    {
        [Key]
        public int ID { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsDelete { get; set; }
        public int UId { get; set; }
        public int RemainingAmount { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}


