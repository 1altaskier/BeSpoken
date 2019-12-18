using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeSpoken.Models;

namespace BeSpoken.Controllers
{
    public class SalesPersonsController : Controller
    {
        private BeSpokenEntities db = new BeSpokenEntities();

        // GET: SalesPersons
        public ActionResult Index(string sortOrder)
        {
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var salesPersons = db.SalesPersons.Include(s => s.Manager);

            switch (sortOrder)
            {
                case "last_name_desc":
                    salesPersons = salesPersons.OrderByDescending(s => s.LastName);
                    break;
                case "first_name_desc":
                    salesPersons = salesPersons.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    salesPersons = salesPersons.OrderByDescending(s => s.FirstName);
                    break;
            }

            return View(salesPersons.ToList());
        }

        // GET: SalesPersons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesPerson salesPerson = db.SalesPersons.Find(id);
            if (salesPerson == null)
            {
                return HttpNotFound();
            }
            return View(salesPerson);
        }

        // GET: SalesPersons/Create
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName");
            return View();
        }

        // POST: SalesPersons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalesPersonId,FirstName,LastName,Address,City,State,Zip,Phone,AltPhone,StartDate,TermDate,ManagerId")] SalesPerson salesPerson)
        {
            if (ModelState.IsValid)
            {
                // Create Input Parameters
                var FirstName = new SqlParameter("@FirstName", salesPerson.FirstName);
                var LastName = new SqlParameter("@LastName", salesPerson.LastName);
                var Address = new SqlParameter("@Address", salesPerson.Address);
                var City = new SqlParameter("@City", salesPerson.City);
                var State = new SqlParameter("@State", salesPerson.State);
                var Zip = new SqlParameter("@Zip", salesPerson.Zip);
                var Phone = new SqlParameter("@Phone", salesPerson.Phone);
                var AltPhone = new SqlParameter("@AltPhone", salesPerson.Phone);
                var StartDate = new SqlParameter("@StartDate", salesPerson.StartDate);
                var ManagerId = new SqlParameter("@ManagerId", salesPerson.ManagerId);

                // Create Output Parameter
                var outParam = new SqlParameter("@SalespersonId", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;

                var sql = "exec @SalespersonId = AddSalesperson " +
                    "@FirstName, " +
                    "@LastName, " +
                    "@Address, " +
                    "@City, " +
                    "@State, " +
                    "@Zip, " +
                    "@Phone, " +
                    "@AltPhone, " +
                    "@StartDate, " +
                    "@ManagerId, " +
                    "@SalespersonId OUT";

                var data = db.Database.SqlQuery<object>(sql,
                    FirstName,
                    LastName,
                    Address,
                    City,
                    State,
                    Zip,
                    Phone,
                    AltPhone,
                    StartDate,
                    ManagerId,
                    outParam);

                // Read the results so that the output variables are accessible
                var item = data.FirstOrDefault();

                var salesPersonIdOutput = (int)outParam.Value;

                //if (salesPersonIdOutput > 1)
                //{
                //    return RedirectToAction("Index");
                //}

                //ViewBag.Message = "Salesperon Already Exists";

                //return RedirectToAction("Create");

                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", salesPerson.ManagerId);
            ViewBag.State = new SelectList(db.States, "StateAbbrv", "StateName", salesPerson.State);

            return View(salesPerson);
        }

    

    // GET: SalesPersons/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        SalesPerson salesPerson = db.SalesPersons.Find(id);
        if (salesPerson == null)
        {
            return HttpNotFound();
        }
        ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", salesPerson.ManagerId);
        return View(salesPerson);
    }

    // POST: SalesPersons/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "SalesPersonId,FirstName,LastName,Address,City,State,Zip,Phone,AltPhone,StartDate,TermDate,ManagerId")] SalesPerson salesPerson)
    {
        if (ModelState.IsValid)
        {
            db.Entry(salesPerson).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.ManagerId = new SelectList(db.Managers, "ManagerId", "FirstName", salesPerson.ManagerId);
        return View(salesPerson);
    }

    // GET: SalesPersons/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        SalesPerson salesPerson = db.SalesPersons.Find(id);
        if (salesPerson == null)
        {
            return HttpNotFound();
        }
        return View(salesPerson);
    }

    // POST: SalesPersons/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        SalesPerson salesPerson = db.SalesPersons.Find(id);

        try
        {
            db.SalesPersons.Remove(salesPerson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Error Encountered While Attempting to Delete - Msg: " + ex.Message;

            return View(salesPerson);
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
