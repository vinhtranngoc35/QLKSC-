using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication4.Models;


namespace WebApplication4.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [Route("Home/Rooms/api/[controller]")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        private readonly ProductContext _context;
        private string Connection = "Server=localhost,1433;Database=HotelDb;User=sa;Password=yourStrong(!)Password";
        public HomeApiController(ProductContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("Products")]
        public IActionResult GetAllProduct()
        {
            List<Product> products;
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                products = connection.Query<Product>("select * from Products").ToList();


            }
            return Ok(products);
        }
        [HttpPost]
        [Route("BillDetail")]
        public IActionResult SaveBillDetail(BillDetail billDetail)
        {
            
            _context.BillDetails.Add(billDetail);
            var b = _context.SaveChangesAsync();
            
            return NoContent();
        }
        [HttpPost]
        [Route("Customer")]
        public IActionResult SaveCustomer(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                var b = _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return NoContent();
            }

            
        }
        [HttpGet]
        [Route("Customers/{idBookingDetails}")]
        public IActionResult GetAllCustomer(int idBookingDetails)
        {
            List<Customer> CustomerList;
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                CustomerList = connection.Query<Customer>("select Customers.* from Customers inner join BookingDetails on BookingDetails.ID=Customers.BookingDetailId where BookingDetails.ID =" + idBookingDetails).ToList();


            }
            
            return Ok(CustomerList);
        }
    }
}