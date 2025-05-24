using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class HomeController : Controller
    {
        private static string _Role;
        private static int? _ID;
        // GET: Home
        public ActionResult Index(string role,int? userId)
        {
            try
            {
                if (userId != null)
                {
                    _Role = role;
                    _ID = userId;
                    ViewBag.UserRole = role;
                }
                
                return View();
            }catch (Exception )
            {
                return View();
            }
        }

        public ActionResult Academic()
        {
            ViewBag.UserId = _ID;
            ViewBag.UserRole = _Role;
            return View();
        }
        public ActionResult GetUserRole()
        {
            return Content(_Role);
        }

        public ActionResult Registraion()
        {
            ViewBag.UserId = _ID;
            ViewBag.UserRole = _Role;
            return View();
        }


        public ActionResult Personal()
        {
            ViewBag.UserId = _ID;
            ViewBag.UserRole = _Role;
            return View();
        }

        public ActionResult Instructor()
        {
            ViewBag.UserId = _ID;
            ViewBag.UserRole = _Role;
            return View();
        }

        public ActionResult ManageInstructor()
        {
            ViewBag.UserRole = _Role;
            return View();
        }

        public ActionResult ManageStudent()
        {
            ViewBag.UserRole = _Role;
            return View();
        }

        public ActionResult ManageCourses()
        {
            ViewBag.UserRole = _Role;
            ViewBag.UserId = _ID;
            return View();
        }

        public ActionResult LogOut()
        {
            return RedirectToAction("Index", "LogIn");
        }

    }
}