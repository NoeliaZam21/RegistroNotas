using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class EstudentController : Controller
    {
        public ActionResult Index()

        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {

                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    List<EstudentListView> lstEstudent = (from d in db.estudents
                                                          select new EstudentListView
                                                          {
                                                              id = d.id,
                                                              name = d.name,
                                                              last_name = d.last_name,
                                                              dni = d.dni,
                                                              email = d.email
                                                          }).ToList();

                    return View(lstEstudent);
                }
            }

            return RedirectToAction("Login", "Auth");
            
        }

        [HttpPost]
        public ActionResult CreateEstudent(EstudentListView estudentListView)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    var oEstudent = new estudent();
                    oEstudent.name = estudentListView.name;
                    oEstudent.last_name = estudentListView.last_name;
                    oEstudent.dni = estudentListView.dni;
                    oEstudent.email = estudentListView.email;
                    db.estudents.Add(oEstudent);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Guardado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetEstudent(int id)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    var oEstudent = db.estudents.Find(id);
                    EstudentListView estudentListView = new EstudentListView();
                    estudentListView.id = oEstudent.id;
                    estudentListView.name = oEstudent.name;
                    estudentListView.last_name = oEstudent.last_name;
                    estudentListView.dni = oEstudent.dni;
                    estudentListView.email = oEstudent.email;

                    return Json(new { success = true, data = estudentListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateEstudent(EstudentListView estudentListView)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    var oEstudent = db.estudents.Find(estudentListView.id);
                    oEstudent.name = estudentListView.name;
                    oEstudent.last_name = estudentListView.last_name;
                    oEstudent.dni = estudentListView.dni;
                    oEstudent.email = estudentListView.email;
                    db.Entry(oEstudent).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Actualizado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteEstudent(int id)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    var oEstudent = db.estudents.Find(id);
                    db.estudents.Remove(oEstudent);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Eliminado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}