using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class RegistraionController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: Registraion
        public ActionResult Index()
        {
            var courses = db.COURSEs.ToList();
            return View(courses);
        }

        public ActionResult OnlineRegestraion(int id)
        {
            var course = db.COURSEs.Find(id);
            return View(course);
        }

        [HttpPost]
        public ActionResult OnlineRegestraion(int id, int studentId)
        {
            var course = db.COURSEs.Find(id);
            var student = db.STUDENTs.Find(studentId);

            if (course != null && student != null)
            {
                var studentCourse = new STUDENT_COURSE
                {
                    ID_STUDENT = studentId,
                    ID_COURSE = id,
                    COURSE = db.COURSEs.Find(id),
                    STUDENT = db.STUDENTs.Find(id)
                };

                db.STUDENT_COURSE.Add(studentCourse);
                db.SaveChanges();
                course.STUDENT_COURSE = new List<STUDENT_COURSE>();
                course.STUDENT_COURSE.Add(studentCourse);
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Error");
        }

    }
}