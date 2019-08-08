using System.Web.Mvc;

namespace CatchFilms.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            Session["userAutentication"] = null;
            Session["sessionData"] = null;
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
    }
}