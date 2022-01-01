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
    public class ActivitiesController : Controller
    {
        private PMTEntities2 db = new PMTEntities2();

        // GET: Activities
        public ActionResult Index()
        {
            var activities = db.Activities.Include(a => a.Task);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name");
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name");
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name");
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.Status = new SelectList(db.Status, "Name", "Name");
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name");
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.FK_DependentOn = new SelectList(db.Activities, "Id", "Name");
            ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name");

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FK_Task,Name,Description,StartDate,EndDate,ProjectOwner,Status,Budget,Progress,Priority,FK_Project,FK_Milestone,FK_MilestoneOwner,AssignedTo,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,Longitude,Latitude,Radius,IsDependent,DependentOn,FK_ProjectOwner")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name", activity.FK_Task);
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", activity.FK_Milestone);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", activity.FK_Project);
  
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_MilestoneOwner);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_ProjectOwner);


            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }

            ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name", activity.FK_Task);
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", activity.FK_Milestone);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", activity.FK_Project);
     
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_MilestoneOwner);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_ProjectOwner);
  

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FK_Task,Name,Description,StartDate,EndDate,ProjectOwner,Status,Budget,Progress,Priority,FK_Project,FK_Milestone,FK_MilestoneOwner,AssignedTo,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,Longitude,Latitude,Radius,IsDependent,DependentOn,FK_ProjectOwner")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name", activity.FK_Task);
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", activity.FK_Milestone);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", activity.FK_Project);
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_MilestoneOwner);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", activity.FK_ProjectOwner);
           


            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
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
