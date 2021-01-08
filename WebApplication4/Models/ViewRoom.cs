using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class ViewRoom
    {
        public int ID { get; set; }
        public string NameRoom { get; set; }
        public string NameTypeRoom { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public List<string> NameCustomers { get; set; } 
        public bool Status { get; set; }
    }
}
