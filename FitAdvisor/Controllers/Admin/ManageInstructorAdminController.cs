using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers.Admin
{
    public class ManageInstructorAdminController : Controller
    {
        private FitAdvisorDBEntities1 db = new FitAdvisorDBEntities1();

        // GET: AcdemicAdmin
        public ActionResult Index()
        {
            var instructors = db.INSTRUCTORs.ToList();
            return View(instructors);
        }

        public ActionResult AddNewInstructor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewInstructor(INSTRUCTOR instructor)
        {
            if (ModelState.IsValid)
            {
                db.INSTRUCTORs.Add(instructor);
                db.SaveChanges();
                //ViewBag.InstructorId = instructor.ID;
                return RedirectToAction("AddNewUser", new { instructorId = instructor.ID });
            }
            return View(instructor);
        }

        public ActionResult AddNewUser(int instructorId)
        {
            ViewBag.InstructorId = instructorId;
            return View(new USER { INSTRUCTOR_ID = instructorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewUser(USER user)
        {
            if (ModelState.IsValid)
            {
                var instructor = db.INSTRUCTORs.Find(user.INSTRUCTOR_ID);
                user.INSTRUCTOR = instructor;
                user.Role = "instructor";
                db.USERS.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }


        public ActionResult ViewInstructor(int id)
        {
            var instructor = db.INSTRUCTORs.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        public ActionResult EditInstructorAccount(int id)
        {
            var instructor = db.INSTRUCTORs.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInstructorAccount(INSTRUCTOR instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        public ActionResult DeactivateInstructorAccount(int id)
        {
            var instructor = db.INSTRUCTORs.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmed(int id)
        {
            var user = db.USERS.Where(u => u.INSTRUCTOR_ID == id).FirstOrDefault();
            var instructor = db.INSTRUCTORs.Find(id);
            var note = db.NOTEs.Where(u => u.ID_INSTRUCTOR == instructor.ID).ToList();
            var message = db.MESSAGEs.Where(u => u.ID_INSTRUCTOR == instructor.ID).ToList();
            var community_Services = db.COMMUNITY_SERVICE.Where(u => u.ID_INSTRUCTOR == instructor.ID).ToList();
            if (instructor == null)
            {
                return HttpNotFound();
            }
            if (note != null)
            {
                foreach (var item in note)
                {
                    db.NOTEs.Remove(item);
                    db.SaveChanges();
                }
                
            }
            if(user != null)
            {
                db.USERS.Remove(user);
                db.SaveChanges();
            }
            if (message != null)
            {
                foreach (var item in message)
                {
                    db.MESSAGEs.Remove(item);
                    db.SaveChanges();
                }
            }
            if (community_Services != null)
            {
                foreach (var item in community_Services)
                {
                    db.COMMUNITY_SERVICE.Remove(item);
                    db.SaveChanges();
                }
            }
            db.INSTRUCTORs.Remove(instructor);
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