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

        public async Task<ActionResult> Details(int? id)
        {
            dynamic models = new ExpandoObject();
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync(String.Concat("api/movies/", id));
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Movie>();
                    readTask.Wait();
                    models.movie = readTask.Result;
                    List<Function> functions = await new FunctionController().List(statusCode, models.movie.movieID);
                    models.functions = functions;
                    models.moviePremier = functions.First().time.ToString("dddd, MMMM dd, yyyy", new CultureInfo("es-ES"));
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
                Debug.WriteLine("Codigo: "+ result.StatusCode);
                
            }

            return View(models);
        }
    }
}