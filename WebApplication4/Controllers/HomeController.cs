using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private string Connection = "Server=localhost,1433;Database=HotelDb;User=sa;Password=yourStrong(!)Password";
        private Booking Bookings(String Identity)
        {
            string sql = "select Bookings.* from Bookings inner join Customers on Bookings.CustomerId=Customers.ID where Customers.IdentityCard= " + Identity + " and Bookings.Status=0;";
            Booking bookings = new Booking();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                bookings = connection.Query<Booking>(sql).FirstOrDefault();
            }
            return bookings;
        }
        [HttpPost]
        [Route("Account/Home/Register")]
        public async Task<IActionResult> CreateLoginAsync(LoginForm loginForm)
        {
           
                var user = new IdentityUser();
                user.Email = loginForm.Email;
                user.UserName = loginForm.Email;

                var result = await userManager.CreateAsync(user, loginForm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user: user, isPersistent: false);
                    
                }
            return RedirectToAction("index", "home");
        }
        [HttpPost]
        [Route("Home/Logout")]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
           
            return RedirectToAction("index", "home");
        }


        
        [HttpGet("/Account/Login")]
        [AllowAnonymous]
        public IActionResult ShowLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Views/Home/Login.cshtml");
        }
        private Discount FindDiscount(DateTime date, int id)
        {
            string sql = "SELECT * FROM Discounts where '" + date.ToString("MM/dd/yyyy HH:mm:ss") + "' between TimeStart and TimeStop and TypeRoomId=" + id;
            Discount discount = new Discount();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                discount = connection.Query<Discount>(sql).FirstOrDefault();
            }
            return discount;
        }
        private BookingDetail FindBookingDetailToday(DateTime checkIn, DateTime checkOut, int IdRoom)
        {
            string sql = "select * from Bookingdetails where CheckInExpected< '" + checkOut.ToString("MM/dd/yyyy HH:mm:ss") + "' and CheckOutExpected> '" + checkIn.ToString("MM/dd/yyyy HH:mm:ss") + "' and CheckOut is null and RoomId=" + IdRoom;
            BookingDetail bookingDetail = new BookingDetail();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                bookingDetail = connection.Query<BookingDetail>(sql).FirstOrDefault();
            }
            return bookingDetail;
        }
        private List<Booking> GetBookingToDay(DateTime DateStart, DateTime DateEnd)
        {
            string sql = "select * from bookings where checkIn<='" + DateEnd.ToString("MM/dd/yyyy HH:mm:ss") + "' and checkOut>='" + DateStart.ToString("MM/dd/yyyy HH:mm:ss") + "' and status=0";
            List<Booking> Bookings = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                Bookings = connection.Query<Booking>(sql).ToList();
            }
            return Bookings;
        }
        private List<BillDetail> GetBillDetailsByBookingDetails(int id)
        {
            List<BillDetail> billDetails;
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                billDetails = connection.Query<BillDetail>("select BillDetails.* from BillDetails inner join BookingDetails on BookingDetails.ID=BillDetails.BookingDetailId where BookingDetails.ID =" + id).ToList();


            }
            return billDetails;
        }
        private List<BookingDetail> FindBookingDetailsByBooking(int id)
        {
            string sql = "select * from BookingDetails where BookingId= " + id;
            List<BookingDetail> List = new List<BookingDetail>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                List = connection.Query<BookingDetail>(sql).ToList();
            }
            return List;
        }
        private List<Room> CheckTypeRoom(int idTypeRoom, DateTime checkIn, DateTime checkOut)
        {
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
            return rooms;
        }
        private List<Room> CheckRoom(DateTime checkIn, DateTime checkOut)
        {
            string sql = "SELECT Rooms.*"
                + " FROM Rooms LEFT JOIN TypeRooms on Rooms.TypeRoomId = TypeRooms.ID"
                + " WHERE Rooms.ID in"
                + " (SELECT t.ID from"
                + " (select Rooms.[Name], Rooms.ID, Rooms.TypeRoomId"
                + " from Rooms left join BookingDetails on Rooms.ID= BookingDetails.RoomId "
                + " where BookingDetails.CheckInExpected<'" + checkOut.ToString("MM/dd/yyyy HH:mm:ss") + "' and"
                + " BookingDetails.CheckOutExpected> '" + checkIn.ToString("MM / dd / yyyy HH: mm:ss") + "' and"
                + " BookingDetails.CheckOut is null"
                + " group by Rooms.[Name],Rooms.ID,Rooms.TypeRoomId) as t) ";
            List<Room> rooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                rooms = connection.Query<Room>(sql).ToList();
            }
            return rooms;
        }



        public HomeController(ILogger<HomeController> logger, ProductContext context
            , UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("/EnterProducts")]
        public IActionResult EnterProduct()
        {
            string name = "Version: " + System.Environment.Version.ToString();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        [Route("home/create")]
        public async Task<IActionResult> CreateBooking(CreateBooking createBooking)
        {
            Booking booking = new Booking();
            if (createBooking.Booking.CheckIn == createBooking.Booking.CheckOut)
            {

                TimeSpan time = new TimeSpan(20, 30, 0);
                createBooking.Booking.CheckOut = createBooking.Booking.CheckOut.AddDays(0.8);

                booking.CheckOut = createBooking.Booking.CheckOut;
                booking.CheckIn = createBooking.Booking.CheckIn;
                booking.Customer = createBooking.Customer;
                booking.Total = 0;
                booking.CreateTime = DateTime.Now;
            }
            else
            {
                booking.CheckOut = createBooking.Booking.CheckOut;
                booking.CheckIn = createBooking.Booking.CheckIn;
                booking.Customer = createBooking.Customer;
                booking.Total = 0;
                booking.CreateTime = DateTime.Now;
            }

            List<BookingDetail> bookingDetails = new List<BookingDetail>();
            for (int i = 0; i < createBooking.NumberRoom.Length; i++)
            {
                for (int j = 0; j < createBooking.NumberRoom[i]; j++)
                {
                    List<Room> rooms = CheckTypeRoom(createBooking.TypeRoomID[i], createBooking.Booking.CheckIn, createBooking.Booking.CheckOut);
                    BookingDetail bookingDetail = new BookingDetail();
                    bookingDetail.RoomId = rooms[j].ID;
                    bookingDetail.CheckInExpected = createBooking.Booking.CheckIn;
                    bookingDetail.CheckOutExpected = createBooking.Booking.CheckOut;
                    bookingDetail.Booking = booking;
                    bookingDetail.NumberDate = 0;
                    bookingDetail.Price = createBooking.PriceRoom[i];
                    bookingDetail.Total = 0;
                    bookingDetail.NumberDateDiscount = 0;
                    bookingDetail.PriceDiscount = 0;
                    bookingDetails.Add(bookingDetail);
                }

            }
            booking.BookingDetails = bookingDetails;
            _context.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        
        [HttpGet]
        [Route("/Rooms")]
        public IActionResult RoomsAvaible()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            List<Room> roomIsBooking = CheckRoom(startDate, endDate);
            List<Room> rooms = _context.Rooms.ToList();
            List<ViewRoom> viewRooms = new List<ViewRoom>();

            for (int i = 0; i < rooms.Count; i++)
            {
                ViewRoom viewRoom = new ViewRoom();
                viewRoom.Status = false;
                for (int j = 0; j < roomIsBooking.Count; j++)
                {
                    if (rooms[i].ID == roomIsBooking[j].ID)
                    {

                        BookingDetail bookingDetail = FindBookingDetailToday(startDate, endDate, rooms[i].ID);
                        if (bookingDetail != null)
                        {
                            List<string> CustomerList;
                            using (SqlConnection connection = new SqlConnection(Connection))
                            {

                                CustomerList = connection.Query<string>("select Customers.Name from Customers inner join BookingDetails on BookingDetails.ID=Customers.BookingDetailId where BookingDetails.ID =" + bookingDetail.ID).ToList();


                            }
                            viewRoom.NameCustomers = CustomerList;
                            viewRoom.CheckIn = bookingDetail.CheckInExpected;
                            viewRoom.CheckOut = bookingDetail.CheckOutExpected;
                            viewRoom.Status = true;

                            break;
                        }


                    }

                }
                viewRoom.ID = rooms[i].ID;
                viewRoom.NameRoom = rooms[i].Name;
                viewRooms.Add(viewRoom);
            }
            return View(viewRooms);
        }

        [HttpGet]
        [Route("Home/Rooms/{idRoom}")]
        public IActionResult ViewRoom(int idRoom)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            BookingDetail bookingDetail = FindBookingDetailToday(startDate, endDate, idRoom);
            var Room = _context.Rooms.Find(idRoom);
            List<Room> rooms = CheckTypeRoom(_context.Rooms.Find(idRoom).TypeRoomId, startDate, endDate);
            rooms.Add(Room);
            ViewData["RoomId"] = new SelectList(rooms, "ID", "Name", bookingDetail.RoomId);
            return View(bookingDetail);
        }
        [HttpPost]
        public async Task<IActionResult> EditBookingDetail(int id, [Bind("ID,CheckInExpected,CheckOutExpected,Price,RoomId")] BookingDetail bookingDetail)
        {
            var bookingDetailOld = _context.BookingDetails.Find(bookingDetail.ID);
            bookingDetailOld.RoomId = bookingDetail.RoomId;
            _context.Update(bookingDetailOld);
            await _context.SaveChangesAsync();
            return Redirect("Rooms/" + bookingDetail.RoomId);
        }
        [HttpGet]
        [Route("BookingToDay")]
        public IActionResult ViewBookingToDay()
        {
            DateTime DateNow = DateTime.Now;
            DateTime DateEnd = DateNow.AddDays(1);
            List<Booking> bookings = GetBookingToDay(DateNow, DateEnd);
            for (int i = 0; i < bookings.Count; i++)
            {
                bookings[i].Customer = _context.Customers.Find(bookings[i].CustomerId);
            }
            return View(bookings);
        }
        [HttpGet]
        [Route("Home/BookingToDay/{id}")]
        public IActionResult CHeckOut(int id)
        {
            Booking booking = _context.Bookings.Find(id);
            booking.PriceRoom = 0;
            booking.ExtraPrice = 0;
            List<BookingDetail> bookingDetails = FindBookingDetailsByBooking(id);
            for (int i = 0; i < bookingDetails.Count; i++)
            {
                bookingDetails[i].PriceExtra = 0;
                List<BillDetail> billDetails = GetBillDetailsByBookingDetails(bookingDetails[i].ID);
                Room room = _context.Rooms.Find(bookingDetails[i].RoomId);
                List<Discount> discounts = new List<Discount>();
                for (int j = 0; j < billDetails.Count; j++)
                {
                    bookingDetails[i].PriceExtra += billDetails[j].Total;
                }
                for (DateTime date = bookingDetails[i].CheckInExpected; date <= bookingDetails[i].CheckOutExpected; date = date.AddDays(1))
                {
                    Discount discount = FindDiscount(date, room.TypeRoomId);
                    if (discount != null)
                    {
                        discounts.Add(discount);
                    }

                }
                bookingDetails[i].NumberDateDiscount = 0;
                bookingDetails[i].PriceDiscount = 0;
                if (discounts.Count > 0)
                {
                    bookingDetails[i].NumberDateDiscount = discounts.Count;
                    bookingDetails[i].PriceDiscount = bookingDetails[i].NumberDateDiscount * bookingDetails[i].Price * discounts[0].NumberDiscount / 100;
                    bookingDetails[i].Discount = discounts[0].NumberDiscount;
                }
                bookingDetails[i].Room = _context.Rooms.Find(bookingDetails[i].RoomId);
                bookingDetails[i].Room.TypeRoom = _context.TypeRooms.Find(bookingDetails[i].Room.TypeRoomId);
                bookingDetails[i].NumberDate = (bookingDetails[i].CheckOutExpected - bookingDetails[i].CheckInExpected).Days + 1;
                bookingDetails[i].PriceRoom = bookingDetails[i].Price * bookingDetails[i].NumberDate;
                bookingDetails[i].Total = bookingDetails[i].PriceRoom - bookingDetails[i].PriceDiscount;
                booking.PriceRoom += bookingDetails[i].Total;
                booking.ExtraPrice += bookingDetails[i].PriceExtra;

            }
            booking.BookingDetails = bookingDetails;
            booking.NumberRoom = bookingDetails.Count;
            booking.Total = booking.PriceRoom + booking.ExtraPrice;
            booking.Customer = _context.Customers.Find(booking.CustomerId);
            _context.Bookings.Update(booking);
            _context.SaveChangesAsync();
            return View(booking);
        }
        [HttpGet]
        [Route("Home/Pay/{id}")]
        public IActionResult Pay(int id)
        {
            Booking booking = _context.Bookings.Find(id);
            booking.Status = true;
            List<BookingDetail> bookingDetails = FindBookingDetailsByBooking(id);

            for (int i = 0; i < bookingDetails.Count; i++)
            {
                bookingDetails[i].CheckIn = bookingDetails[i].CheckInExpected;
                bookingDetails[i].CheckOut = bookingDetails[i].CheckOutExpected;
            }
            booking.BookingDetails = bookingDetails;
            _context.Bookings.Update(booking);
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetListBill)); ;
        }
        [HttpGet]
        [Route("ListBill")]
        public IActionResult GetListBill()
        {
            string sql = "select * from Bookings where status=1";
            List<Booking> bookings = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                bookings = connection.Query<Booking>(sql).ToList();
            }
            for (int i = 0; i < bookings.Count; i++)
            {
                Booking booking = bookings[i];
                booking.Customer = _context.Customers.Find(booking.CustomerId);
            }
            return View(bookings);
        }
        [HttpPost]
        [Route("CheckIn")]
        public IActionResult CheckIn(string identity)
        {
            Booking booking = Bookings(identity);
            booking.BookingDetails = FindBookingDetailsByBooking(booking.ID);
            booking.Customer = _context.Customers.Find(booking.CustomerId);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Home/Login")]
        public async Task<IActionResult> Login(LoginForm loginForm)
        {
            var result = await signInManager.PasswordSignInAsync(userName: loginForm.Email, password: loginForm.Password
                ,isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(loginForm.urlReturn))
                {
                    return Redirect(loginForm.urlReturn);
                }
                return Redirect("/");
            }
            else
            {
                
                    ModelState.AddModelError("", "Invalid");
                    return Redirect(loginForm.urlReturn);
             
            }
            
        }
            
        

    }
}
