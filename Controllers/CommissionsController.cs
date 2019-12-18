using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeSpoken.Models;

namespace BeSpoken.Controllers
{
    public class CommissionsController : Controller
    {
        private BeSpokenEntities db = new BeSpokenEntities();

        // GET: Commissions
        public ActionResult Index()
        {
            var commissions = db.Commissions.Include(c => c.Qtr1).Include(c => c.SalesPerson);
            return View(commissions.ToList());
        }

        // GET: Commissions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        // GET: Commissions/Create
        public ActionResult Create()
        {
            ViewBag.QtrId = new SelectList(db.Qtrs, "QtrId", "QtrId");
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName");
            return View();
        }

        // POST: Commissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommissionId,QtrId,Amount,SalesPersonId,Qtr,Year")] Commission commission)
        {
            if (ModelState.IsValid)
            {
                db.Commissions.Add(commission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QtrId = new SelectList(db.Qtrs, "QtrId", "QtrId", commission.QtrId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", commission.SalesPersonId);
            return View(commission);
        }

        // GET: Commissions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            ViewBag.QtrId = new SelectList(db.Qtrs, "QtrId", "QtrId", commission.QtrId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", commission.SalesPersonId);
            return View(commission);
        }

        // POST: Commissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommissionId,QtrId,Amount,SalesPersonId,Qtr,Year")] Commission commission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QtrId = new SelectList(db.Qtrs, "QtrId", "QtrId", commission.QtrId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", commission.SalesPersonId);
            return View(commission);
        }

        // GET: Commissions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Commission commission = db.Commissions.Find(id);
            if (commission == null)
            {
                return HttpNotFound();
            }
            return View(commission);
        }

        // POST: Commissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Commission commission = db.Commissions.Find(id);
            db.Commissions.Remove(commission);
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
