using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers.Instructor
{
    public class RegistraionInstructorController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: RegistraionInstructor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentSchedule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DisplaySchedule(int studentId)
        {
            var student = db.STUDENTs
        .Include("STUDENT_COURSE.COURSE")
        .SingleOrDefault(s => s.ID == studentId);

            if (student == null)
            {
                ViewBag.ErrorMessage = "The Student have no Courses Yet!!!";
                return View("StudentSchedule");
            }

            return View("StudentSchedule", student.STUDENT_COURSE);
        }

        public ActionResult SemesterSchedule()
        {
            return View();
        }

        public ActionResult ManageClassAttendance(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var instructorCourse = db.INSTRUCTOR_COURSE.Where(u => u.ID_INSTRUCTOR == instructorId);
            var courses = db.STUDENT_COURSE.ToList();
            List<STUDENT_COURSE> sTUDENT_COURSEs = new List<STUDENT_COURSE>();
            foreach (var instruccourse in instructorCourse)
            {
                foreach (var course in courses)
                {
                    if (instruccourse.ID_COURSE == course.ID_COURSE)
                    {
                        sTUDENT_COURSEs.Add(course);
                    }
                }
            }
            return View(sTUDENT_COURSEs);
        }

        public ActionResult FindStudentById()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindStudentByIdTable(int studentId)
        {
            var student = db.STUDENTs.SingleOrDefault(s => s.ID == studentId);

            if (student == null)
            {
                ViewBag.ErrorMessage = "Student not found.";
                return View("FindStudentById");
            }

            return View("FindStudentById", student);
        }

        public ActionResult FindACourseByName()
        {
            return View();
        }

        public ActionResult FindACourseByNameTable(int courseId)
        {
            var courses = db.COURSEs.SingleOrDefault(s => s.ID == courseId);

            if (courses == null)
            {
                ViewBag.ErrorMessage = "Course not found.";
                return View("FindACourseByName");
            }

            return View("FindACourseByName", courses);
        }

    }
}