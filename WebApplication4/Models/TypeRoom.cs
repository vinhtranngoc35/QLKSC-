using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class TypeRoom
    {   
        [Key]
        public int ID { get; set; }
        [Display(Name ="Tên Loại Phòng ")]
        public string Name { get; set; }
        [Display(Name="Giá")]
        public int Price { get; set; }
        public bool IsDelete { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
