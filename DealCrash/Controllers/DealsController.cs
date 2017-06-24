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
    public class DealsController : Controller
    {
        private DealDBContext db = new DealDBContext();

        // GET: Deals
        public ActionResult Index()
        {
            User usr = (User)Session["user"];
            List<Deal> deals = null;
            deals = db.Deals.Include(d => d.user).OrderByDescending(d => d.createdDate).ToList<Deal>();

            return View(deals);
        }


        public ActionResult CatClothing()
        {
            User usr = (User)Session["user"];
            List<Deal> deals = null;

            if (usr == null)
                deals = new List<Deal>();
            else if (usr.admin)
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Clothing").OrderByDescending(d => d.createdDate).ToList<Deal>();
            else
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Clothing").Where(d => d.UserID == usr.UserID).OrderByDescending(d => d.createdDate).ToList<Deal>();
            return View(deals);
        }

        public ActionResult CatFood()
        {
            User usr = (User)Session["user"];
            List<Deal> deals = null;

            if (usr == null)
                deals = new List<Deal>();
            else if (usr.admin)
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Food").OrderByDescending(d => d.createdDate).ToList<Deal>();
            else
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Food").Where(d => d.UserID == usr.UserID).OrderByDescending(d => d.createdDate).ToList<Deal>();
            return View("CatClothing", deals);
        }

        public ActionResult CatExtreme()
        {
            User usr = (User)Session["user"];
            List<Deal> deals = null;

            if (usr == null)
                deals = new List<Deal>();
            else if (usr.admin)
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Extreme").OrderByDescending(d => d.createdDate).ToList<Deal>();
            else
                deals = db.Deals.Include(d => d.user).Where(d => d.Category == "Extreme").Where(d => d.UserID == usr.UserID).OrderByDescending(d => d.createdDate).ToList<Deal>();
            return View("CatClothing", deals);
        }

        // GET: Deals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        public List<Deal> getAllDeals()
        {
            return db.Deals.Include(d => d.user).OrderByDescending(d => d.createdDate).ToList<Deal>();
        }

        // GET: /Post/Search
        public ActionResult Search(string text, DateTime? date, int? id)
        {
            var q = db.Deals.Include(d => d.user);

            if ((id != null) && (id > 0))
            {
                q = q.Where(d => d.DealID == id);
            }
            if ((text != null) && (text.ToString().Length > 0))
            {
                q = q.Where(d => d.text.Contains(text.ToString()));
            }
            if (date != null)
            {
                q = q.Where(d => d.createdDate == date);
            }

            return View(q.OrderByDescending(d => d.createdDate).ToList<Deal>());
        }


        // GET: Deals/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email");
            return View();
        }

        public ActionResult CreateComment(int userID, int dealID, string text)
        {
            Comment comment = new Comment();
            comment.DealID = dealID;
            comment.UserID = userID;
            comment.text = text;
            comment.createdDate = DateTime.Now;

            if (text == "")
            {
                comment.text = "תגובה ריקה";
            }

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Deals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DealID,UserID,title,text, Category,image, address, facebook")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                deal.createdDate = DateTime.Now;
                db.Deals.Add(deal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", deal.UserID);
            return View(deal);
        }

        // GET: Deals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", deal.UserID);
            return View(deal);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DealID,UserID,title,text, Category,image, address, facebook, createdDate")] Deal deal)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(deal).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e) { string m = e.Message; }
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", deal.UserID);
            return View(deal);
        }

        // GET: Deals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deal deal = db.Deals.Find(id);
            if (deal == null)
            {
                return HttpNotFound();
            }
            return View(deal);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deal deal = db.Deals.Find(id);
            db.Deals.Remove(deal);
            db.SaveChanges();
            return RedirectToAction("Index");
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