using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers.Instructor
{
    public class PersonalInstructorController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: PersonalInstructor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyNotes(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var courses = db.INSTRUCTOR_COURSE.Where(u => u.ID_INSTRUCTOR == instructorId).ToList();
            return View(courses);
        }

        public ActionResult AddNote(int id)
        {
            var instructorCourse = db.INSTRUCTOR_COURSE.Find(id);
            if (instructorCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorCourse = instructorCourse;
            return View(instructorCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNote(INSTRUCTOR_COURSE instructorCourse)
        {
            db.Entry(instructorCourse).State = EntityState.Modified;
            var course = db.COURSEs.FirstOrDefault(m => m.ID == instructorCourse.ID_COURSE);
            var student_courses = course.STUDENT_COURSE.ToList();
            var note = new NOTE
            {
                NOTES = instructorCourse.INSTRUCTOR_NOTE,
                TIME = DateTime.Now,
                ID_INSTRUCTOR = instructorCourse.ID_INSTRUCTOR ?? 0,
                INSTRUCTOR = db.INSTRUCTORs.FirstOrDefault(i => i.ID ==  instructorCourse.ID_INSTRUCTOR)
            };
            foreach (var courseItem in student_courses)
            {
                var student_Id = courseItem.ID_STUDENT;
                var student = db.STUDENTs.FirstOrDefault(u => u.ID == student_Id);
                note.ID_STUDENT = student_Id ?? 0;
                note.STUDENT = student;
                db.NOTEs.Add(note);
                db.SaveChanges();
            }
            
            db.SaveChanges();
            return RedirectToAction("Personal", "Home");
        }

        public ActionResult UpdateMyInfomation(int id)
        {
            var instructorID = db.USERS.Find(id).INSTRUCTOR_ID;
            var instructor = db.INSTRUCTORs.Find(instructorID);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMyInfomation(INSTRUCTOR updatedInstructor)
        {
            if (ModelState.IsValid)
            {
                // Update instructor properties (except password)
                var instructorID = db.USERS.Find(updatedInstructor.ID).INSTRUCTOR_ID;
                var instructor = db.INSTRUCTORs.Find(instructorID);
                if (instructor != null)
                {
                    instructor.FULL_NAME = updatedInstructor.FULL_NAME;
                    instructor.INSTRUCTOR_NUMBER = updatedInstructor.INSTRUCTOR_NUMBER;
                    instructor.EMAIL = updatedInstructor.EMAIL;

                    db.SaveChanges();
                    return RedirectToAction("Personal", "Home");
                }
            }

            return View(updatedInstructor);
        }

        public ActionResult UpdatePassword(int id)
        {
            var instructorID = db.USERS.Find(id).INSTRUCTOR_ID;
            var instructor = db.INSTRUCTORs.Find(instructorID);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(int id,string newPassword)
        {
            if (ModelState.IsValid)
            {
                // Update instructor properties (except password)
                var user = db.USERS.Find(id);
                if (user != null)
                {
                    user.Password = newPassword;
                    db.SaveChanges();
                    return RedirectToAction("Personal", "Home");
                }
            }

            return View();
        }

       public ActionResult Message(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var messages = db.MESSAGEs.Where(u => u.ID_INSTRUCTOR == instructorId).ToList();
            return View(messages);
        }

        public ActionResult ReplyMessage(int id)
        {
            var message = db.MESSAGEs.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.InstructorCourse = message;
            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyMessage(MESSAGE mesage)
        {
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("dd/M/yyyy h:mm tt");
            DateTime parsedTime = DateTime.ParseExact(formattedTime, "dd/M/yyyy h:mm tt", null);
            mesage.TIME = parsedTime;
            db.Entry(mesage).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Personal", "Home");
        }
    }
}