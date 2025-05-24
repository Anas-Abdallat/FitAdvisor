using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers.Admin
{
    public class ManageStudentAdminController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: ManageStudentAdmin
        public ActionResult Index()
        {
            var students = db.STUDENTs.ToList();
            return View(students);
        }

        public ActionResult AddnewStudents()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddnewStudents(STUDENT student, USER user)
        {
            if (ModelState.IsValid)
            {
                db.STUDENTs.Add(student);
                db.SaveChanges();

                user.STUDENT_ID = student.ID; // Associate the user with the newly added student
                user.Role = "student";
                db.USERS.Add(user);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult ViewStudents(int id)
        {
            var student = db.STUDENTs.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult EditStudentsAccount(int id)
        {
            var student = db.STUDENTs.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudentsAccount(STUDENT student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult DeactivateStudentsAccount(int id)
        {
            var student = db.STUDENTs.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmed(int id)
        {
            var user = db.USERS.Where(u => u.STUDENT_ID == id).FirstOrDefault();
            var student = db.STUDENTs.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            var student_course = db.STUDENT_COURSE.Where(u => u.ID_STUDENT == student.ID).FirstOrDefault();
            db.USERS.Remove(user);
            if(student_course != null)
            {
                db.STUDENT_COURSE.Remove(student_course);
            }
            db.STUDENTs.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}