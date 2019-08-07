using System.Diagnostics;
using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            Debug.WriteLine("Controlador Home, AccessToken: "+Session["userAutentication"]);
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
    }
}