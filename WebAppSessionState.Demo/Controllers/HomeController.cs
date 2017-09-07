using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAppSessionState.Demo.Models;
using Microsoft.AspNetCore.Http;
using WebAppSessionState.Demo.Extensions;
using WebAppSessionState.Demo.Middleware;

namespace WebAppSessionState.Demo.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyName = "_Name";
        const string SessionKeyYearsMember = "_YearsMember";
        const string SessionKeyDate = "_Date";
        public IActionResult Index()
        {
            HttpContext.Session.SetString(SessionKeyName, "Rick");
            HttpContext.Session.SetInt32(SessionKeyYearsMember,3);
            return RedirectToAction("SessionNameYears");
        }

        public IActionResult SessionNameYears()
        {
            var name = HttpContext.Session.GetString(SessionKeyName);
            var yearsMember = HttpContext.Session.GetInt32(SessionKeyYearsMember);
            var value = HttpContext.Items[SampleMiddleware.SampleKey];
            return Content($"Name: \"{name}\",  Membership years: \"{yearsMember}\" ,SampleKey:\"{value}\"");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SetDate()
        {
            HttpContext.Session.Set(SessionKeyDate, DateTime.Now);
            return RedirectToAction("GetDate");
        }

        public IActionResult GetDate()
        {
            var date = HttpContext.Session.Get<DateTime>(SessionKeyDate);
            var sessionTime = date.TimeOfDay.ToString();
            var currentTime = DateTime.Now.TimeOfDay.ToString();

            return Content($"Current time: {currentTime} - "
                 + $"session time: {sessionTime}");
        }
    }
}
