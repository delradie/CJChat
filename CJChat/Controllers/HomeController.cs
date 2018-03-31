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
        private const String DefaultTimeZone = "US/Eastern";

        public ActionResult Index()
        {
            String SelectedTimeZone = String.Empty;

            if (Request.Cookies["TimeZoneName"] != null
                && !String.IsNullOrEmpty(Request.Cookies["TimeZoneName"].Value))
            { 
                SelectedTimeZone = Request.Cookies["TimeZoneName"].Value;
            }

            if (!String.IsNullOrWhiteSpace(Session["UserName"] as String)
                && !String.IsNullOrWhiteSpace(SelectedTimeZone))
            {
                return new RedirectResult("~/Home/Chat");
            }

            SelectedTimeZone = DefaultTimeZone;

            IEnumerable<SelectListItem> TimeZones = TZConvert.KnownIanaTimeZoneNames.OrderBy(t => t).Select(t =>
    new SelectListItem
    {
        Text = t,
        Value = t,
        Selected = String.Equals(t.Trim(), SelectedTimeZone, StringComparison.InvariantCultureIgnoreCase)
    });

            ViewBag.TimeZones = TimeZones;
            ViewBag.DefaultTimeZone = SelectedTimeZone;
            return View();
        }

        [HttpPost]
        public ActionResult Identify(SignOn input)
        {
            Session["UserName"] = input.UserName;

            Response.Cookies.Add(new HttpCookie("TimeZoneName", input.Location)
            {
                Expires = DateTime.UtcNow.AddYears(1)
            });

            if (input.Remember)
            {
                Response.Cookies.Add(new HttpCookie("NickerBoxUser", input.UserName)
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

            if (Request.Cookies["TimeZoneName"] != null)
            {
                SelectedTimeZone = Request.Cookies["TimeZoneName"].Value;
            }
            else
            {
                return new RedirectResult("~/");
            }

            ViewBag.SelectedTimeZoneId = SelectedTimeZone;


            ViewBag.Title = "NickerBox Chat.";

            return View();
        }
    }
}