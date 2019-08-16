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

        public ActionResult Edit(int id)
        {
            Room room = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var responseTask = client.GetAsync($"api/rooms/{id}");
                var result = responseTask.Result;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        ViewBag.InfoMessage = TempData["InfoMessage"];
                        var readTask = result.Content.ReadAsAsync<Room>();
                        readTask.Wait();
                        room = readTask.Result;
                        Room tempRoom = defineSeatArray(room.roomSeats);
                        room.seatArray = tempRoom.seatArray;
                        room.rows = tempRoom.rows;
                        room.columns = tempRoom.columns;
                        room.notAvailable = tempRoom.notAvailable;
                        //Luego de haber generado el atributo room.seatArray la lista de RoomSeats ya no es necesaría
                        room.roomSeats = null;
                        break;
                    case HttpStatusCode.NotFound:
                        return HttpNotFound();
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Debug.WriteLine("Codigo: " + result.StatusCode);
            }
            return View(room);
        }
        
        [HttpPost]
        public ActionResult Edit(Room room)
        {
            TryValidateModel(room);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Se encontraron uno o más errores en los campos. Sujerencia recargue la página.");
                return View(room);
            }

            var uri = $"api/rooms/{room.roomID}?notAvailable={room.notAvailable}";
            room.rows = null;
            room.columns = null;
            room.roomSeats = null;
            room.notAvailable = null;
            room.seatArray = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var putTask = client.PutAsJsonAsync(uri, room);
                putTask.Wait();
                var result = putTask.Result;

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        TempData["InfoMessage"] = "Datos modificados correctamente";
                        return RedirectToAction("edit","room");
                    case HttpStatusCode.NoContent:
                        TempData["InfoMessage"] = "Datos modificados correctamente";
                        return RedirectToAction("edit","room");
                    case HttpStatusCode.NotAcceptable:
                        TempData["InfoMessage"] = "Algunas butacas no pueden ser modificadas, porque están resevadas";
                        return RedirectToAction("edit", "room");
                    default:
                        return RedirectToAction("internalservererror", "error");
                }
            }
        }
        public ActionResult Create()
        {
            ViewBag.OkMessage = (TempData["shortMessage"] == null) ? "" : TempData["shortMessage"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Room room)
        {
            string seatList = room.seatArray;
            room.seatArray = null;

            TryValidateModel(room);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Se encontraron uno o más errores en los campos");
                return View(room);
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
                    var postTask = client.PostAsJsonAsync<Room>(String.Concat("api/rooms?listSeats=", seatList), room);

                    postTask.Wait();
                    var res = postTask.Result;

                    if (res.StatusCode == HttpStatusCode.Created)
                    {
                        TempData["shortMessage"] = String.Concat("Registro: Sala ", room.number, " creado con éxito."); ;
                        return RedirectToAction("create", "room");
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
                return RedirectToAction("create", "room");
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
        private Room defineSeatArray(List<RoomSeat> roomSeats)
        {
            List<string> seatArray = new List<string>();
            int defaultASCII = 65; int rows = 0; int columns = 0;
            List<string> notAvailable = new List<string>();
            if (roomSeats.Count() == 0)
            {
                return new Room() { seatArray = JsonConvert.SerializeObject(seatArray), rows = rows, columns = columns };
            }
            columns = roomSeats.Where(f => f.seat.row == "A").Count();
            rows = roomSeats.Where(f => f.seat.column == 1).Count();
            for (int i = 0; i < rows; i++)
            {
                string roomRow = "";
                for (int c = 0; c < columns; c++)
                {
                    RoomSeat roomSeat = roomSeats
                        .Where(r => ((int)r.seat.row.ToCharArray()[0] == (defaultASCII + i)) && r.seat.column == (c+1)).FirstOrDefault();
                    Debug.WriteLine("Asiento: " + JsonConvert.SerializeObject(roomSeats));

                    if (roomSeat != null)
                    {
                        roomRow += "a";
                        if (roomSeat.status != 1)
                        {
                            notAvailable.Add(String.Concat(roomSeat.seat.row, "_", roomSeat.seat.column));
                        }
                    }
                }
                if (roomRow != "") { seatArray.Add(roomRow); }
            }
            return new Room() { seatArray = JsonConvert.SerializeObject(seatArray), notAvailable = JsonConvert.SerializeObject(notAvailable), rows = rows, columns = columns};
        }
    }
}