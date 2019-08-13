﻿using CatchFilms.Models;
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            movie.movieID = null;
            Debug.WriteLine(JsonConvert.SerializeObject(movie));

            using (var client = new HttpClient())
            {
                try
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

                    if (res.IsSuccessStatusCode)
                    {
                        return RedirectToAction("create", "movie");
                    }
                    else if (res.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("unauthorized", "error");
                    }

                    Debug.WriteLine("ERROR: " + res.StatusCode);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + e.Message);
                }

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

        // CONTINUAR EDIT
        public ActionResult Edit(int id )
        {
            Movie movie = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);

                var responseTask = client.GetAsync("api/movies/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Movie>();
                    readTask.Wait();
                    movie = readTask.Result;
                }

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
           
    }
}