using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class CourseController : Controller
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

                    List<CourseListView> lstCourses = (from d in db.courses
                                                       select new CourseListView
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           id_teacher = d.id_teacher
                                                       }).ToList();

                    CourseGeneral courseGeneral = new CourseGeneral
                    {
                        TeacherListViews = lstTeachers,
                        CourseListViews = lstCourses
                    };

                    return View(courseGeneral);
                }

            }

            return RedirectToAction("Login", "Auth");

        }

        [HttpPost]
        public ActionResult CreateCourse(CourseListView courseListView)
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
                    cours course = new cours();
                    course.id = courseListView.id;
                    course.name = courseListView.name;
                    course.id_teacher = courseListView.id_teacher;

                    db.courses.Add(course);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Curso creado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetCourse(int id)
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
                    var course = db.courses.Find(id);
                    CourseListView courseListView = new CourseListView();
                    courseListView.id = course.id;
                    courseListView.name = course.name;
                    courseListView.id_teacher = course.id_teacher;

                    return Json(new { success = true, data = courseListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateCourse(CourseListView courseListView)
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
                    var course = db.courses.Find(courseListView.id);
                    course.id = courseListView.id;
                    course.name = courseListView.name;
                    course.id_teacher = courseListView.id_teacher;

                    db.Entry(course).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Curso actualizado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteCourse(int id)
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
                    var course = db.courses.Find(id);
                    db.courses.Remove(course);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Curso eliminado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }   
    }
}