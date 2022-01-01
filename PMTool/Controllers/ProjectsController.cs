using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PMTool.EntityModel;

namespace PMTool.Controllers
{
    public class ProjectsController : SharedController
    {
        private PMTEntities2 db = new PMTEntities2();

        // GET: Projects
        public ActionResult Index()
        {
            var projects = db.Projects;
            return View(projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            Project project = new Project();
            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name");
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.employee365s = new SelectList(db.Employee365, "Id", "Name");
           
            ViewBag.Status = new SelectList(db.Status, "Name", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,UploadAttachement,StartDate,EndDate,Status,FK_ProjectOwner,Budget,Progress,Priority,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,employee365s,images,files")] Project project)
        {
            if (ModelState.IsValid)
            {
                List<Attachment> Attachmentss = new List<Attachment>();
                foreach (HttpPostedFileBase file in project.files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/ProjectFiles/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                       
                        //assigning file uploaded status to ViewBag for showing message to user.  
                        //ViewBag.UploadStatus = project.files.Count().ToString() + " files uploaded successfully.";

                        //project.ProductPictureList = new List<Attachment>();
                        
                            Attachment objprflepicpth = new Attachment();
                            objprflepicpth.Path = ServerSavePath;


                            var full = InputFileName.Split('.');

                            var fullLength = 1;
                            for (int j = full.Length - 1; j >= 0; j--)
                            {
                                fullLength = full.Length - 1;
                                if (fullLength == j)
                                {
                                    objprflepicpth.extention = full[j].ToString();
                                }
                                else
                                {
                                    objprflepicpth.Name += full[j].ToString();
                                }
                            }
                        Attachmentss.Add(objprflepicpth);
                    }

                }

                //Procedure Working
                //int proId = 0;
                ObjectParameter exceptionMessage = new ObjectParameter("ExceptionMessage", typeof(string));
                ObjectParameter validateMessage = new ObjectParameter("ValidateMessage", typeof(string));
                ObjectParameter proId = new ObjectParameter("ProId", typeof(int));

                var res = db.SP_AddProject(project.Name, project.Description, project.UploadAttachement, project.StartDate, project.EndDate,
                             project.FK_ProjectOwner, project.Status, project.Budget, project.Progress, project.Priority, null, null, project.CreatedBy, null, project.ModifiedBy,
                             project.IsActive, exceptionMessage,validateMessage,proId);

                List<int> empIds=  project.employee365s.ToList();
                for(int i=0; i < project.employee365s.Count(); i++)
                {
                    var ProId = proId.Value;
                    var empId = empIds[i];
                    var empName = db.Employee365.Where(x => x.Id == empId).Select(m => m.Name).FirstOrDefault();

                    var result = db.AddProjectMember(empName, (int?)ProId,empId,null, project.CreatedBy, null, project.ModifiedBy,
                             project.IsActive, exceptionMessage, validateMessage);
                }

                if (Attachmentss != null)
                {
                    foreach (var item in Attachmentss)
                    {

                        var result = db.AddAttachment(item.Name,item.Path,item.extention, (int?)proId.Value, null, project.CreatedBy, null, project.ModifiedBy,
                            project.IsActive, exceptionMessage, validateMessage);
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", project.Priority);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", project.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", project.Status);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            //Attachment attachment = db.Attachments.FirstOrDefault(a => a.FK_Project == id);
            //project.UploadAttachement = attachment.Name;

            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", project.Priority);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", project.FK_ProjectOwner);
            ViewBag.employee365s = new SelectList(db.Employee365, "Id", "Name");
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", project.Status);

            List<ProjectMember> pms = new List<ProjectMember>();
            var projectmembers = db.ProjectMembers.Where(x => x.FK_Project == id).ToList();
            for(int i=0; i < projectmembers.Count(); i++)
            {
                ProjectMember projectMember = new ProjectMember();
                projectMember.Id = (int)projectmembers[i].FK_Employee365ID;
                projectMember.Name = projectmembers[i].Name;
                pms.Add(projectMember);
            }
            ViewBag.employees = pms;

            List<Attachment> attachments = new List<Attachment>();
            var ProAttachments = db.Attachments.Where(x => x.FK_Project == id).ToList();
            for (int i = 0; i < ProAttachments.Count(); i++)
            {
                Attachment attachment = new Attachment();
                attachment.Name = ProAttachments[i].Name;
                // Id On delete button
                attachment.Id = ProAttachments[i].Id;
                attachments.Add(attachment);
            }
            project.ProductPictureList = attachments;
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,UploadAttachement,StartDate,EndDate,FK_ProjectOwner,Status,Budget,Progress,Priority,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy,IsActive,employee365s,files")] Project project)
        {
            if (ModelState.IsValid)
            {
                List<Attachment> Attachmentss = new List<Attachment>();
                foreach (HttpPostedFileBase file in project.files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/ProjectFiles/") + InputFileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);

                        //assigning file uploaded status to ViewBag for showing message to user.  
                        //ViewBag.UploadStatus = project.files.Count().ToString() + " files uploaded successfully.";

                        //project.ProductPictureList = new List<Attachment>();

                        Attachment objprflepicpth = new Attachment();
                        objprflepicpth.Path = ServerSavePath;


                        var full = InputFileName.Split('.');

                        var fullLength = 1;
                        for (int j = full.Length - 1; j >= 0; j--)
                        {
                            fullLength = full.Length - 1;
                            if (fullLength == j)
                            {
                                objprflepicpth.extention = full[j].ToString();
                            }
                            else
                            {
                                objprflepicpth.Name += full[j].ToString();
                            }
                        }
                        Attachmentss.Add(objprflepicpth);
                    }

                }

                #region Procedure Work
                ObjectParameter exceptionMessage = new ObjectParameter("ExceptionMessage", typeof(string));
                ObjectParameter validateMessage = new ObjectParameter("ValidateMessage", typeof(string));

                var res = db.SP_EditProject(project.Id, project.Name, project.Description, project.UploadAttachement, project.StartDate, project.EndDate,
                           project.FK_ProjectOwner, project.Status, project.Budget, project.Progress, project.Priority, null, null, project.CreatedBy, null, project.ModifiedBy,
                           project.IsActive, exceptionMessage, validateMessage);

                // Add Attachments
                if (Attachmentss != null)
                {
                    foreach (var item in Attachmentss)
                    {

                        var result = db.AddAttachment(item.Name, item.Path, item.extention, (int?)project.Id, null, project.CreatedBy, null, project.ModifiedBy,
                            project.IsActive, exceptionMessage, validateMessage);
                    }
                }

                //Delete Existing Values
                List<ProjectMember> pms = new List<ProjectMember>();
                var projectmembers = db.ProjectMembers.Where(x => x.FK_Project == project.Id).ToList();
                foreach(var item in projectmembers)
                {
                    //ProjectMember projectMember = new ProjectMember();
                    //projectMember.Id = (int)projectmembers[i].Id;
                    db.ProjectMembers.Remove(item);
                }
                db.SaveChanges();

                // Add New Values
                List<int> empIds = project.employee365s.ToList();
                for (int i = 0; i < project.employee365s.Count(); i++)
                {
                    var ProId = project.Id;
                    var empId = empIds[i];
                    var empName = db.Employee365.Where(x => x.Id == empId).Select(m => m.Name).FirstOrDefault();

                    var result = db.AddProjectMember(empName, (int?)ProId, empId, null, project.CreatedBy, null, project.ModifiedBy,
                             project.IsActive, exceptionMessage, validateMessage);
                }

                #endregion


                return RedirectToAction("Index");
            }

            ViewBag.Priority = new SelectList(db.Priorities, "Name", "Name", project.Priority);
            ViewBag.FK_ProjectOwner = new SelectList(db.Employee365, "Id", "Name", project.FK_ProjectOwner);
            ViewBag.Status = new SelectList(db.Status, "Name", "Name", project.Status);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                Attachment attachment = db.Attachments.Find(id);
                if (attachment == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new { Result = "Error" });
                }

                //Remove from database
                db.Attachments.Remove(attachment);
                db.SaveChanges();

                //Delete file from the file system
                var path = attachment.Path;/* Path.Combine(Server.MapPath("~/ProjectFiles/"), attachment.Name + attachment.extention);*/
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
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
