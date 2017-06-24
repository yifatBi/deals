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
    public class CommentsController : Controller
    {
        private DealDBContext db = new DealDBContext();

        // GET: Comments
        public ActionResult Index()
        {
            User usr = (User)Session["user"];
            List<Comment> comments = null;

            if (usr != null && usr.admin)
                comments = db.Comments.Include(c => c.user).OrderByDescending(c => c.createdDate).ToList<Comment>();

            return View(comments);
        }

        // GET: /Comment/Search
        public ActionResult Search(string text, DateTime? date, int? id)
        {
            var q = db.Comments.Include(c => c.user);

            if ((id != null) && (id > 0))
            {
                q = q.Where(c => c.CommentID == id);
            }
            if ((text != null) && (text.ToString().Length > 0))
            {
                q = q.Where(c => c.text.Contains(text.ToString()));
            }
            if (date != null)
            {
                q = q.Where(c => c.createdDate == date);
            }

            return View(q.OrderByDescending(c => c.createdDate).ToList<Comment>());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.DealID = new SelectList(db.Deals, "DealID", "title");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,UserID,DealID,text,createdDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.createdDate = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DealID = new SelectList(db.Deals, "DealID", "title", comment.DealID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", comment.UserID);
            return View(comment);
        }

        // POST: /Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuick([Bind(Include = "CommentID,UserID,DealID,text,createdDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.createdDate = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/#Comment" + comment.CommentID);
            }

            ViewBag.DealID = new SelectList(db.Deals, "DealID", "title", comment.DealID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.DealID = new SelectList(db.Deals, "DealID", "title", comment.DealID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", comment.UserID);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,UserID,DealID,text,createdDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception) { }
                return RedirectToAction("Index");
            }
            ViewBag.DealID = new SelectList(db.Deals, "DealID", "title", comment.DealID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "email", comment.UserID);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
