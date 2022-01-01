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
    public class TasksController : Controller
    {
        private PMTEntities2 db = new PMTEntities2();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.MileStone);

            //ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name");
            //// ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name");
            //ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name");
            //ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name");
            //ViewBag.StatusT = new SelectList(db.Status, "Name", "Name");

            //ViewBag.Priority = new SelectList(db.Priorities.ToList(), "Name", "Name");
            //ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name");
            //ViewBag.FK_DependentOn = new SelectList(db.Tasks, "Id", "Name");
            //ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name");

            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name");
            ViewBag.FK_DependentTask = new SelectList(db.Tasks, "Id", "Name");
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name");
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.StatusT = new SelectList(db.Status, "Name", "Name");

            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name");
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.FK_DependentOn = new SelectList(db.Tasks, "Id", "Name");
            ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,ProjectOwner,Budget,Progress,FK_AssignTo,Status,Priority,FK_Project,FK_MileStone,FK_DependentTask,FK_MilestoneOwner,IsDependentTask,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,Longitude,Latitude,Radius,IsApproved,ApprovedBy,ApprovedOn,FK_ProjectOwner")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", task.FK_MileStone);
           // ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name", task.FK_Task);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", task.FK_Project);
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_MilestoneOwner);
            ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name", task.FK_AssignTo);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", task.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", task.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.Tasks, "Id", "Name", task.FK_DependentTask);

            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", task.FK_MileStone);
            // ViewBag.FK_Task = new SelectList(db.Tasks, "Id", "Name", task.FK_Task);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", task.FK_Project);
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_MilestoneOwner);
            ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name", task.FK_AssignTo);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", task.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", task.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.Tasks, "Id", "Name", task.FK_DependentTask);
            
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,ProjectOwner,Budget,Progress,FK_AssignTo,Status,Priority,FK_Project,FK_MileStone,FK_DependentTask,FK_MilestoneOwner,IsDependentTask,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,Longitude,Latitude,Radius,IsApproved,ApprovedBy,ApprovedOn,FK_ProjectOwner")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_MileStone = new SelectList(db.MileStones, "Id", "Name", task.FK_MileStone);
            ViewBag.FK_Project = new SelectList(db.Projects, "Id", "Name", task.FK_Project);
            ViewBag.AssignedTo = new SelectList(db.Employee365, "Id", "Name", task.FK_AssignTo);
            ViewBag.FK_MilestoneOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_MilestoneOwner);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", task.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", task.Status);
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", task.Priority);
            ViewBag.FK_DependentOn = new SelectList(db.Tasks, "Id", "Name", task.FK_DependentTask);

            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
