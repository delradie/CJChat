using CJChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeZoneConverter;

namespace CJChat.Controllers
{
    public class HomeController : Controller
    {
        private const String DefaultTimeZone = "US Eastern Standard Time";

        public ActionResult Index()
        {
            if (!String.IsNullOrWhiteSpace(Session["UserName"] as String))
            {
                return new RedirectResult("~/Home/Chat");
            }

            String SelectedTimeZone = DefaultTimeZone;

            if (Request.Cookies["TimeZone"] != null)
            {
                SelectedTimeZone = Request.Cookies["TimeZone"].Value;
            }

            ViewBag.DefaultTimeZone = SelectedTimeZone;
            return View();
        }

        [HttpPost]
        public ActionResult Identify(String userName, String timezone, bool remember)
        {
            Session["UserName"] = userName;

            Response.Cookies.Add(new HttpCookie("TimeZone", timezone)
            {
                Expires = DateTime.UtcNow.AddYears(1)
            });

            if (remember)
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

        public ActionResult SignOut()
        {
            Response.Cookies.Remove("NickerBoxUser");
            Session["UserName"] = null;

            return new RedirectResult("~/");
        }

        public ActionResult Chat()
        {
            if (String.IsNullOrWhiteSpace(Session["UserName"] as String))
            {
                return new RedirectResult("~/");
            }

            String SelectedTimeZone = DefaultTimeZone;

            if (Request.Cookies["TimeZone"] != null)
            {
                SelectedTimeZone = Request.Cookies["TimeZone"].Value;
            }

            ViewBag.SelectedTimeZoneName = SelectedTimeZone;
            ViewBag.SelectedTimeZoneId = TZConvert.WindowsToIana(SelectedTimeZone);

            TimeZoneInfo Info = TimeZoneInfo.FindSystemTimeZoneById(SelectedTimeZone);
            ViewBag.SelectedTimeZone = Info;
            ViewBag.TimeZoneAdjustment = Info.BaseUtcOffset.Hours;

            ViewBag.Title = "NickerBox Chat.";
           
            return View();
        }
    }
}