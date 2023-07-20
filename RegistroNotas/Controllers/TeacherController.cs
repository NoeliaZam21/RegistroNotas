using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {

                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    List<TeacherListView> lstTeachers = (from d in db.teachers
                                                         select new TeacherListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             last_name = d.last_name,
                                                             specialty = d.specialty,
                                                             email = d.email
                                                         }).ToList();

                    return View(lstTeachers);
                }
            }

            


            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult CreateTeacher(TeacherListView teacherListView)
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
                    var oTeacher = new teacher();
                    oTeacher.name = teacherListView.name;
                    oTeacher.last_name = teacherListView.last_name;
                    oTeacher.specialty = teacherListView.specialty;
                    oTeacher.email = teacherListView.email;
                    db.teachers.Add(oTeacher);
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
        public ActionResult GetTeacher(int id)
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
                    var oTeacher = db.teachers.Find(id);
                    TeacherListView teacherListView = new TeacherListView();
                    teacherListView.id = oTeacher.id;
                    teacherListView.name = oTeacher.name;
                    teacherListView.last_name = oTeacher.last_name;
                    teacherListView.specialty = oTeacher.specialty;
                    teacherListView.email = oTeacher.email;

                    return Json(new { success = true, data = teacherListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateTeacher(TeacherListView teacherListView)
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
                    var oTeacher = db.teachers.Find(teacherListView.id);
                    oTeacher.name = teacherListView.name;
                    oTeacher.last_name = teacherListView.last_name;
                    oTeacher.specialty = teacherListView.specialty;
                    oTeacher.email = teacherListView.email;
                    db.Entry(oTeacher).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Actualizado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteTeacher(int id)
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
                    var oTeacher = db.teachers.Find(id);
                    db.teachers.Remove(oTeacher);
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