using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DealCrash.Models;

namespace DealCrash.Controllers
{
    public class UsersController : Controller
    {
        private DealDBContext db = new DealDBContext();

        // GET: Users
        public ActionResult Index()
        {
            User usr = (User)Session["user"];
            List<User> users = null;

            if (usr == null)
                users = new List<User>();
            else if (usr.admin)
                users = db.Users.ToList<User>();
            else
                users = db.Users.Where(u => u.UserID == usr.UserID).ToList<User>();

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            User usr = (User)Session["user"];
            if (usr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (usr.admin)
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                usr = user;
            }
            else
            {
                id = usr.UserID;
            }

            return View(usr);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,email,password,confirmPassword,name,link,gender,birthDate,createdDate,admin,active")] User user)
        {
            if (ModelState.IsValid)
            {
                var userId = (from u in db.Users where u.email == user.email select u).FirstOrDefault();
                if (userId == null)
                {
                    user.createdDate = DateTime.Now;
                    user.active = true;
                    db.Users.Add(user);
                    db.SaveChanges();
                    Session.Add("user", user);
                    return RedirectToAction("Index");
                }
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            User usr = (User)Session["user"];
            if (usr == null)
            {
                return View();
            }
            else if (usr.admin)
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                usr = user;
            }
            else
            {
                id = usr.UserID;
            }

            return View(usr);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,email,password,confirmPassword,name,link,gender,birthDate,createdDate,admin,active")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception) { }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User usr = (User)Session["user"];
            if (usr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            else if (!usr.admin)
            {
                if (usr.UserID != id)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "email,password")] User user)
        {
            User usr = null;
            try
            {
                usr = db.Users.Single(u => u.email.Equals(user.email) && u.password.Equals(user.password));
                if (usr != null)
                {
                    Session.Add("user", usr);
                }
            }
            catch (Exception) { }

            return RedirectToAction("Index", "Users");
        }

        public ActionResult Stats()
        {
            ViewBag.posts = StatsPost();
            ViewBag.comments = StatsComment();

            return View();
        }

        public ActionResult LoginToSystem()
        {
            return View();
        }

        /*
         *  this function return how many users postes something in the same year
         *  and how much posts was posted in the same year
         */
        public List<Join> StatsPost()
        {
            var result = from u in this.db.Users
                         group u by u.createdDate.Year into ug
                         // join *after* group
                         join post in db.Deals on ug.FirstOrDefault().createdDate.Year equals post.createdDate.Year
                         select new Join
                         {
                             year = ug.FirstOrDefault().createdDate.Year,
                             users = ug.Count(),
                             count = ug.Sum(p => p.deals.Count)
                         };

            return result.Distinct().ToList<Join>();
        }

        /*
         *  this function return how many users comments something in the same year
         *  and how much comments was posted in the same year
         */
        public List<Join> StatsComment()
        {
            var result = from u in this.db.Users
                         group u by u.createdDate.Year into ug
                         // join *after* group
                         join c in db.Comments on ug.FirstOrDefault().createdDate.Year equals c.createdDate.Year
                         select new Join
                         {
                             year = ug.FirstOrDefault().createdDate.Year,
                             users = ug.Count(),
                             count = ug.Sum(p => p.comments.Count)
                         };
            return result.Distinct().ToList<Join>();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
