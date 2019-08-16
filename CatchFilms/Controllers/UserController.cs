using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Edit(int id)
        {
            validationAuntentication(2);
            SessionData login = (SessionData)Session["sessionData"];

            if (login.userID != id)
            {
                return RedirectToAction("Unauthorized", "error");
            }

            User user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
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
            validationAuntentication(2);
            SessionData login = (SessionData)Session["sessionData"];

            if (login.userID != user.userID)
            {
                return RedirectToAction("Unauthorized", "error");
            }

            user.birthDate = Convert.ToDateTime(user.birthDate.ToString("dd-MM-yyyy"));
            using (var client = new HttpClient())
            {
                Debug.WriteLine("Registro: " + JsonConvert.SerializeObject(user));

                client.BaseAddress = new Uri(LoginController.BaseUrl);
                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
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

        [HttpPost]
        public ActionResult ProfileUser(User Users)
        {
            validationAuntentication(2);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                if (Session["userAutentication"] != null)
                {
                    client.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", Session["userAutentication"].ToString());
                }
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