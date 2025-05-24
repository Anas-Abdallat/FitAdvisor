using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers.Admin
{
    public class ManageCoursesAdminController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: ManageCoursesAdmin
        public ActionResult Index()
        {
            var courses =  db.COURSEs.ToList();
            return View(courses);
        }

        public ActionResult AddnewCourses()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddnewCourses(COURSE course)
        {
            if (ModelState.IsValid)
            {
                db.COURSEs.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        public ActionResult DeleteCourses(int id)
        {
            var course = db.COURSEs.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourseConfirmed(int id)
        {
            var course = db.COURSEs.Find(id);
            var instructor_course = db.INSTRUCTOR_COURSE.Where(u => u.ID_COURSE == course.ID).ToList();
            var student_course = db.STUDENT_COURSE.Where(u => u.ID_COURSE == course.ID).ToList();
            var homework = db.HOMEWORK.Where(u => u.ID_COURSE == course.ID).ToList();
            if (course == null)
            {
                return HttpNotFound();
            }
            if(instructor_course != null)
            {
                foreach (var instructor in instructor_course)
                {
                    db.INSTRUCTOR_COURSE.Remove(instructor);
                    db.SaveChanges();
                }
            }
            if (student_course != null)
            {
                foreach (var student in student_course)
                {
                    db.STUDENT_COURSE.Remove(student);
                    db.SaveChanges();
                }
            }
            if (homework != null)
            {
                foreach (var hw in homework)
                {
                    db.HOMEWORK.Remove(hw);
                    db.SaveChanges();
                }
            }
            db.COURSEs.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditCourses(int id)
        {
            var course = db.COURSEs.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourses(COURSE course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult EnrollCourses(int id)
        {
            var course = db.COURSEs.Find(id);

            ViewBag.Instructors = db.INSTRUCTORs.ToList();
            ViewBag.Students = db.STUDENTs.ToList();

            return View(course);
        }

        [HttpPost]
        public ActionResult EnrollCourses(int id, int instructorId)
        {

            var course = db.COURSEs.Find(id);
            var instructor = db.INSTRUCTORs.Find(instructorId);

            if (course != null && instructor != null)
            {
                var instructorCourse = new INSTRUCTOR_COURSE
                {
                    ID_INSTRUCTOR = instructorId,
                    ID_COURSE = id,
                    INSTRUCTOR_NOTE = string.Empty,
                    COURSE = db.COURSEs.Find(id),
                    INSTRUCTOR = db.INSTRUCTORs.Find(instructorId)
                };

                db.INSTRUCTOR_COURSE.Add(instructorCourse);
                db.SaveChanges();
                course.INSTRUCTOR_COURSE = new List<INSTRUCTOR_COURSE>();
                course.INSTRUCTOR_COURSE.Add(instructorCourse);
                ///var updatedCourse = course;
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                var tes = db.INSTRUCTOR_COURSE.ToList();
                return RedirectToAction("Index");
            }

            return View("Error");
        }

    }
}