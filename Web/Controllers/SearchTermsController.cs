using Bergfall.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bergfall.Web.Controllers
{
    public class SearchTermsController : Controller
    {
        private BergfallDataContext db = new BergfallDataContext();

        // GET: SearchTerms
        public ActionResult Index()
        {
            return View(db.SearchTerms.ToList());
        }

        // GET: SearchTerms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchTerm searchTerm = db.SearchTerms.Find(id);
            if (searchTerm == null)
            {
                return HttpNotFound();
            }
            return View(searchTerm);
        }

        // GET: SearchTerms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchTerms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Value")] SearchTerm searchTerm)
        {
            if (ModelState.IsValid)
            {
                db.SearchTerms.Add(searchTerm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(searchTerm);
        }

        // GET: SearchTerms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchTerm searchTerm = db.SearchTerms.Find(id);
            if (searchTerm == null)
            {
                return HttpNotFound();
            }
            return View(searchTerm);
        }

        // POST: SearchTerms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Value")] SearchTerm searchTerm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(searchTerm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(searchTerm);
        }

        // GET: SearchTerms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SearchTerm searchTerm = db.SearchTerms.Find(id);
            if (searchTerm == null)
            {
                return HttpNotFound();
            }
            return View(searchTerm);
        }

        // POST: SearchTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SearchTerm searchTerm = db.SearchTerms.Find(id);
            db.SearchTerms.Remove(searchTerm);
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
