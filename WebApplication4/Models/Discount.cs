using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Discount
    {
        [Key]
        public int ID { get; set; }
        public int NumberDiscount { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeStop { get; set; }
        public int TypeRoomId { get; set; }
        [ForeignKey("TypeRoomId")]
        public virtual TypeRoom TypeRoom { get; set; }

    }
}
