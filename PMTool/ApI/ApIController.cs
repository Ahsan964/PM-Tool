using Newtonsoft.Json;
using PMTool.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using PMTool.Models;


namespace PMTool.ApI
{
   public class ApIController : Controller
    {
        private PMTEntities2 db = new PMTEntities2();
        // GET api/<controller>
        public ActionResult GetProjectList()
        {
            DTO result = new DTO();
            var projects = db.Projects.ToList();

            var objresult = Newtonsoft.Json.JsonConvert.SerializeObject(projects,
                Formatting.None,
                    new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

            result.data = objresult;
            result.isSuccessful = true;

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //public HttpResponseMessage GetProjectList()
        //{
        //    var projects = db.Projects.ToList();
        //    return Request.CreateResponse(HttpStatusCode.OK, projects, Configuration.Formatters.JsonFormatter);
        //}

        //public IHttpActionResult GetProjectList2()
        //{
        //    var projects = db.Projects.ToList();
        //    return Json(projects);
        //}

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}