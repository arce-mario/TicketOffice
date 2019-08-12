using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Create()
        {
            return View();
        }
    }
}