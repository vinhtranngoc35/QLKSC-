using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("Rooms/api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsApiController : ControllerBase
    {
        public static IWebHostEnvironment _enviroment;
        public class FileUploadApi
        {
            public IFormFile files { get; set; }
        }
        [HttpPost]
        [Route("Images")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var fileUpload = HttpContext.Request.Form.Files["files"];
                Product product = new Product();
                product.Name = HttpContext.Request.Form["name"];
               
                product.PriceProduct = int.Parse(HttpContext.Request.Form["priceProduct"]);
                product.Unit = HttpContext.Request.Form["unit"];
                product.TypeId = int.Parse(HttpContext.Request.Form["typeId"]);
                product.Company = HttpContext.Request.Form["company"];
                product.UrlImage = Guid.NewGuid().ToString()+"_"+fileUpload.FileName;
              
         
               

                FileUploadApi fileUploadApi = new FileUploadApi();
                fileUploadApi.files = fileUpload;
                if (fileUploadApi.files.Length > 0)
                {
                   
                    
                        var uniqueFileName = product.UrlImage;
                        var uploads = Path.Combine(_enviroment.WebRootPath, "Upload");
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        fileUploadApi.files.CopyTo(new FileStream(filePath, FileMode.Create));

                    //to do : Save uniqueFileName  to your db table   


                    //if (!Directory.Exists(_enviroment.WebRootPath + $"\\Upload\\"))
                    //{
                    //    Directory.CreateDirectory(_enviroment.WebRootPath + $"\\Upload\\");
                    //}
                    //using (FileStream fileStream = System.IO.File.Create(_enviroment.WebRootPath + $"\\Upload\\" + product.UrlImage))
                    //{
                    //    fileUploadApi.files.CopyTo(fileStream);
                    //    fileStream.Flush();
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("Created",product);

                    //}
                   

                }
                else
                {
                    return Ok(null);
                }
            }catch(Exception ex)
            {
                return Ok(ex);
            }

           
        }
        private int FindQuantityProduct(int id)
        {
            int quantity;
            string sql = "select sum(EnterProducts.Quantity) from EnterProducts inner join Products on " +
                          "EnterProducts.ProductId = Products.ID where EnterProducts.ProductId = " + id+" and EnterProducts.IsDelete= 0 group by Products.ID ";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                quantity = connection.Query<int>(sql).FirstOrDefault();
            }
            return quantity;
        }
        private List<EnterProduct> FindEnterProduct(int id)
        {
            List<EnterProduct> quantity =new List<EnterProduct>();
            string sql = "select EnterProducts.* from EnterProducts inner join Products on " +
                          "EnterProducts.ProductId = Products.ID where EnterProducts.ProductId = " + id +
                          " and EnterProducts.IsDelete= 0";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                quantity = connection.Query<EnterProduct>(sql).ToList();
            }
            return quantity;
        }
        [HttpGet]
        [Route("page/{page}/{search}")]
        public IActionResult GetProductPage(int page,string search)
        {
            List<Product> products = new List<Product>();
            if (search == "a")
            {


                string sql = "SELECT * FROM Products ORDER BY Products.ID OFFSET " + 2 + "*(" + page + "-1) ROWS FETCH NEXT 2  ROWS ONLY OPTION (RECOMPILE)";

                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    products = connection.Query<Product>(sql).ToList();
                }
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].Type = _context.Types.Find(products[i].TypeId);
                } } else {
                string sql1 = "select * from Products where Products.Name like '%" + search+"%'";

                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    products = connection.Query<Product>(sql1).ToList();
                }
                for (int i = 0; i < products.Count; i++)
                {
                    products[i].Type = _context.Types.Find(products[i].TypeId);
                }
            }
            return Ok(products);
        }
       
        [HttpGet]
        [Route("page")]
        public IActionResult GetPaginationProduct()
        {
            List<Product> products = _context.Products.ToList();
            
            
            return Ok(products.Count);
        }
        [HttpGet]
        [Route("{IdRoom}/{checkIn}/{checkOut}")]
        public IActionResult CheckTypeRoom(int IdRoom, DateTime checkIn, DateTime checkOut)
        {
            var Room = _context.Rooms.Find(IdRoom);
            int idTypeRoom = Room.ID;
            string sql = "SELECT Rooms.*"
                + " FROM Rooms LEFT JOIN TypeRooms on Rooms.TypeRoomId = TypeRooms.ID"
                + " WHERE Rooms.ID not in"
                + " (SELECT t.ID from"
                + " (select Rooms.[Name], Rooms.ID, Rooms.TypeRoomId"
                + " from Rooms left join BookingDetails on Rooms.ID= BookingDetails.RoomId "
                + " where BookingDetails.CheckInExpected<'" + checkOut.ToString("MM/dd/yyyy HH:mm:ss") + "' and"
                + " BookingDetails.CheckOutExpected> '" + checkIn.ToString("MM / dd / yyyy HH: mm:ss") + "' and"
                + " BookingDetails.CheckOut is null"
                + " group by Rooms.[Name],Rooms.ID,Rooms.TypeRoomId) as t) and  Rooms.TypeRoomId=" + idTypeRoom;
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                rooms = connection.Query<Room>(sql).ToList();
            }
            rooms.Add(Room);
            return Ok(rooms);
        }
        private readonly ProductContext _context;
        private string Connection = "Server=localhost,1433;Database=HotelDb;User=sa;Password=yourStrong(!)Password";

        public ProductsApiController(ProductContext context, IWebHostEnvironment environment)
        {
            _enviroment = environment;
            _context = context;
        }
        [HttpGet("booking/{checkIn}/{checkOut}")]
        public IActionResult GetRoomChecked(string checkIn, string checkOut)
        {
            if(DateTime.Parse(checkIn).Date < DateTime.Now.Date)
            {
                return Ok(null);
            }else if(DateTime.Parse(checkOut).Date< DateTime.Parse(checkIn).Date)
            {
                return Ok(null);
            }
            string sql = "SELECT COUNT(*) AS 'NumberRoom', TypeRooms.Price as 'PriceRoom',TypeRooms.[Name] as 'Name',TypeRooms.ID as 'TypeRoomId' "
                + " FROM Rooms LEFT JOIN TypeRooms on Rooms.TypeRoomId = TypeRooms.ID"
                + " WHERE Rooms.ID not in"
                + " (SELECT t.ID from"
                + " (select Rooms.[Name], Rooms.ID, Rooms.TypeRoomId"
                + " from Rooms left join BookingDetails on Rooms.ID= BookingDetails.RoomId "
                + " where BookingDetails.CheckInExpected<'" + checkOut + "' and"
                + " BookingDetails.CheckOutExpected> '" + checkIn + "' and"
                + " BookingDetails.CheckOut is null"
                + " group by Rooms.[Name],Rooms.ID,Rooms.TypeRoomId) as t)"
                + " group by TypeRooms.Price,TypeRooms.[Name],TypeRooms.ID";
            List<RoomChecked> roomCheckeds = new List<RoomChecked>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                roomCheckeds = connection.Query<RoomChecked>(sql).ToList();
            }
            
            
            return Ok(roomCheckeds);
            ////var product = await _context.Products.FindAsync(id);

            //var RoomChecked = _context.Rooms
            //    .FromSqlRaw("select COUNT(*) AS 'NumberRoom', TypeRooms.Price as 'PriceRoom',TypeRooms.[Name] as 'Name',TypeRooms.ID as 'TypeRoomId' "
            //    + "from Rooms left join TypeRooms on Rooms.TypeRoomId = TypeRooms.ID"
            //    + "where Rooms.ID not in"
            //    + "(select t.ID from"
            //    + "(select Rooms.[Name], Rooms.ID, Rooms.TypeRoomId"
            //    + "from Rooms left join BookingDetails on Rooms.ID= BookingDetails.RoomId"
            //    + "where BookingDetails.CheckInExpected<" + checkOut + " and"
            //    + "BookingDetails.CheckOutExpected> " + checkIn + " and"
            //    + "BookingDetails.CheckOut is null"

            //    + "group by Rooms.[Name],Rooms.ID,Rooms.TypeRoomId) as t)"
            //    + "group by TypeRooms.Price,TypeRooms.[Name],TypeRooms.ID")
            //    .Select(RoomChecked => new
            //    {   RoomChecked.NumberRoom
            //        r.TypeRoomId,
            //        r.NumberRoom
            //    }
            //    ).ToList();
            //return Ok(RoomChecked);
        }
        // GET: api/ProductsApi
        [HttpGet]
        public IActionResult GetProducts()
        {

            //string sql = "SELECT Products.* FROM Products inner join Types  on Products.TypeId=Types.ID Where Products.IsDelete = 0"
            //    .Select(p => new
            //    {


            //        p.
            //    }).ToList();
            string sql = "SELECT Products.*FROM Products inner join Types  on Products.TypeId = Types.ID Where Products.IsDelete = 0 ORDER BY Products.ID DESC";


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

        // GET: api/ProductsApi/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    //var product = await _context.Products.FindAsync(id);
        //    var product = _context.Products.FromSqlRaw("SELECT * FROM Products Where Id = "+id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = _context.Products
                .FromSqlRaw("SELECT * FROM Products Where Id = "+id)
                .Select(p => new
                {
                    p.ID,
                    p.Name,
                    p.Price,
                    p.Type,
                    p.IsDelete,
                    p.PriceProduct,
                    p.Company,
                    p.UrlImage,
                }).ToList();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        // PUT: api/ProductsApi/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id)
        {
            var productCheck = _context.Products.Find(id);
            var fileUpload = HttpContext.Request.Form.Files["files"];
            Product product = new Product();
            product.Name = HttpContext.Request.Form["name"];
            product.ID = int.Parse(HttpContext.Request.Form["id"]);
            product.Unit = HttpContext.Request.Form["unit"];
            product.PriceProduct = int.Parse(HttpContext.Request.Form["priceProduct"]);
            product.TypeId = int.Parse(HttpContext.Request.Form["typeId"]);
            product.Company = HttpContext.Request.Form["company"];
            product.UrlImage = HttpContext.Request.Form["urlImage"];
            product.Price = productCheck.Price;
            if ("https://localhost:5001/Upload/" + productCheck.UrlImage != product.UrlImage)
            {
                FileUploadApi fileUploadApi = new FileUploadApi();
                fileUploadApi.files = fileUpload;
                product.UrlImage = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;
                if (fileUploadApi.files.Length > 0)
                {
                    var uniqueFileName = product.UrlImage;
                    var uploads = Path.Combine(_enviroment.WebRootPath, "Upload");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    fileUploadApi.files.CopyTo(new FileStream(filePath, FileMode.Create));



                }
            }
            else
            {
                product.UrlImage = productCheck.UrlImage;
            }
            
          
          


            Product oldProduct = _context.Products.Find(id);
            if (oldProduct.Price == 0)
            {
                oldProduct.Price = oldProduct.ID;
            }
            if (oldProduct.IsDelete)
            {
                string sql = "SELECT * FROM Products Where Products.IsDelete = 0 And Products.Price = " + oldProduct.Price;

                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    oldProduct = connection.Query<Product>(sql).LastOrDefault();
                }

                if (oldProduct.UrlImage != product.UrlImage || oldProduct.Name != product.Name || oldProduct.PriceProduct != product.PriceProduct || oldProduct.TypeId != product.TypeId || oldProduct.Company != product.Company)
                {
                    string sql1 = "SELECT * FROM Products Where Products.IsDelete = 0 And Products.Price = " + oldProduct.Price;

                    using (SqlConnection connection = new SqlConnection(Connection))
                    {
                        oldProduct = connection.Query<Product>(sql1).LastOrDefault();
                    }
                    return Ok(oldProduct);

                }
                else
                {
                    //    Product newProduct = new Product();

                    //    newProduct.Name = product.Name;
                    //    newProduct.Type = _context.Types.Find(product.TypeId);
                    //    newProduct.PriceProduct = product.PriceProduct;
                    //    newProduct.Company = product.Company;
                    //    newProduct.Price = oldProduct.Price;
                    //    newProduct.UrlImage = oldProduct.UrlImage;
                    //    oldProduct.IsDelete = true;
                    //    _context.Entry(oldProduct).State = EntityState.Modified;
                    //    _context.Products.Add(newProduct);

                    //    await _context.SaveChangesAsync();
                    return Ok(null);
                  
                }
                
            }

            if (product.UrlImage!= oldProduct.UrlImage ||product.Name != oldProduct.Name || product.PriceProduct != oldProduct.PriceProduct || product.Company != oldProduct.Company || product.TypeId != oldProduct.TypeId)
            {

                
                    oldProduct.IsDelete = true;
                    _context.Entry(oldProduct).State = EntityState.Modified;
                    Product productSave = new Product();
                    productSave.Name = product.Name;
                    productSave.Type = _context.Types.Find(product.TypeId);
                    productSave.PriceProduct = product.PriceProduct;
                    productSave.Company = product.Company;
                    productSave.UrlImage = product.UrlImage;
                    

                    
                    if (oldProduct.Price == 0)
                    {
                        productSave.Price = product.ID;
                    }
                    else
                    {
                        productSave.Price = oldProduct.Price;
                    }

                    _context.Products.Add(productSave);

                    if (id != product.ID)
                    {
                        return BadRequest();
                    }



                    try
                    {
                        await _context.SaveChangesAsync();
                    
                    List<EnterProduct> enterProducts = FindEnterProduct(product.ID);
                    for(int i =0; i < enterProducts.Count(); i++)
                    {
                        enterProducts[i].Product = productSave;
                        enterProducts[i].ProductId = productSave.ID;
                        _context.Entry(enterProducts[i]).State = EntityState.Modified;
                        

                    }
                    await _context.SaveChangesAsync();



                }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    


                }
            return Ok(null);

        }
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        [Route("DeleteSoft/{id}")]
        public async Task<IActionResult> DeleteSoftProduct(int id)
        {
            var product  = await _context.Products.FindAsync(id);
            product.IsDelete = true;
            if (id != product.ID)
            {
                return BadRequest();
            }
           
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/ProductsApi
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/ProductsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            string sql = "UPDATE Products SET IsDelete = true WHERE Id = "+id;
            _context.Products.FromSqlRaw(sql);
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            //_context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
