using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class AcdemicInstructorController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: Acdemic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MySchedual(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var courses = db.INSTRUCTOR_COURSE.Where(u => u.ID_INSTRUCTOR == instructorId).ToList();
            return View(courses);
        }

        public ActionResult StudentsMarks(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var instructorCourse = db.INSTRUCTOR_COURSE.Where(u => u.ID_INSTRUCTOR == instructorId);
            var courses = db.STUDENT_COURSE.ToList();
            List<STUDENT_COURSE> sTUDENT_COURSEs = new List<STUDENT_COURSE>();
            foreach(var instruccourse in instructorCourse)
            {
                foreach(var course in courses)
                {
                    if(instruccourse.ID_COURSE == course.ID_COURSE)
                    {
                        sTUDENT_COURSEs.Add(course);
                    }
                }
            }
            return View(sTUDENT_COURSEs);
        }


        public ActionResult AddMark(int id)
        {
            var studuentCourse = db.STUDENT_COURSE.Find(id);
            if (studuentCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentCourse = studuentCourse;
            return View(studuentCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMark(STUDENT_COURSE studuentCourse)
        {
                db.Entry(studuentCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Academic", "Home");
        }

        public ActionResult ManageHomeWork(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var instructor = db.INSTRUCTORs.FirstOrDefault(i => i.ID == instructorId);
            if (instructor == null)
            {
                return HttpNotFound();
            }

            var instructorCourses = instructor.INSTRUCTOR_COURSE.Select(ic => ic.COURSE).ToList();
            var students = instructorCourses.SelectMany(course => course.STUDENT_COURSE.Select(sc => sc.STUDENT)).ToList();

            ViewBag.Courses = instructorCourses;
            ViewBag.Students = students;
            ViewBag.InstructorId = instructorId;

            return View();
        }

        [HttpPost]
        public ActionResult ManageHomeWork(int id, int courseId, List<int> studentIds, string assignment)
        {
            var homework = new HOMEWORK
            {
                ASSIGNMENT = assignment,
                ID_INSTRUCTOR = id,
                ID_COURSE = courseId,
                COURSE = db.COURSEs.FirstOrDefault(u => u.ID == courseId),
                INSTRUCTOR = db.INSTRUCTORs.FirstOrDefault(i => i.ID == id)
            };

            foreach (var studentId in studentIds)
            {
                var student = db.STUDENTs.FirstOrDefault(s => s.ID == studentId);
                if (student != null)
                {
                    homework.ID_STUDENT = studentId;
                    homework.STUDENT = student;
                    db.HOMEWORK.Add(homework);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Academic", "Home");
        }

        public ActionResult AssignNotesForStudents()
        {
            return View();
        }

        public ActionResult ViewStudentsAttendance(int id)
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

        public ActionResult AddAttendance(int id)
        {
            var studuentCourse = db.STUDENT_COURSE.Find(id);
            if (studuentCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentCourse = studuentCourse;
            return View(studuentCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAttendance(STUDENT_COURSE studuentCourse)
        {
            db.Entry(studuentCourse).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Academic", "Home");
        }

        public ActionResult ViewAnnouncements()
        {
            return View();
        }

        public ActionResult ViewStudentsCommunityServices(int id)
        {
            var instructorId = db.USERS.Find(id).INSTRUCTOR.ID;
            var instructor = db.INSTRUCTORs.FirstOrDefault(i => i.ID == instructorId);
            if (instructor == null)
            {
                // Handle error here
                return HttpNotFound();
            }

            var students = db.STUDENTs.ToList();
            var communityServices = db.COMMUNITY_SERVICE.ToList();

            ViewBag.InstructorId = id;
            ViewBag.Students = students;
            ViewBag.CommunityServices = communityServices;

            return View();
        }

        [HttpPost]
        public ActionResult ViewStudentsCommunityServices(int instructorId, List<int> studentIds, int hoursPassed, string activity)
        {
            Random random = new Random();
            var instructor_id = db.USERS.Where(u => u.Id == instructorId).FirstOrDefault().INSTRUCTOR_ID;
            foreach (var studentId in studentIds)
            {
                var communityService = new COMMUNITY_SERVICE
                {
                    Id = random.Next(1, 100),
                    ID_INSTRUCTOR = instructorId,
                    ID_STUDENT = studentId,
                    HOURS_PASSED = hoursPassed,
                    ACTIVITY = activity,
                    INSTRUCTOR = db.INSTRUCTORs.Where(u => u.ID == instructor_id).FirstOrDefault(),
                    STUDENT = db.STUDENTs.FirstOrDefault(u => u.ID == studentId)
                };

                db.COMMUNITY_SERVICE.Add(communityService);
                db.SaveChanges();
            }

            return RedirectToAction("ViewStudentsCommunityServices", new { id = instructorId });
        }


    }
}