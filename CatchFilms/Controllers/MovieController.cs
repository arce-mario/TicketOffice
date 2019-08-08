using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
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

                if (Session["userAutentication"] != null)
                {                    
                    client.DefaultRequestHeaders.Authorization = new 
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }

                HttpResponseMessage res = await client.GetAsync("api/movies");

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    Movies = JsonConvert.DeserializeObject<List<Movie>>(response);

                }
                else if (res.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("unauthorized", "error");
                }
                Debug.WriteLine("Codigo de respuesta: "+res.StatusCode);
                return View(Movies);
            }
        }

        public ActionResult Details(int? id)
        {
            Movie movie = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(LoginController.BaseUrl);
                    if (Session["userAutentication"] != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new
                            AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                    }
                    var responseTask = client.GetAsync(String.Concat("api/movies/", id));
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Movie>();
                        readTask.Wait();
                        movie = readTask.Result;
                    }
                }
            }

            return View(movie);
        }
    }
}