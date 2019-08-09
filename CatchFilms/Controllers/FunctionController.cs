using CatchFilms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        public async Task<List<Function>> List(HttpStatusCode statusCode, string accessToken)
        {
            List<Function> functions = new List<Function>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new
                       AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage res = await client.GetAsync("api/functions");

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
    }
}