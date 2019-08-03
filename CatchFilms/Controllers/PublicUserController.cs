using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class PublicUserController : Controller
    {
        // GET: SelectSeats
        public ActionResult SelectSeats()
        {
            return View();
        }
    }
}