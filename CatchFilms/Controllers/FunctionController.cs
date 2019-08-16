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
        [HttpGet]
        public ActionResult Details(int id)
        {
            Function function = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync($"api/functions/{id}");
                var result = responseTask.Result;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        ViewBag.InfoMessage = TempData["InfoMessage"];
                        var readTask = result.Content.ReadAsAsync<Function>();
                        readTask.Wait();
                        function = readTask.Result;
                        Session["function"] = function;
                        break;
                    case HttpStatusCode.NotFound:
                        return HttpNotFound();
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Debug.WriteLine("Codigo: " + result.StatusCode);
            }
            return View(function);
        }
        
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.InfoMessage = TempData["InfoMessage"];
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(Function function)
        {
            if (function.movie != null)
                function.movie.type = "Default";
            TryValidateModel(function);

            if ((function.movieID == null && function.movie == null) || function.roomID == null || (function.priceID == null && function.price == null))
            {
                ModelState.AddModelError(String.Empty, "Debe definir o selecionar los campos de Película, Precio y Sala");
                return View(function);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Se encontraron uno o más errores en los campos.");
                return View(function);
            }

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
                    Debug.WriteLine("ERROR: " + res.StatusCode);
                    switch (res.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            TempData["InfoMessage"] = String.Concat("Función \"",function.description,"\" registrada exitosamente en el sistema");
                            return RedirectToAction("create","function");
                        case HttpStatusCode.Unauthorized:
                            return RedirectToAction("unauthorized", "error");
                        case HttpStatusCode.NotAcceptable:
                            ModelState.AddModelError(String.Empty, "No se encontraron registro de precios por defectos");
                            break;
                        default:
                            return RedirectToAction("internalservererror","error");
                    }
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

    }
}