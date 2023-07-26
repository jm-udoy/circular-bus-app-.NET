using Circular_Bus_App.Models.Database;
using Circular_Bus_App.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Circular_Bus_App.Controllers
{
    public class UserController : Controller

    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginUser()
        {
            //if (User.Identity.IsAuthenticated) { return RedirectToAction("Index"); }
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(User user)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                        where u.U_Password.Equals(user.U_Password) &&
                        u.U_UserName.Equals(user.U_UserName)
                        select u).FirstOrDefault(); ;
            if (data != null)
            {               
                FormsAuthentication.SetAuthCookie(data.U_UserName, true);

                Session["role"] = data.U_Role;
                Session["userid"] = data.U_Id;
                Session["username"] = data.U_UserName;

                if (Session["role"].ToString() == "User      ")
                {
                    return RedirectToAction("Index", "Home");
                }
                //SJ
                else if (Session["role"].ToString() == "Supervisor")
                {
                    return RedirectToAction("Welcome", "Supervisor");
                }
                //RG
                else if (data.U_Role == "BusOwner  ")
                {
                    return RedirectToAction("Index", "BusOwner");

                }
                else if(data.U_Role == "Admin     ")
                {
                    return RedirectToAction("Login", "Admin");

                }


            }

            TempData["msg"] = "Invalid Username or Password";
            return RedirectToAction("LoginUser");
        }

        public ActionResult LogoutUser()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult Create(User u)
        {
            
            if(ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();

                if (u.U_Role == "Supervisor")
                {

                    User us = new User();
                    us.U_UserName = u.U_UserName;
                    us.U_Password = u.U_Password;
                    us.U_Phone = u.U_Phone;
                    us.U_Email = u.U_Email;
                    us.U_Address = u.U_Address;
                    us.U_Gender = u.U_Gender;
                    us.U_DateofBirth = u.U_DateofBirth;
                    us.U_Role = u.U_Role;
                    us.U_Status = "Pending";

                    db.Users.Add(us);
                    db.SaveChanges();
                    return RedirectToAction("LoginUser");

                }
            
                
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("LoginUser");
            }
            return View(u);

        }

        [UserAccess]
        [HttpGet]
        public ActionResult BusDetails()
        {
            CircularBusEntities db = new CircularBusEntities();
            return View(db.BusInfoes.ToList());

        }

        [HttpGet]
        [Authorize]
        [UserAccess]

        public ActionResult Profile(User user)
        {
            string loggedUserName = Session["username"].ToString();
            string id = Session["userid"].ToString(); 
            int loggedUserId = Int32.Parse(id);
            CircularBusEntities db = new CircularBusEntities();
            
;            var data = (from e in db.Users
                        where e.U_Id.Equals(loggedUserId) &&
                        e.U_UserName.Equals(loggedUserName)
                        select e).ToList();
            return View(data);
        }

        [UserAccess]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                        where u.U_Id == id
                        select u).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(User new_User)
        {
            if (ModelState.IsValid)
            {
                CircularBusEntities db = new CircularBusEntities();
                var data = (from u in db.Users
                            where u.U_Id == new_User.U_Id
                            select u).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(new_User);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return View(new_User);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                         where u.U_Id == id
                         select u).FirstOrDefault();
            return View(data);
        }
        [HttpPost]

        public ActionResult Delete(User delete)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from u in db.Users
                        where u.U_Id == delete.U_Id
                        select u).FirstOrDefault();
            db.Entry(data).CurrentValues.SetValues(delete);
            db.Users.Remove(data);
            db.SaveChanges();
            return RedirectToAction("LoginUser");

        }

        [HttpGet]
        public ActionResult Purchase(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from b in db.BusRoutes
                        where b.BR_BId == id
                        select b).FirstOrDefault();
            return View(data);
            
        }

        [HttpGet]

        public ActionResult Cart(string data1, int data2, int data3, int data4)
        {
            CircularBusEntities db = new CircularBusEntities();
            int s = (int)Session["userid"];
            var br = (from r in db.BusRoutes
                      where r.BR_Id == data4
                      select r).FirstOrDefault();

            if (data1 == br.BR_Stopage1)
            {
                Cart c = new Cart();
                c.U_Id = s;
                c.B_Id = data3;
                c.BS_Fair = data2;
                c.Stopage = data1;
                db.Carts.Add(c);
                db.SaveChanges();

                var data = (from car in db.Carts
                            where car.U_Id == s
                            select car).ToList();

                var sum_price = (from sum in db.Carts
                                 where sum.U_Id == s
                                 select sum).Sum(S => S.BS_Fair).ToString();
                ViewBag.SP = sum_price;
                return View(data);
            }

            else if (data1 == br.BR_Stopage2)
            {
                Cart c = new Cart();
                c.U_Id = s;
                c.B_Id = data3;
                c.BS_Fair = data2;
                c.Stopage = data1;
                db.Carts.Add(c);
                db.SaveChanges();

                var data = (from car in db.Carts
                            where car.U_Id == s
                            select car).ToList();

                var sum_price = (from sum in db.Carts
                                 where sum.U_Id == s
                                 select sum).Sum(S => S.BS_Fair).ToString();
                ViewBag.SP = sum_price;
                return View(data);

            }

            else if (data1 == br.BR_Stopage3)
            {
                Cart c = new Cart();
                c.U_Id = s;
                c.B_Id = data3;
                c.BS_Fair = data2;
                c.Stopage = data1;
                db.Carts.Add(c);
                db.SaveChanges();

                var data = (from car in db.Carts
                            where car.U_Id == s
                            select car).ToList();

                var sum_price = (from sum in db.Carts
                                 where sum.U_Id == s
                                 select sum).Sum(S => S.BS_Fair).ToString();
                ViewBag.SP = sum_price;
                return View(data);
            }

            return View();
        }



        [Authorize]

        public ActionResult CartDetail()
        {
            CircularBusEntities db = new CircularBusEntities();
            int s = (int)Session["userid"];
            var data = (from car in db.Carts
                        where car.U_Id == s
                        select car).ToList();

            var sum_price = (from sum in db.Carts
                             where sum.U_Id == s
                             select sum).Sum(S=>S.BS_Fair).ToString();
            ViewBag.SumPrice = sum_price;
            return View(data);

        }

        [HttpGet]
        public ActionResult DeleteTicket(int id)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from c in db.Carts
                        where c.CR_Id == id
                        select c).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult DeleteTicket(Cart delete)
        {
            CircularBusEntities db = new CircularBusEntities();
            var data = (from c in db.Carts
                        where c.CR_Id == delete.CR_Id
                        select c).FirstOrDefault();
            db.Entry(data).CurrentValues.SetValues(delete);
            db.Carts.Remove(data);
            db.SaveChanges();
            return RedirectToAction("CartDetail", "User");



        }

    }
}