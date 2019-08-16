using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiCatchFilms.Controllers
{
    public class RoomsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        // GET: api/Rooms
        public IQueryable<Room> GetRooms(string name = "", int opc = 0)
        {
            if (name != "")
            {
                Regex re = new Regex(@"\d+");
                Match m = re.Match(name);

                if (m.Success)
                {
                    int value = int.Parse(m.Value);
                    return db.Rooms.Where(rm => rm.number == value);
                }
                
            }

            if (opc == 0)
            {
                return db.Rooms;
            }

            return null;
        }

        [Route("{id:int}/functions")]
        [ResponseType(typeof(IQueryable<Function>))]
        public IQueryable<Function> GetFunctions(int id)
        {
            return db.Functions.Where(f => (f.functionID == id && f.time >= DateTime.UtcNow));
        }
        // GET: api/Rooms/5
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> GetRoom(int id)
        {
            Room room = await db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        // PUT: api/Rooms/5
        [Authorize(Roles = LoginController.ADMIN_ROL)]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.roomID)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> PostRoom(Room room, string listSeats = "")
        {
            List<Seat> seats = getSeats(listSeats);

            if (!ModelState.IsValid || seats == null)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            await db.SaveChangesAsync();
            List<Seat> seatsDB = await db.Seats.OrderBy(s => s.seatID).ToListAsync();
            List<RoomSeat> roomSeats = new List<RoomSeat>();
            
            int max = seatsDB.Count;
            int min = seats.Count;

            if (min > max)
            {
                seats.RemoveRange(0, max);
                db.Seats.AddRange(seats);
                await db.SaveChangesAsync();

                seatsDB = await db.Seats.OrderBy(s => s.seatID).ToListAsync();

                foreach (Seat seat in seatsDB)
                {
                    roomSeats.Add(new RoomSeat() { roomID = room.roomID, seatID = seat.seatID, status = 1});
                }
            }
            else
            {
                seats = seatsDB.GetRange(0, min);
                foreach (Seat seat in seats)
                {
                    roomSeats.Add(new RoomSeat() { roomID = room.roomID, seatID = seat.seatID, status = 1});
                }
            }

            db.RoomSeats.AddRange(roomSeats);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = room.roomID }, room);
        }
        
        private List<Seat> getSeats(string parameterSeats)
        {
            int primaryASCII = 65;
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(parameterSeats);
            List<Seat> listSeats = new List<Seat>();

            for (int i = 0; i < seats.LongCount(); i++)
            {
                string row = Convert.ToChar(primaryASCII++).ToString();

                for (int c = 0; c < seats.ElementAt(i).Length; c++)
                {
                    listSeats.Add(new Seat() { row = row, column = (c + 1) });
                }
            }
            Debug.WriteLine("Butacas: " + JsonConvert.SerializeObject(listSeats));
            return listSeats;
        }

        // DELETE: api/Rooms/5
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> DeleteRoom(int id)
        {
            Room room = await db.Rooms.FindAsync(id);
            
            if (room == null)
            {
                return NotFound();
            }
            else if(db.Functions.Where(fn => fn.roomID == room.roomID).Count() == 0)
            {
                db.RoomSeats.RemoveRange(db.RoomSeats.Where(rs => rs.roomID == room.roomID));
                db.Rooms.Remove(room);
                await db.SaveChangesAsync();
                return Ok(room);
            }
            return StatusCode(HttpStatusCode.NotAcceptable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.roomID == id) > 0;
        }
    }
}