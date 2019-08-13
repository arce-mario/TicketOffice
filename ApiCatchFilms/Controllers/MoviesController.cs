using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;
using System;
using System.Diagnostics;

namespace ApiCatchFilms.Controllers
{
    [AllowAnonymous]
    public class MoviesController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        //opc == 1 // todos los registros
        public IQueryable<Movie> GetMovies(string movie = "",string name="", int opc = 0)
        {
            if (movie != ""){
                return db.Movies
                    .Where(m => m.name.Contains(movie))
                    .Take(2);
            }

            if (name != "")
            {
                return db.Movies.Where(m => db.Functions.Where(f => (m.movieID == f.movieID) && f.time >= DateTime.UtcNow).Count() >= 1 && m.name.Contains(name));
            }

            if (opc == 0)
            {
                return db.Movies.Where(m => (db.Functions.Where(f => (f.movieID == m.movieID) && f.time >= DateTime.UtcNow).Count() >= 1));
            }
            return null;
        }
        // GET: api/Movies/5
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> GetMovie(int id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // PUT: api/Movies/5
       
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.movieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/Movies
       
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = movie.movieID }, movie);
        }

        // DELETE: api/Movies/5
        [Authorize(Roles = LoginController.ADMIN_ROL)]
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> DeleteMovie(int id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            await db.SaveChangesAsync();

            return Ok(movie);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.movieID == id) > 0;
        }
    }
}