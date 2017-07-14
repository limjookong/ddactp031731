using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetAppSqlDb.Models;

namespace DotNetAppSqlDb.Controllers
{
    [Authorize]
    public class ShipmentsController : Controller
    {
        private MyDatabaseContext db = new MyDatabaseContext();

        // GET: Shipments
        public ActionResult Index()
        {
            var shipments = db.Shipments.Include(s => s.Cargo);
            return View(shipments.ToList());
        }

        // GET: Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Id");
            ViewBag.status = GetAllStatus();
            var clients = db.Warehouses
                .Select(s => new
                {
                    Name = s.Name + " - " + s.Location,

                })
            .ToList();
            ViewBag.Source = new SelectList(clients, "Name", "Name");
            ViewBag.Destination = new SelectList(clients, "Name", "Name");
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,CargoId,Source,Destination,ShippingDate,ArrivalDate,status")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Shipments.Add(shipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Id", shipment.CargoId);
            ViewBag.status = GetAllStatus();
            var clients = db.Warehouses
                .Select(s => new
                {
                    Name = s.Name + " - " + s.Location,

                })
            .ToList();
            ViewBag.Source = new SelectList(clients, "Name", "Name");
            ViewBag.Destination = new SelectList(clients, "Name", "Name");
            return View(shipment);
        }

        private List<SelectListItem> GetAllStatus()
        {
            List<SelectListItem> StatusList = new List<SelectListItem>();

            StatusList.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            StatusList.Add(new SelectListItem { Text = "Delivery", Value = "Delivery" });
            StatusList.Add(new SelectListItem { Text = "Cancelled", Value = "Cancelled" });
            StatusList.Add(new SelectListItem { Text = "Completed", Value = "Completed" });

            return StatusList;
        }

        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Id", shipment.CargoId);
            ViewBag.status = GetAllStatus();
            var clients = db.Warehouses
                .Select(s => new
                {
                    Name = s.Name + " - " + s.Location,

                })
            .ToList();
            ViewBag.Source = new SelectList(clients, "Name", "Name");
            ViewBag.Destination = new SelectList(clients, "Name", "Name");
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CargoId,Source,Destination,ShippingDate,ArrivalDate,status")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CargoId = new SelectList(db.Cargoes, "Id", "Id", shipment.CargoId);
            ViewBag.status = GetAllStatus();
            var clients = db.Warehouses
                .Select(s => new
                {
                    Name = s.Name + " - " + s.Location,

                })
            .ToList();
            ViewBag.Source = new SelectList(clients, "Name", "Name");
            ViewBag.Destination = new SelectList(clients, "Name", "Name");
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
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
