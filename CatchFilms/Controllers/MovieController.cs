using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class MovieController : Controller
    {

        public async Task<ActionResult> Index()
        {
            List<Movie> Movies = new List<Movie>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/movies?opc=1");

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    Movies = JsonConvert.DeserializeObject<List<Movie>>(response);
                    ViewBag.InfoMessage = TempData["InfoMessage"];
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                }
                return View(Movies);
            }
        }

        public async Task<ActionResult> Billboard()
        {
            List<Movie> Movies = new List<Movie>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/movies");

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    Movies = JsonConvert.DeserializeObject<List<Movie>>(response);

                }
                return View(Movies);
            }
        }

        public ActionResult Details(int id)
        {
            Movie movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync($"api/movies/{id}");
                var result = responseTask.Result;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var readTask = result.Content.ReadAsAsync<Movie>();
                        readTask.Wait();
                        movie = readTask.Result;

                        if (movie.functions != null)
                        {
                            if (movie.functions.Count() > 0)
                            {
                                movie.moviePremier = movie.functions.First().time.ToString("dddd, MMMM dd, yyyy", new CultureInfo("es-ES"));
                            }
                        }
                        break;
                    case HttpStatusCode.NotFound:
                        return HttpNotFound();
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Debug.WriteLine("Codigo: "+ result.StatusCode);
            }
            return View(movie);
        }  

        public ActionResult Create()
        {
            ViewBag.InfoMessage = TempData["InfoMessage"];
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            movie.movieID = null;
            movie.type = "default";
            Debug.WriteLine(JsonConvert.SerializeObject(movie));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);

                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
                var postTask = client.PostAsJsonAsync<Movie>("api/movies", movie);

                postTask.Wait();
                var res = postTask.Result;

                if (res.StatusCode == HttpStatusCode.Created)
                {
                    TempData["InfoMessage"] = String.Concat("Película ",movie.name, " registrada correctamente");
                    return RedirectToAction("create", "movie");
                }
                else if (res.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("unauthorized", "error");
                }

                Debug.WriteLine("ERROR: " + res.StatusCode);
                ModelState.AddModelError(String.Empty, "Ocurrió un error al tratar de crear una nueva pelicula");
                return RedirectToAction("create", "movie");
            }
        }
         public async Task<string> getMovies(string movie)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(String.Concat("api/movies?movie=",movie));

                if (res.IsSuccessStatusCode)
                {
                    return res.Content.ReadAsStringAsync().Result;
                }
            }
            return "[]";
        }

        public async Task<ActionResult> searchMovies(string name)
        {
            List<Movie> Movies = new List<Movie>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(String.Concat("api/movies?name=",name));

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    Movies = JsonConvert.DeserializeObject<List<Movie>>(response);

                }

                return View("~/Views/Movie/Billboard.cshtml", Movies);
            }
        }

        public ActionResult Edit(int id )
        {
            Movie movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync($"api/movies/{id}?opc=1");
                var result = responseTask.Result;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var readTask = result.Content.ReadAsAsync<Movie>();
                        readTask.Wait();
                        movie = readTask.Result;

                        if (movie.functions != null)
                        {
                            if (movie.functions.Count() > 0)
                            {
                                movie.moviePremier = movie.functions.First().time.ToString("dddd, MMMM dd, yyyy", new CultureInfo("es-ES"));
                            }
                        }
                        break;
                    case HttpStatusCode.NotFound:
                        return HttpNotFound();
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Debug.WriteLine("Codigo: " + result.StatusCode);
            }
            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            movie.type = "default";
            using (var client = new HttpClient())
            {
                Debug.WriteLine("Registro: "+JsonConvert.SerializeObject(movie));

                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var putTask = client.PutAsJsonAsync($"api/movies/{movie.movieID}", movie);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit");
                }
                Debug.WriteLine("Codigo de error: "+result.StatusCode);   
            }
            return View(movie);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var deleteTask = client.DeleteAsync($"api/movies/{id}");
                deleteTask.Wait();

                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    Movie movie = JsonConvert.DeserializeObject<Movie>(response);
                    TempData["InfoMessage"] = String.Concat("Película: ", movie.name, ", eliminada correctamente.");
                    return RedirectToAction("index", "movie");
                }
                else if (result.StatusCode == HttpStatusCode.NotAcceptable)
                {
                    TempData["ErrorMessage"] = "Error el registro no puede ser eliminado, porque este recurso está siendo utilizado por el sistema";
                    return RedirectToAction("index", "movie");
                }
                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["ErrorMessage"] = "Error el registro no puede ser eliminado, <strong>porque este recurso no existe</strong>";
                    return RedirectToAction("index", "movie");
                }
                TempData["ErrorMessage"] = "Error interno del sistema, no se tuvo acceso al recurso solicitado";
                return RedirectToAction("index", "movie");
            }
        }

    }
}