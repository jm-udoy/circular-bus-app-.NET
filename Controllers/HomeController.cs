using Circular_Bus_App.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Circular_Bus_App.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = db.BusRoutes.ToList();
            return View(data);
            
        }

        [HttpGet]
        public ActionResult Login()
        {    
            return View();
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
    }
}