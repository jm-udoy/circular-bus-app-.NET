using Circular_Bus_App.Auth;
using Circular_Bus_App.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Circular_Bus_App.Controllers
{
    [Authorize]
    [BusOwnerAccess]
    public class BusOwnerController : Controller
    {
        // GET: BusOwner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BusOwnerProfile()
        {
            string loggedUserName = Session["username"].ToString();
            string id = Session["userid"].ToString();
            int loggedUserId = Int32.Parse(id);
            CircularBusEntities db = new CircularBusEntities();

            var data = (from e in db.Users
                        where e.U_Id.Equals(loggedUserId) &&
                        e.U_UserName.Equals(loggedUserName)
                        select e).ToList();
            return View(data);
        }

        public ActionResult Buses()
        {
            CircularBusEntities db = new CircularBusEntities();
            int s = (int)Session["userid"];
            var data = (from u in db.BusInfoes
                        where u.B_OwnedBy == s && u.B_Status == "Active"
                        select u).ToList();

            return View(data);
        }

        [HttpGet]
        public ActionResult BusOwnerEdit(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                        where u.U_Id == id
                        select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult BusOwnerEdit(User new_User)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                var data = (from u in db.Users
                            where u.U_Id == new_User.U_Id
                            select u).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(new_User);
                db.SaveChanges();
                return RedirectToAction("Index", "BusOwner");

            }
            return View();
        }

        [HttpGet]
        public ActionResult BusOwnerDelete(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                        where u.U_Id == id
                        select u).FirstOrDefault();
            db.Users.Remove(data);
            db.SaveChanges();
            return RedirectToAction("LoginUser", "User");
        }
        public ActionResult BusEdit(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusInfoes
                        where u.B_Id == id
                        select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult BusEdit(BusInfo b)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                var data = (from u in db.BusInfoes
                            where u.B_Id == b.B_Id
                            select u).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(b);
                db.SaveChanges();
                return RedirectToAction("Buses", "BusOwner");

            }
            return View();
        }
        [HttpGet]
        public ActionResult BusDelete(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusInfoes
                        where u.B_Id == id
                        select u).FirstOrDefault();
            db.BusInfoes.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Buses", "BusOwner");
        }



        public ActionResult LogoutUser()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser", "User");
        }
        public ActionResult AddBus()
        {
            return View(new BusInfo());
        }
        [HttpPost]
        public ActionResult AddBus(BusInfo u)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                db.BusInfoes.Add(u);
                db.SaveChanges();
                return RedirectToAction("Buses", "BusOwner");
            }
            return View(u);
        }

        public ActionResult ViewSell()
        {
            CircularBusEntities db = new CircularBusEntities();
            int s = (int)Session["userid"];

            var sum_price = (from sum in db.Carts
                             join b in db.BusInfoes on sum.B_Id equals b.B_Id
                             where sum.B_Id == b.B_Id && b.B_OwnedBy == s
                             select sum).Sum(S => S.BS_Fair).ToString();
            ViewBag.SellPrice = sum_price;
            var tbl = (from sum in db.Carts

                       where sum.BusInfo.B_OwnedBy == s
                       select sum).ToList();

            return View(tbl);
        }

        public ActionResult BusRoute(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusInfoes
                        where u.B_Id == id
                        select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult BusRoute(BusRoute b)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                db.BusRoutes.Add(b);
                db.SaveChanges();
                return RedirectToAction("Buses", "BusOwner");

            }
            return View();
        }
        public ActionResult BusRouteView(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusRoutes
                        where u.BR_BId == id
                        select u).ToList();
            return View(data);
        }

        public ActionResult BusRouteEdit(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusRoutes
                        where u.BR_Id == id
                        select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult BusRouteEdit(BusRoute b)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                var data = (from u in db.BusRoutes
                            where u.BR_Id == b.BR_Id
                            select u).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(b);
                db.SaveChanges();
                return RedirectToAction("Buses", "BusOwner");

            }
            return View();
        }

        [HttpGet]
        public ActionResult BusRouteDelete(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.BusRoutes
                        where u.BR_Id == id
                        select u).FirstOrDefault();
            db.BusRoutes.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Buses", "BusOwner");
        }

    }
}