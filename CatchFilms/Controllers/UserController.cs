using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Edit(int id)
        {
            User user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);

                var responseTask = client.GetAsync("api/users/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<User>();
                    readTask.Wait();
                    user = readTask.Result;
                }

            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            user.birthDate = Convert.ToDateTime(user.birthDate.ToString("dd-MM-yyyy"));
            //movie.type = "default";
            using (var client = new HttpClient())
            {
                Debug.WriteLine("Registro: " + JsonConvert.SerializeObject(user));

                client.BaseAddress = new Uri(LoginController.BaseUrl);
                var putTask = client.PutAsJsonAsync($"api/users/{user.userID}", user);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Edit");
                }
                Debug.WriteLine("Codigo de error: " + result.StatusCode);
            }
            return View(user);
        }
    }
}