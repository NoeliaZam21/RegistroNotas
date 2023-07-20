using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class NoteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotesEstudent()
        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                int id = Convert.ToInt32(Session["Id_Student"]);

                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    List<NoteListView> stNotes = (from d in db.notes
                                                                                       
                                             where d.id_student == id
                                                 select new NoteListView
                                                 {
                                                      id = d.id,
                                                      note_1 = d.note_1,
                                                      note_2 = d.note_2,
                                                      note_3 = d.note_3,
                                                      note_4 = d.note_4,
                                                      id_student = d.id_student,
                                                      id_course = d.id_course,
                                                  }).ToList();

                    List<CourseListView> courseListViews = (from d in db.courses
                                                            select new CourseListView
                                                            {
                                                                id = d.id,
                                                                name = d.name,
                                                                id_teacher = d.id_teacher,
                                                            }).ToList();

                    NoteGeneral noteGeneral = new NoteGeneral
                    {
                        noteListViews = stNotes,
                        courseListViews = courseListViews
                    };

                    return View("ViewNotes", noteGeneral);
                }   
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult GetNoteById(int id) 
        {

            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                try
                {
                    //Get Note by id
                    using (registro_notasEntities1 db = new registro_notasEntities1())
                    {
                        NoteListView noteListView = (from d in db.notes where d.id == id select new NoteListView {
                                                             id = d.id,
                                                              note_1 = d.note_1,
                                                              note_2 = d.note_2,
                                                              note_3 = d.note_3,
                                                              note_4 = d.note_4,
                                                              id_student = d.id_student,
                                                              id_course = d.id_course,
                                                          }).FirstOrDefault();

                        //Get course by id
                        CourseListView courseListView = (from d in db.courses where d.id == noteListView.id_course select new CourseListView
                        {
                                                              id = d.id,
                                                              name = d.name,
                                                              id_teacher = d.id_teacher,
                                                          }).FirstOrDefault();

                        //Get teacher by id
                        TeacherListView teacherListView = (from d in db.teachers where d.id == courseListView.id_teacher select new TeacherListView
                        {
                                                              id = d.id,
                                                              name = d.name,
                                                              last_name = d.last_name,
                                                          }).FirstOrDefault();

                        //Get Subjet by id teacher
                        SubjectListView subjectListView = (from d in db.subjects where d.id_teacher == teacherListView.id select new SubjectListView
                        {
                                                              id = d.id,
                                                              name = d.name,
                                                              id_teacher = d.id_teacher,
                                                          }).FirstOrDefault();


                        return Json(new { success = true, data = noteListView, course = courseListView, teacher = teacherListView, subject = subjectListView }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }   

            return RedirectToAction("Login", "Auth");
        }

        public ActionResult NotesTeacher()
        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {


                using (registro_notasEntities1 db = new registro_notasEntities1())
                {


                    List<NoteListView> stNotes = (from d in db.notes
                                                  select new NoteListView
                                                  {
                                                      id = d.id,
                                                      note_1 = d.note_1,
                                                      note_2 = d.note_2,
                                                      note_3 = d.note_3,
                                                      note_4 = d.note_4,
                                                      id_student = d.id_student,
                                                      id_course = d.id_course,
                                                  }).ToList();

                    List<EstudentListView> lstUsers = (from d in db.estudents
                                                       select new EstudentListView
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           last_name = d.last_name,
                                                       }).ToList();

                    List<CourseListView> courseListViews = (from d in db.courses
                                                            select new CourseListView
                                                            {
                                                                id = d.id,
                                                                name = d.name,
                                                                id_teacher = d.id_teacher,
                                                            }).ToList();

                    NoteGeneral noteGeneral = new NoteGeneral
                    {
                        estudentListViews = lstUsers,
                        noteListViews = stNotes,
                        courseListViews = courseListViews
                    };

                    return View("ManagingNotes", noteGeneral);
                }

            }

            return RedirectToAction("Login", "Auth");
        }

        public ActionResult GetNote(int id)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    NoteListView noteListView = (from d in db.notes
                                                                                                 where d.id == id
                                                                                                                                                 select new NoteListView
                                                                                                                                                 {
                                                     id = d.id,
                                                     note_1 = d.note_1,
                                                     note_2 = d.note_2,
                                                     note_3 = d.note_3,
                                                     note_4 = d.note_4,
                                                     id_student = d.id_student,
                                                     id_course = d.id_course,
                                                 }).FirstOrDefault();

                    return Json(new { success = true, data = noteListView }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult CreateNote(NoteListView noteListView)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    note note = new note();
                    note.note_1 = noteListView.note_1;
                    note.note_2 = noteListView.note_2;
                    note.note_3 = noteListView.note_3;
                    note.note_4 = noteListView.note_4;
                    note.id_student = noteListView.id_student;
                    note.id_course = noteListView.id_course;

                    db.notes.Add(note);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Nota creada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateNote (NoteListView noteListView)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    note note = db.notes.Find(noteListView.id);
                    note.note_1 = noteListView.note_1;
                    note.note_2 = noteListView.note_2;
                    note.note_3 = noteListView.note_3;
                    note.note_4 = noteListView.note_4;
                    note.id_student = noteListView.id_student;
                    note.id_course = noteListView.id_course;

                    db.Entry(note).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Nota editada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }   
        }

        [HttpPost]
        public ActionResult DeleteNote (int id)
        {
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    note note = db.notes.Find(id);
                    db.notes.Remove(note);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Nota eliminada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}