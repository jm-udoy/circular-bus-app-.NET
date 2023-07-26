using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Circular_Bus_App.Auth;
using Circular_Bus_App.Models.Database;

namespace Circular_Bus_App.Controllers
{
    public class SupervisorController : Controller
    {
        // GET: Supervisor
        public ActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from s in db.Users
                        where s.U_Id == id
                        select s).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(User super)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                var data = (from s in db.Users
                            where s.U_Id == super.U_Id
                            select s).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(super);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Supervisor");

            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from s in db.Users
                        where s.U_Id == id
                        select s).FirstOrDefault();

            db.Users.Remove(data);
            db.SaveChanges();
            return RedirectToAction("LoginUser", "User");
        }

        [SupervisorAccess]
        [HttpGet]
        public ActionResult Dashboard(User super)
        {
            string loggedName = Session["username"].ToString();
            string id = Session["userid"].ToString();
            int loggedId = Int32.Parse(id);
            CircularBusEntities db = new CircularBusEntities(); ;

            var data = (from s in db.Users
                        where s.U_Id.Equals(loggedId) &&
                        s.U_UserName.Equals(loggedName)
                        select s).ToList();
            return View(data);
        }

        [SupervisorAccess]
        [HttpGet]
        public ActionResult AssignedBuses(User super)
        {
            /*int id = (int)Session["userid"];*/

            string id = Session["userid"].ToString();
            int loggedId = Int32.Parse(id);

            CircularBusEntities db = new CircularBusEntities(); ;

            var data = (from s in db.BusInfoes
                        where s.B_SId == loggedId
                        select s).ToList();
            return View(data);
        }

   

        [HttpGet]
        public ActionResult EditSeat(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from b in db.BusInfoes
                        where b.B_Id == id
                        select b).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult EditSeat(BusInfo info)
        {
            CircularBusEntities db = new CircularBusEntities();

            var data = (from b in db.BusInfoes
                        where b.B_Id == info.B_Id
                        select b).FirstOrDefault();
            db.Entry(data).CurrentValues.SetValues(info);
            db.SaveChanges();
            return RedirectToAction("AssignedBuses", "Supervisor");

        }
    }
}