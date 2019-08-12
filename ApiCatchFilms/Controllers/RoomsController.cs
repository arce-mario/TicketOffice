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

        // POST: api/Rooms
        [Authorize(Roles = LoginController.ADMIN_ROL)]
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> PostRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = room.roomID }, room);
        }

        // DELETE: api/Rooms/5
        [Authorize(Roles = LoginController.ADMIN_ROL)]
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> DeleteRoom(int id)
        {
            Room room = await db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
            await db.SaveChangesAsync();

            return Ok(room);
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