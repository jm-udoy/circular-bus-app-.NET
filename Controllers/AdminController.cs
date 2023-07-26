using Circular_Bus_App.Models.Database;
using Circular_Bus_App.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;

namespace Circular_Bus_App.Controllers
{

    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            CircularBusEntities db = new CircularBusEntities();
            var a_name = (from d in db.Admins where d.A_Id == 1 select d).FirstOrDefault();
            return View(a_name);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from e in db.Admins
                        where e.A_Password.Equals(admin.A_Password) &&
                        e.A_UserName.Equals(admin.A_UserName)
                        select e).FirstOrDefault();

            if (data != null)
            {
                FormsAuthentication.SetAuthCookie(data.A_UserName, false);
                return RedirectToAction("Index");
            }
            TempData["msg"] = "Invalid Credentials! Try again...";
            return RedirectToAction("Login");

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult UserList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.Users.ToList());
        }

        [Authorize]
        public ActionResult User(User obj)
        {

            return View("AddUser", obj);

        }
        [Authorize]
        public ActionResult CreateUser(User obj)
        {

            return View("AddUser");

        }


        [Authorize]
        [HttpPost]
        public ActionResult AddUser(User user)
        {

                CircularBusEntities db = new CircularBusEntities();
                User obj = new User();
                obj.U_Id = user.U_Id;
                obj.U_UserName = user.U_UserName;
                obj.U_Password = user.U_Password;
                obj.U_Phone = user.U_Phone;
                obj.U_Email = user.U_Email;
                obj.U_Address = user.U_Address;
                obj.U_Gender = user.U_Gender;
                obj.U_DateofBirth = user.U_DateofBirth;
                obj.U_Role = user.U_Role;
                obj.U_Status = user.U_Status;

                if (user.U_Id == 0)
                {
                    db.Users.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
            
            

            return View("AddUser");
        }

        [Authorize]
        public ActionResult DeleteUser(int u_id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var res = db.Users.Where(x => x.U_Id == u_id).First();
            db.Users.Remove(res);
            db.SaveChanges();

            var list = db.Users.ToList();

            return View("UserList", list);
        }

        [Authorize]
        public ActionResult BusInfoList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.BusInfoes.ToList());
        }

        [Authorize]
        public ActionResult BusOwnerList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.BusOwners.ToList());
        }

        [Authorize]
        public ActionResult BusRouteList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.BusRoutes.ToList());
        }

        [Authorize]
        public ActionResult SoldTicketList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.SoldTickets.ToList());
        }

        [Authorize]
        public ActionResult SupervisorList()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.Supervisors.ToList());
        }


    }
}