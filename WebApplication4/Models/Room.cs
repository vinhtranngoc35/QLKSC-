using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class Room
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        
        public int TypeRoomId { get; set; }
        [ForeignKey("TypeRoomId")]
        [Display(Name = "Loại Phòng")]
        public virtual TypeRoom TypeRoom { get; set; }
        public bool IsDelete { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }

    }
}
