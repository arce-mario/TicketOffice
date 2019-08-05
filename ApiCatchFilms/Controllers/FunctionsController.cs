using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;

namespace ApiCatchFilms.Controllers
{
    public class FunctionsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        // GET: api/Functions
        public IQueryable<Function> GetFunctions()
        {
            return db.Functions
                .Include(f => f.movie)
                .Include(f => f.price)
                .Include(f => f.room);
        }

        // GET: api/Functions/5
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> GetFunction(int id)
        {
            Function function = await db.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }

            return Ok(function);
        }

        // PUT: api/Functions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFunction(int id, Function function)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != function.functionID)
            {
                return BadRequest();
            }

            db.Entry(function).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunctionExists(id))
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

        // POST: api/Functions
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> PostFunction(Function function)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Functions.Add(function);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = function.functionID }, function);
        }

        // DELETE: api/Functions/5
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> DeleteFunction(int id)
        {
            Function function = await db.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }

            db.Functions.Remove(function);
            await db.SaveChangesAsync();

            return Ok(function);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FunctionExists(int id)
        {
            return db.Functions.Count(e => e.functionID == id) > 0;
        }
    }
}