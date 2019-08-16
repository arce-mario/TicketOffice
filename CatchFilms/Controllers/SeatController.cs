using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class SeatController : Controller
    {
        public ActionResult buyTickets(int id, string data = "")
        {
            TempData["queryData"] = data;
            TempData["roomID"] = id;
            return RedirectToAction("seatstickets","seat");
        }

        public ActionResult seatsTickets()
        {
            validationAuntentication(2);

            Info info = new Info();
            String data = (TempData["queryData"] == null) ? "" : TempData["queryData"].ToString();
            int id = (TempData["roomID"] == null) ? 0 : int.Parse(TempData["roomID"].ToString());
            Function function = (Function)Session["function"];

            if (data == "" || id == 0 || function == null)
            {
                return RedirectToAction("notfound", "error");
            }
            
            InfoTickets infoTicket = JsonConvert.DeserializeObject<InfoTickets>(data);
            info.infoTickets = infoTicket;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
                var responseTask = client.GetAsync($"api/rooms/{id}");
                var result = responseTask.Result;
                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        ViewBag.InfoMessage = TempData["InfoMessage"];
                        ViewBag.price = (function.price != null)?JsonConvert.SerializeObject(function.price) : null;
                        var readTask = result.Content.ReadAsAsync<Room>();
                        readTask.Wait();
                        info.room = readTask.Result;
                        Room tempRoom = defineSeatArray(info.room.roomSeats);
                        info.room.seatArray = tempRoom.seatArray;
                        info.room.rows = tempRoom.rows;
                        info.room.columns = tempRoom.columns;
                        info.room.notAvailable = tempRoom.notAvailable;
                        //Luego de haber generado el atributo room.seatArray la lista de RoomSeats ya no es necesaría
                        info.room.roomSeats = null;
                        break;
                    case HttpStatusCode.NotFound:
                        return HttpNotFound();
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Debug.WriteLine("Codigo: " + result.StatusCode);
            }
            return View(info);
        }

        [HttpPost]
        public ActionResult saveTickets(Info info)
        {
            Room room = info.room;
            Function function = (Function) Session["function"];
            SessionData user = (SessionData)Session["sessionData"];
            String notAvailable = room.notAvailable;
            Debug.WriteLine(notAvailable);

            if (function == null || user == null)
            {
                ModelState.AddModelError(String.Empty, "Error interno del sistema, no se logró obtener toda la información requerida por la solicitud, revice los campos de formulario y su inicio de sesión");
                return View(info);
            }

            Ticket ticket = new Ticket() {
                createAT = DateTime.UtcNow,
                functionID = function.functionID,
                priceID = function.price.priceID,
                userID = user.userID,
                roomSeatID = 0
            };

            Debug.WriteLine("Ticket: "+JsonConvert.SerializeObject(ticket));
            var uri = $"api/tickets?notAvailable={notAvailable}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
                var postTask = client.PostAsJsonAsync(uri, ticket);
                postTask.Wait();
                var res = postTask.Result;
                Debug.WriteLine("ERROR: "+ res.StatusCode);
                switch (res.StatusCode)
                {
                    case HttpStatusCode.NoContent:
                        TempData["InfoMessage"] = "Transacción realizada correctamente";
                        return RedirectToAction("Billboard", "movie");
                    case HttpStatusCode.Created:
                        TempData["InfoMessage"] = "Transacción realizada correctamente";
                        return RedirectToAction("Billboard", "movie");
                    default:
                        TempData["ErrorMessage"] = "Ocurrió un error en el sistema, al realizar la trasaccion";
                        return View(info);
                }
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
                        .Where(r => ((int)r.seat.row.ToCharArray()[0] == (defaultASCII + i)) && r.seat.column == (c + 1)).FirstOrDefault();
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
            return new Room() { seatArray = JsonConvert.SerializeObject(seatArray), notAvailable = JsonConvert.SerializeObject(notAvailable), rows = rows, columns = columns };
        }

        private ActionResult validationAuntentication(int opc)
        {
            if (opc == 1)
            {
                SessionData user = (SessionData)Session["sessionData"];
                if (user != null)
                {
                    if (user.rolID != 1) { return RedirectToAction("Unauthorized", "error"); }
                }
                else
                {
                    return RedirectToAction("Unauthorized", "error");
                }
            }
            else
            {
                if (Session["sessionData"] != null) { return RedirectToAction("Unauthorized", "error"); }
            }
            return null;
        }
    }
}