using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            //Esto permite indicarle al cliente web el código de respuesta 404
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Unauthorized()
        {
            //Esto permite indicarle al cliente web el código de respuesta 401
            Response.StatusCode = 401;
            return View();
        }
    }
}