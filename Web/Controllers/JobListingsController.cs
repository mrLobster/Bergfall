using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bergfall.Data;

namespace Bergfall.Web.Controllers
{
    public class JobListingsController : Controller
    {
        private BergfallDataContext db = new BergfallDataContext();

        // GET: JobListings
        public ActionResult Index()
        {
            return View(db.JobListings.ToList());
        }

        // GET: JobListings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobListing jobListing = db.JobListings.Find(id);
            if (jobListing == null)
            {
                return HttpNotFound();
            }
            return View(jobListing);
        }

        // GET: JobListings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobListings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RetrievalDate,Title,Text,Source")] JobListing jobListing)
        {
            if (ModelState.IsValid)
            {
                db.JobListings.Add(jobListing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobListing);
        }

        // GET: JobListings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobListing jobListing = db.JobListings.Find(id);
            if (jobListing == null)
            {
                return HttpNotFound();
            }
            return View(jobListing);
        }

        // POST: JobListings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RetrievalDate,Title,Text,Source")] JobListing jobListing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobListing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobListing);
        }

        // GET: JobListings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobListing jobListing = db.JobListings.Find(id);
            if (jobListing == null)
            {
                return HttpNotFound();
            }
            return View(jobListing);
        }

        // POST: JobListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobListing jobListing = db.JobListings.Find(id);
            db.JobListings.Remove(jobListing);
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
