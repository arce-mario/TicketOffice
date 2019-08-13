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
    public class RoomController : Controller
    {
        public async Task<string> getRooms(string name = "")
        {
            using (var client = new HttpClient())
            {
                if (name != "")
                {
                    client.BaseAddress = new Uri(LoginController.BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage res = await client.GetAsync(String.Concat("api/rooms?name=", name));

                    if (res.IsSuccessStatusCode)
                    {
                        return res.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            return "[]";
        }

        public async Task<ActionResult> Index()
        {
            List<Room>rooms = new List<Room>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync("api/rooms");

                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(response);
                    rooms = JsonConvert.DeserializeObject<List<Room>>(response);
                    ViewBag.InfoMessage = (TempData["InfoMessage"] == null) ? "" : TempData["InfoMessage"].ToString();
                    ViewBag.ErrorMessage = (TempData["ErrorMessage"] == null) ? "" : TempData["ErrorMessage"].ToString();
                }
                return View(rooms);
            }
        }

        public ActionResult Create()
        {
            ViewBag.OkMessage = (TempData["shortMessage"] == null)? "" : TempData["shortMessage"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Room room, string seatsList = "")
        {
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
                    var postTask = client.PostAsJsonAsync<Room>(String.Concat("api/rooms?listSeats=",seatsList), room);

                    postTask.Wait();
                    var res = postTask.Result;

                    if (res.StatusCode == HttpStatusCode.Created)
                    {
                        TempData["shortMessage"] = String.Concat("Registro: Sala ", room.number, " creado con éxito."); ;
                        return RedirectToAction("create","room");
                    }
                    else if (res.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("unauthorized", "error");
                    }

                    Debug.WriteLine("ERROR ROOM: " + res.StatusCode);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + e.Message);
                }

                ModelState.AddModelError(String.Empty, "Ocurrió un error al tratar de crear el nuevo registro");
                return RedirectToAction("create", "room"); ;
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var deleteTask = client.DeleteAsync($"api/rooms/{id}");
                deleteTask.Wait();

                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    
                    var response = result.Content.ReadAsStringAsync().Result;
                    Room room = JsonConvert.DeserializeObject<Room>(response);
                    TempData["InfoMessage"] = String.Concat("Registro, Sala ", room.number, " eliminado correctamente.");
                    return RedirectToAction("index", "room");
                }
                else if(result.StatusCode == HttpStatusCode.NotAcceptable)
                {
                    TempData["ErrorMessage"] = "Error el registro no puede ser eliminado, porque este recurso está siendo utilizado por el sistema";
                    return RedirectToAction("index","room");
                }
                TempData["ErrorMessage"] = "Error interno del sistema, no se tuvo acceso al recurso solicitado";
                return RedirectToAction("index","room");
            }
        }

    }
}