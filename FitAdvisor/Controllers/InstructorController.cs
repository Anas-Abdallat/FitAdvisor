using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class InstructorController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: Instructor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchForInstructor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchForInstructor(int instructorId)
        {
            var instructor = db.INSTRUCTORs.SingleOrDefault(s => s.ID == instructorId);

            if (instructor == null)
            {
                ViewBag.ErrorMessage = "Instructor not found.";
                return View("SearchForInstructor");
            }

            return View("SearchForInstructor", instructor);
        }

        public ActionResult Message(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var message = db.MESSAGEs.Where(u => u.ID_STUDENT == studentId).ToList();

            ViewBag.id = id;
            return View(message);
        }

        public ActionResult SendMessage(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var student = db.STUDENTs.Find(studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            // Retrieve instructors to populate a dropdown list in the view
            var instructors = db.INSTRUCTORs.ToList();

            ViewBag.Instructors = new SelectList(instructors, "ID", "FULL_NAME");

            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(int id, int selectedInstructorId, string messageText)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var student = db.STUDENTs.Find(studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            // Retrieve the selected instructor
            var instructor = db.INSTRUCTORs.Find(selectedInstructorId);

            if (instructor == null)
            {
                return HttpNotFound();
            }

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("dd/M/yyyy h:mm tt");
            DateTime parsedTime = DateTime.ParseExact(formattedTime, "dd/M/yyyy h:mm tt", null);
            // Create a new message
            var message = new MESSAGE
            {
                ID_STUDENT = student.ID,
                ID_INSTRUCTOR = instructor.ID,
                MESSAGE1 = messageText,
                TIME = parsedTime
            };

            db.MESSAGEs.Add(message);
            db.SaveChanges();

            return RedirectToAction("Instructor","Home"); // Redirect to the appropriate action
        }


        public ActionResult InstructionSchedual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DisplaySchedule(int instructorId)
        {
            var instructor = db.INSTRUCTORs
        .Include("INSTRUCTOR_COURSE.COURSE")
        .SingleOrDefault(s => s.ID == instructorId);

            if (instructor == null)
            {
                ViewBag.ErrorMessage = "The Instructor have no Courses Yet!!!";

                return View("InstructionSchedual");
            }
            
            return View("InstructionSchedual", instructor.INSTRUCTOR_COURSE);
        }   

    }
}