using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class PersonalController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: Personal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyAttendance(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var student = db.STUDENTs.Where(u => u.ID == studentId);

            if (student == null)
            {
                return HttpNotFound();
            }

            // Retrieve the attendance records for the student
            List<STUDENT_COURSE> attendanceRecords = new List<STUDENT_COURSE>();
            foreach (var record in student)
            {
                foreach(var c in record.STUDENT_COURSE)
                {
                    attendanceRecords.Add(c);
                }
            }
            return View(attendanceRecords);
        }

        public ActionResult StudentInformation(int id)
        {
            var studentID = db.USERS.Find(id).STUDENT_ID;
            var student = db.STUDENTs.Find(studentID);

            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        public ActionResult ExamSchedual()
        {
            return View();
        }

        public ActionResult UpdateMyinformation(int id)
        {
            var studentID = db.USERS.Find(id).STUDENT_ID;
            var student = db.STUDENTs.Find(studentID);

            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMyinformation(STUDENT updatedStudent)
        {
            if (ModelState.IsValid)
            {
                // Update instructor properties (except password)
                var studentID = db.USERS.Find(updatedStudent.ID).STUDENT_ID;
                var student = db.STUDENTs.Find(studentID);
                if (student != null)
                {
                    student.FULL_NAME = updatedStudent.FULL_NAME;
                    student.STUDENT_NUMBER = updatedStudent.STUDENT_NUMBER;
                    student.EMAIL = updatedStudent.EMAIL;
                    student.DEPARTMENT = updatedStudent.DEPARTMENT;

                    db.SaveChanges();
                    return RedirectToAction("Personal", "Home");
                }
            }

            return View(updatedStudent);
        }

        public ActionResult UpdatePassword(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(int id, string newPassword)
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

        public ActionResult CommunityServices(int id)
        {
            var studentID = db.USERS.Find(id).STUDENT_ID;
            var communities = db.COMMUNITY_SERVICE.Where(u => u.ID_STUDENT == studentID).ToList();
            
            return View(communities);
        }

    }
}