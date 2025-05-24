using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class AcdemicController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();
        // GET: Acdemic
        public ActionResult Index()
        {
            var courses = db.STUDENT_COURSE.ToList();
            return View(courses);
        }

        public ActionResult MySchedual(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var courses = db.STUDENT_COURSE.Where(u => u.ID_STUDENT == studentId).ToList();
            return View(courses);
        }

        public ActionResult AdvisorNotes(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var note = db.NOTEs.Where(u => u.ID_STUDENT == studentId).ToList();
            return View(note);
        }


        public ActionResult MajorPlan()
        {
            return View();
        }


        public ActionResult HomeWork(int id)
        {
            var studentId = db.USERS.Find(id).STUDENT_ID;
            var homeworks = db.HOMEWORK.Where(u => u.ID_STUDENT == studentId).ToList();
            return View(homeworks);
        }

        public ActionResult Annoucment()
        {
            return View();
        }

    }
}