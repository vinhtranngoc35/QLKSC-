using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("Rooms/api/[controller]")]
    [ApiController]
    public class RoomsApiController : ControllerBase
    {
        private readonly ProductContext _context;
        private string Connection = "Data Source=(localdb)\\ProjectsV13;Initial Catalog=Product;Integrated Security=True";
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
        // GET: api/RoomsApit
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/RoomsApit/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/RoomsApit
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/RoomsApit/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
