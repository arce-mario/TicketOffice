using ApiCatchFilms.Models;
using CatchFilms.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;

namespace CatchFilms.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class LoginController : Controller
    {
        string BaseUrl = "http://192.168.43.37/apicatchfilms/";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValidateUser(LoginUser loginUser)
        {
            Debug.WriteLine(String.Concat("Nueva solicitud de inicio de sesión: ",loginUser.User));
            Session["userAutentication"] = null;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var response = await client.PostAsJsonAsync("api/login", loginUser);
                    Token responseBody = await response.Content.ReadAsAsync<Token>();

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(string.Concat("Resultado: ", responseBody.token,responseBody.Message));
                        Session["userAutentication"] = responseBody.token; 
                        return RedirectToAction("index","home");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + e.Message);
                }

                ModelState.AddModelError(String.Empty, "Ocurrió un error al tratar de crear el nuevo registro");
                return RedirectToAction("login", "home"); ;
            }
        }
    }
}