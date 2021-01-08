using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterProductsController : ControllerBase
    {
      

        private EnterProduct FindEnterProductNew(int id)
        {
            EnterProduct enterProduct = new EnterProduct();
            string sql = "SELECT * FROM EnterProducts Where EnterProducts.IsDelete = 0 AND EnterProducts.UId ="+id;

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                enterProduct = connection.Query<EnterProduct>(sql).FirstOrDefault();
            }
            return enterProduct;
        }
        private List<EnterProduct> FindEnterProduct()
        {
            List<EnterProduct> enterProduct = new List<EnterProduct>();
            string sql = "SELECT * FROM EnterProducts Where EnterProducts.IsDelete = 0 ";

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                enterProduct = connection.Query<EnterProduct>(sql).ToList();
            }
            return enterProduct;
        }
        private readonly ProductContext _context;
        public static bool JSONEquals( object obj, object another)
        {
            
            if (ReferenceEquals(obj, another)) return true;
            if ((obj == null) || (another == null)) return false;
            if (obj.GetType() != another.GetType()) return false;

            var objJson = JsonConvert.SerializeObject(obj);
            var anotherJson = JsonConvert.SerializeObject(another);

            return objJson == anotherJson;
        }
        private string Connection = "Server=localhost,1433;Database=HotelDb;User=sa;Password=yourStrong(!)Password";
        public EnterProductsController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/EnterProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnterProduct>>> GetEnterProducts()
        {
            List<EnterProduct> enterProducts = FindEnterProduct();
            for(int i = 0; i< enterProducts.Count; i++)
            {
                enterProducts[i].Product = _context.Products.Find(enterProducts[i].ProductId);
            }
            return enterProducts;
        }

        // GET: api/EnterProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnterProduct>> GetEnterProduct(int id)
        {
            var enterProduct = await _context.EnterProducts.FindAsync(id);
           
            if (enterProduct == null)
            {
                return NotFound();
            }
            else
            {
                enterProduct.Product = await _context.Products.FindAsync(enterProduct.ProductId);
            }

            return enterProduct;
        }

        // PUT: api/EnterProducts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnterProduct(int id, EnterProduct enterProduct)
        {
            EnterProduct enterProductOld = _context.EnterProducts.Find(id);

            if (enterProductOld.IsDelete)
            {
                EnterProduct enterProductNewUpdate = FindEnterProductNew(enterProduct.UId);
                int idEnterProduct = enterProduct.ID;
                enterProduct.ID = 0;
                enterProductNewUpdate.ID = 0;
                
                enterProduct.CreateAt = enterProductOld.CreateAt;
                int idEnterProductNewUpdate = enterProductNewUpdate.ID;
                if (JSONEquals(enterProduct, enterProductNewUpdate))
                {
                    return Ok(null);
                }
                else
                {
                    enterProductNewUpdate.ID = idEnterProductNewUpdate;
                    return Ok(enterProductNewUpdate);
                }
            }
            else
            {
                if(enterProduct.UId == 0)
                {
                    enterProduct.UId = enterProduct.ID;
                }
                EnterProduct enterProductSave = enterProduct;
                enterProductSave.ID = 0;
                enterProductOld.IsDelete = true;
                enterProduct.Quantity = enterProduct.Quantity + (enterProduct.RemainingAmount - enterProductOld.RemainingAmount);
                enterProductSave.CreateAt = enterProductOld.CreateAt;
                _context.Entry(enterProductOld).State = EntityState.Modified;
                _context.EnterProducts.Add(enterProductSave);
                await _context.SaveChangesAsync();
            }
            

            return Ok(null);
        }

        // POST: api/EnterProducts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EnterProduct>> PostEnterProduct(EnterProduct[] enterProducts)
        {
            for(int i =0; i<enterProducts.Count(); i++)
            {
                EnterProduct enterProduct = enterProducts[i];
                enterProduct.CreateAt = DateTime.Now;
                enterProduct.Quantity = enterProduct.RemainingAmount;
                enterProduct.Product = _context.Products.Find(enterProduct.ProductId);
                _context.EnterProducts.Add(enterProduct);
            }
          
            await _context.SaveChangesAsync();
            return Ok(null);
        }

        // DELETE: api/EnterProducts/5
        [HttpPut("Delete/{id}")]
        public async Task<ActionResult<EnterProduct>> DeleteEnterProduct(int id)
        {

            var enterProduct = await _context.EnterProducts.FindAsync(id);
            enterProduct.IsDelete = true;
            if (enterProduct == null)
            {
                return NotFound();
            }

            _context.Entry(enterProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return enterProduct;
        }

        private bool EnterProductExists(int id)
        {
            return _context.EnterProducts.Any(e => e.ID == id);
        }
    }
}
