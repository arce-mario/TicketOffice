using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class FunctionController : Controller
    {
        // GET: Function
        public async Task<List<Function>> List(HttpStatusCode statusCode, int movieID)
        {
            List<Function> functions = new List<Function>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(String.Concat("api/functions/movie/", movieID));

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    functions = JsonConvert.DeserializeObject<List<Function>>(response);
                }
                statusCode = res.StatusCode;
                Debug.WriteLine("Codigo de respuesta: " + res.StatusCode);
                return functions;
            }
        }

        public ActionResult Details(int? id)
        {
            Function function = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(LoginController.BaseUrl);
                    var responseTask = client.GetAsync(String.Concat("api/functions/", id));
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Function>();
                        readTask.Wait();
                        function = readTask.Result;
                    }
                }
            }

            if (function == null) { return new HttpStatusCodeResult(HttpStatusCode.NotFound); }
            return View(function);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Function function)
        {
            function.functionID = null;

            function.price.priceID = (function.price.priceID == 0) ? null : function.price.priceID;
            function.room.roomID = (function.room.roomID == 0) ? null : function.room.roomID;

            function.priceID = (function.priceID == 0) ? null : function.priceID;
            function.movieID = (function.movieID == 0) ? null : function.movieID;
            function.roomID = (function.roomID == 0) ? null : function.roomID;

            function.movie.type = "Default";
            Debug.WriteLine(String.Concat("FunctionController :: Create() :: function: ",JsonConvert.SerializeObject(function)));
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
                    var postTask = client.PostAsJsonAsync<Function>("api/functions", function);

                    postTask.Wait();
                    var res = postTask.Result;

                    if (res.IsSuccessStatusCode)
                    {
                        ViewBag.message = "Registro creado de forma correcta";
                        return View();
                    }
                    else if (res.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("unauthorized", "error");
                    }else if (res.StatusCode == HttpStatusCode.NotAcceptable)
                    {
                        ModelState.AddModelError(String.Empty, "No se encontraron registro de precios por defectos");
                    }

                    Debug.WriteLine("ERROR: " + res.StatusCode);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + e.Message);
                }
                ModelState.AddModelError(String.Empty, "Ocurrió un error al tratar de crear el nuevo registro");
                ModelState.AddModelError(String.Empty, "El registro de película o sala selecionado se mantienen");
                return View();
            }
        }

        public ActionResult ProfileUser(int id)
        {
            User user = new User();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync(String.Concat("api/users/", id));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<User>();
                    readTask.Wait();
                    user = readTask.Result;

                }
                Debug.WriteLine(result.StatusCode + "holis");
            }
            
            return View(user);
        }

        [HttpPost]
        public ActionResult ProfileUser(User Users)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var putTask = client.PutAsJsonAsync($"api/apiCatchFilms/{Users.userID}", Users);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(Users);
        }

        public ActionResult Edit(int id)
        {
            Function function = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);

                var responseTask = client.GetAsync(String.Concat("api/functions/", id));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Function>();
                    readTask.Wait();
                    function = readTask.Result;
                }

            }

            return View(function);
        }

        [HttpPost]
        public ActionResult Edit(Function function)
        {
            using (var client = new HttpClient())
            {
                Debug.WriteLine("Registro: " + JsonConvert.SerializeObject(function));

                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var putTask = client.PutAsJsonAsync($"api/functions/{function.functionID}", function);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit");
                }
                Debug.WriteLine("Codigo de error: " + result.StatusCode);
            }
            return View(function);
        }

    }
}