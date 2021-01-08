using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("Home/Rooms/api/[controller]")]
    [ApiController]
    public class BillDetailsController : ControllerBase
    {
        private readonly ProductContext _context;
        private string Connection = "Server=localhost,1433;Database=HotelDb;User=sa;Password=yourStrong(!)Password";
        private int FindQuantityProduct(int id)
        {
            int quantity;
            string sql = "select sum(EnterProducts.Quantity) from EnterProducts inner join Products on " +
                          "EnterProducts.ProductId = Products.ID where EnterProducts.ProductId = " + id + " and EnterProducts.IsDelete= 0 group by Products.ID ";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                quantity = connection.Query<int>(sql).FirstOrDefault();
            }
            return quantity;
        }
        private EnterProduct FindEnterProduct(int id)
        {
            EnterProduct quantity;
            string sql = "select EnterProducts.* from EnterProducts inner join Products on " +
                          "EnterProducts.ProductId = Products.ID where EnterProducts.Quantity>0 and EnterProducts.ProductId = " + id + " and EnterProducts.IsDelete= 0 ";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                quantity = connection.Query<EnterProduct>(sql).FirstOrDefault();
            }
            return quantity;
        }
        [HttpGet("Product")]
        public IActionResult GetProducts()
        {

            //string sql = "SELECT Products.* FROM Products inner join Types  on Products.TypeId=Types.ID Where Products.IsDelete = 0"
            //    .Select(p => new
            //    {


            //        p.
            //    }).ToList();
            string sql = "SELECT Products.*FROM Products inner join Types  on Products.TypeId = Types.ID Where Products.IsDelete = 0";


            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                products = connection.Query<Product>(sql).ToList();


            }
            //var product = await _context.Products.FindAsync(id);
            for (int i = 0; i < products.Count(); i++)
            {
                products[i].Quantity = FindQuantityProduct(products[i].ID);
                products[i].Type = _context.Types.Find(products[i].TypeId);
            }

            return Ok(products);
            //return await _context.Products.ToListAsync();
        }

        public BillDetailsController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/BillDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillDetail>>> GetBillDetails()
        {
            return await _context.BillDetails.ToListAsync();
        }

        // GET: api/BillDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetail>> GetBillDetail(int id)
        {
            List<BillDetail> billDetails;
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                billDetails = connection.Query<BillDetail>("select BillDetails.* from BillDetails inner join BookingDetails on BookingDetails.ID=BillDetails.BookingDetailId where BookingDetails.ID ="+id).ToList();


            }
            for(int i =0; i<billDetails.Count; i++)
            {
                billDetails[i].Product = _context.Products.Find(billDetails[i].ProductId);
            }
            return Ok(billDetails);



        
        }

        // PUT: api/BillDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillDetail(int id, BillDetail billDetail)
        {
            if (id != billDetail.ID)
            {
                return BadRequest();
            }

            _context.Entry(billDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BillDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BillDetail>> PostBillDetail(BillDetail billDetail)
        {
            List<BillDetail> billDetails;
            EnterProduct enterProduct = FindEnterProduct(billDetail.ProductId);
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                billDetails = connection.Query<BillDetail>("select BillDetails.* from BillDetails inner join BookingDetails on BookingDetails.ID=BillDetails.BookingDetailId where BookingDetails.ID =" + billDetail.BookingDetailId).ToList();


            }
            for (int i = 0; i < billDetails.Count; i++)
            {
                if (billDetails[i].ProductId == billDetail.ProductId)
                {
                    if (FindQuantityProduct(billDetail.ProductId) > 0)
                    {
                        
                        enterProduct.Quantity -= billDetail.Quantity;
                        _context.Entry(enterProduct).State = EntityState.Modified;
                        billDetails[i].Quantity += billDetail.Quantity;
                        billDetails[i].Total = billDetails[i].Price * billDetails[i].Quantity;
                        _context.BillDetails.Update(billDetails[i]);
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                  
                }
            }
         
            enterProduct.Quantity -= billDetail.Quantity;
            _context.Entry(enterProduct).State = EntityState.Modified;
            _context.BillDetails.Add(billDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBillDetail", new { id = billDetail.ID }, billDetail);
        }

        // DELETE: api/BillDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillDetail>> DeleteBillDetail(int id)
        {
            var billDetail = await _context.BillDetails.FindAsync(id);
            if (billDetail == null)
            {
                return NotFound();
            }

            _context.BillDetails.Remove(billDetail);
            await _context.SaveChangesAsync();

            return billDetail;
        }

        private bool BillDetailExists(int id)
        {
            return _context.BillDetails.Any(e => e.ID == id);
        }
    }
}
