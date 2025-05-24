using FitAdvisor.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitAdvisor.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            using (var db = new FitAdvisorDBEntities1()) 
            {
                var user = db.USERS.FirstOrDefault(u => u.UserName == userName && u.Password == password);
                if (user != null)
                {
                    // Store role information in a session for later use
                    Session["UserRole"] = user.Role;
                    HttpCookie cookie = new HttpCookie("UsernameCookie", user.UserName);
                    Response.Cookies.Add(cookie);
                    HttpCookie idCookie = new HttpCookie("UserIDCookie", user.Id.ToString());
                    Response.Cookies.Add(idCookie);
                    return RedirectToAction("Index", "Home", new { role = user.Role,userId = user.Id });
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid login credentials.";
                }
            }

            return View();
        }

    }
}