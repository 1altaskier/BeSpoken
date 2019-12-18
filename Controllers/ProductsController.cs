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
    public class ProductsController : Controller
    {
        private BeSpokenEntities db = new BeSpokenEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Discount);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.DiscountId = new SelectList(db.Discounts, "DiscountId", "DiscountId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,Manufacturer,Style,PurchasePrice,SalePrice,QtyOnHand,CommissionPercentage,DiscountId")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Error Encountered While Attempting to Add Product (No Duplicates Allowed) - Msg: " + ex.Message;
                }
            }

            ViewBag.DiscountId = new SelectList(db.Discounts, "DiscountId", "DiscountId", product.DiscountId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.DiscountId = new SelectList(db.Discounts, "DiscountId", "DiscountId", product.DiscountId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Name,Manufacturer,Style,PurchasePrice,SalePrice,QtyOnHand,CommissionPercentage,DiscountId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DiscountId = new SelectList(db.Discounts, "DiscountId", "DiscountId", product.DiscountId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);

            try { 
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error Encountered While Attempting to Delete - Msg: " + ex.Message;

                return View(product);
            }
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
