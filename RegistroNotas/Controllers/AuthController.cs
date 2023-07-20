using System;
using System.Linq;
using System.Web.Mvc;
using RegistroNotas.Models;

namespace RegistroNotas.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            if (Session["State"] != null && Session["State"].ToString() == "Logged")
            {
                return RedirectToAction("Index", "Home");
            }

            return View("Login");
        }

        [HttpPost]
        public ActionResult LoginMethod(string username, string password)
        {
            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    var oUser = (from d in db.users where d.name == username && d.password == password select d).FirstOrDefault();

                    if (oUser == null)
                    {
                        ViewBag.Error = "Usuario o contraseña incorrectos";
                        
                        return Json(new { success = false, message = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);
                    }


                    Session["State"] = "Logged";
                    Session["User"] = oUser.name;
                    Session["Role"] = oUser.id_role;
                    Session["Id"] = oUser.id;
                    Session["Id_Student"] = oUser.id_student;
                    
                    return Json(new { success = true, message = "Login Correcto" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
               return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            Session["State"] = null;
            Session["User"] = null;
            Session["Role"] = null;
            Session["Id"] = null;

            return RedirectToAction("Login");
        }
    }
}