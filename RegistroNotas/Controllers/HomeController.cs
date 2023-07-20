using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroNotas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {

                return View();
            }

            return RedirectToAction("Login", "Auth");
        }
    }
}