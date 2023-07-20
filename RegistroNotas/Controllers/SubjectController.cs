using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class SubjectController : Controller
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

                    List<SubjectListView> lstCourses = (from d in db.subjects
                                                        select new SubjectListView
                                                        {
                                                            id = d.id,
                                                            name = d.name,
                                                            description = d.description,
                                                            id_teacher = d.id_teacher
                                                        }).ToList();

                    SubjectGeneral subjectGeneral = new SubjectGeneral
                    {
                        teacherListViews = lstTeachers,
                        subjectListViews = lstCourses
                    };

                    return View(subjectGeneral);
                }
            }

            

            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult CreateSubject(SubjectListView subjectListView)
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
                    subject subject = new subject();
                    subject.id = subjectListView.id;
                    subject.name = subjectListView.name;
                    subject.description = subjectListView.description;
                    subject.id_teacher = subjectListView.id_teacher;

                    db.subjects.Add(subject);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Se ha creado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetSubject(int id)
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
                    SubjectListView subjectListView = (from d in db.subjects
                                                                                                             where d.id == id
                                                                                                                                                                   select new SubjectListView
                                                                                                                                                                   {
                                                           id = d.id,
                                                           name = d.name,
                                                           description = d.description,
                                                           id_teacher = d.id_teacher
                                                       }).FirstOrDefault();

                    return Json(new { success = true, data = subjectListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }   
        }

        [HttpPost]
        public ActionResult UpdateSubject(SubjectListView subjectListView)
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
                    subject subject = db.subjects.Find(subjectListView.id);
                    subject.name = subjectListView.name;
                    subject.description = subjectListView.description;
                    subject.id_teacher = subjectListView.id_teacher;

                    db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Se ha actualizado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteSubject(int id)
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
                    subject subject = db.subjects.Find(id);
                    db.subjects.Remove(subject);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Se ha eliminado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}