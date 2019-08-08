using CatchFilms.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
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
        public const string BaseUrl = "http://192.168.43.37/apicatchfilms/";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValidateUser(LoginUser loginUser)
        {
            Debug.WriteLine(String.Concat("Nueva solicitud de inicio de sesión: ",loginUser.User));
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
                        //Se almacena en la sesión el accesstoken
                        Session["userAutentication"] = responseBody.token;
                        //Se guarda en la sesión los datos del usuario
                        var handler = new JwtSecurityTokenHandler();
                        var rsToken = handler.ReadToken(responseBody.token) as JwtSecurityToken;

                        JObject session = JObject.Parse(rsToken.ToString()
                            .Split(new string[] { "}." }, StringSplitOptions.None)[1]);

                        Session["sessionData"] = new SessionData() {
                            rolID = int.Parse(session["rolID"].ToString()),
                            firstName = session["firstName"].ToString(),
                            lastName = session["lastName"].ToString(),
                            nameID = session["nameid"].ToString(),
                            email = session["email"].ToString()
                        };

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