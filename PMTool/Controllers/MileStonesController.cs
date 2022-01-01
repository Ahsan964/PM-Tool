using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMTool.EntityModel;

namespace PMTool.Controllers
{
    public class MileStonesController : Controller
    {
        private PMTEntities2 db = new PMTEntities2();

        // GET: MileStones
        public ActionResult Index()
        {
            var mileStones = db.MileStones;
            return View(mileStones.ToList());
        }

        // GET: MileStones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileStone mileStone = db.MileStones.Find(id);
            if (mileStone == null)
            {
                return HttpNotFound();
            }
            return View(mileStone);
        }

        // GET: MileStones/Create
        public ActionResult Create()
        {
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name");
            ViewBag.Status = new SelectList(db.Status, "Name", "Name");
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name");
            ViewBag.FK_DependentOn = new SelectList(db.MileStones, "Id", "Name");
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.CurrentlyAssignedTo = new SelectList(db.Employee365, "Id", "Name");
            
            return View();
        }

        // POST: MileStones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FK_Project,Name,Description,StartDate,EndDate,Status,Budget,Progress,Priority,FK_DependentOn,IsDependent,AssignedTo,IsActive,FK_ProjectOwner")] MileStone mileStone)
        {
            if (ModelState.IsValid)
            {
                db.MileStones.Add(mileStone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", mileStone.FK_Project);
            ViewBag.CurrentlyAssignedTo = new SelectList(db.Employee365, "Id", "Name", mileStone.CurrentlyAssignedTo);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", mileStone.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", mileStone.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", mileStone.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.MileStones, "Id", "Name",mileStone.FK_DependentOn);
            return View(mileStone);
        }

        // GET: MileStones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileStone mileStone = db.MileStones.Find(id);
            if (mileStone == null)
            {
                return HttpNotFound();
            }

            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", mileStone.FK_Project);
            ViewBag.CurrentlyAssignedTo = new SelectList(db.Employee365, "Id", "Name", mileStone.CurrentlyAssignedTo);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name",mileStone.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", mileStone.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", mileStone.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.MileStones, "Id", "Name", mileStone.FK_DependentOn);

            return View(mileStone);
        }

        // POST: MileStones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FK_Project,Name,Description,StartDate,EndDate,Status,Budget,Progress,Priority,FK_DependentOn,IsDependent,AssignedTo,IsActive,FK_ProjectOwner")] MileStone mileStone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mileStone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", mileStone.FK_Project);
            ViewBag.CurrentlyAssignedTo = new SelectList(db.Employee365, "Id", "Name", mileStone.CurrentlyAssignedTo);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name",mileStone.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", mileStone.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", mileStone.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.MileStones, "Id", "Name", mileStone.FK_DependentOn);

            return View(mileStone);
        }

        // GET: MileStones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MileStone mileStone = db.MileStones.Find(id);
            if (mileStone == null)
            {
                return HttpNotFound();
            }
            return View(mileStone);
        }

        // POST: MileStones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MileStone mileStone = db.MileStones.Find(id);
            db.MileStones.Remove(mileStone);
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
