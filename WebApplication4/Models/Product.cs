using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Tên Sản Phẩm")]
        public string Name { get; set; }
        [Display(Name = "Giá")]
        public int Price { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "Loại")]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        [Display(Name = "Loại")]
        public Type Type { get; set; }

        [Display(Name="Giá Sản Phẩm")]
        public int PriceProduct { get; set; }
        [Display(Name = "Công Ty")]
        public string Company { get; set; }
        public string UrlImage { get; set; }
      
        public string Unit { get; set; }
        public int Quantity { get; set; }
       
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public virtual ICollection<EnterProduct> EnterProducts { get; set; }
    }
}
