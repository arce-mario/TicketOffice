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
        public async Task<List<Function>> List(HttpStatusCode statusCode, int movieID)
        {
            List<Function> functions = new List<Function>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(LoginController.BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(String.Concat("api/functions/movie/",movieID));

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