using RegistroNotas.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;

namespace RegistroNotas.Controllers
{

    public class UserController : Controller
    {
        public ActionResult Index()
        {
            using (registro_notasEntities1 db = new registro_notasEntities1())
            {
                if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
                {
                    List<UserListView> lstUsers = (from d in db.users
                                                   select new UserListView
                                                   {
                                                       id = d.id,
                                                       name = d.name,
                                                       password = d.password,
                                                       id_role = d.id_role,
                                                       id_student = d.id_student,
                                                       id_teacher = d.id_teacher
                                                   }).ToList();

                    List<TeacherListView> lstTeachers = (from d in db.teachers
                                                         select new TeacherListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             last_name = d.last_name,
                                                             specialty = d.specialty,
                                                             email = d.email
                                                         }).ToList();

                    List<EstudentListView> lstEstudent = (from d in db.estudents
                                                          select new EstudentListView
                                                          {
                                                              id = d.id,
                                                              name = d.name,
                                                              last_name = d.last_name,
                                                              dni = d.dni,
                                                              email = d.email
                                                          }).ToList();
                    UserGeneraView userGeneraView = new UserGeneraView
                    {
                        TeacherListViews = lstTeachers,
                        EstudentListViews = lstEstudent,
                        UserListViews = lstUsers

                    };

                    return View(userGeneraView);
                }


                return RedirectToAction("Login", "Auth");
            }   
            
        }

        [HttpPost]
        public ActionResult CreateUser(UserListView userListView)
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
                    var oUser = new user();
                    oUser.name = userListView.name;
                    oUser.password = userListView.password;
                    oUser.id_role = userListView.id_role;
                    oUser.id_student = userListView.id_student;
                    oUser.id_teacher = userListView.id_teacher;
                    db.users.Add(oUser);
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
        public ActionResult GetUser(int id)
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
                    UserListView userListView = (from d in db.users
                                                                                                 where d.id == id
                                                                                                                                                 select new UserListView
                                                                                                                                                 {
                                                     id = d.id,
                                                     name = d.name,
                                                     password = d.password,
                                                     id_role = (int)d.id_role,
                                                     id_student = (int)d.id_student,
                                                     id_teacher = (int)d.id_teacher
                                                 }).FirstOrDefault();

                    return Json(new { success = true, data = userListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(UserListView userListView)
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
                    var oUser = db.users.Find(userListView.id);
                    oUser.name = userListView.name;
                    oUser.password = userListView.password;
                    oUser.id_role = userListView.id_role;
                    oUser.id_student = userListView.id_student;
                    oUser.id_teacher = userListView.id_teacher;
                    db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
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
        public ActionResult DeleteUser(int id)
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
                    var oUser = db.users.Find(id);
                    db.users.Remove(oUser);
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