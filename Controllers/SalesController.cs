using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeSpoken.Models;

namespace BeSpoken.Controllers
{
    public class SalesController : Controller
    {
        private BeSpokenEntities db = new BeSpokenEntities();

        // GET: Sales
        public ActionResult Index()
        {
            var sales = db.Sales.Include(s => s.Customer).Include(s => s.Product).Include(s => s.SalesPerson);
            return View(sales.ToList());
        }

        // GET: Sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // GET: Sales/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "LastName");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalesId,ProductId,SalesPersonId,CustomerId,SalesDate,Qty")] Sale sale)
        {
            if (ModelState.IsValid)
            {
            SqlParameter param1 = new SqlParameter("@ProductId", sale.ProductId);
                SqlParameter param2 = new SqlParameter("@SalesPersonId", sale.SalesPersonId);
                SqlParameter param3 = new SqlParameter("@CustomerId", sale.CustomerId);
                SqlParameter param4 = new SqlParameter("@Qty", sale.Qty);
                db.Database.ExecuteSqlCommand("AddSale @ProductId, @SalesPersonId, @CustomerId, @Qty",
                                              param1, param2, param3, param4);

                //db.Sales.Add(sale);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", sale.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", sale.SalesPersonId);
            return View(sale);
        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", sale.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", sale.SalesPersonId);
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalesId,ProductId,SalesPersonId,CustomerId,SalesDate,Qty")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", sale.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", sale.ProductId);
            ViewBag.SalesPersonId = new SelectList(db.SalesPersons, "SalesPersonId", "FirstName", sale.SalesPersonId);
            return View(sale);
        }

        // GET: Sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sale sale = db.Sales.Find(id);
            db.Sales.Remove(sale);
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
