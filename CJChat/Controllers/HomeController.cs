using CJChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(!String.IsNullOrWhiteSpace(Session["UserName"] as String))
            {
                return new RedirectResult("~/Home/Chat");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Identify(String userName, bool remember)
        {
            Session["UserName"] = userName;

            if(remember)
            {
                Response.Cookies.Add(new HttpCookie("NickerBoxUser", userName)
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                });
            }

            return new RedirectResult("~/Home/Chat");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chat()
        {
            if (String.IsNullOrWhiteSpace(Session["UserName"] as String))
            {
                return new RedirectResult("~/");
            }

            ViewBag.Title = "NickerBox Chat.";

            return View();
        }
    }
}