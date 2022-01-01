using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMTool.Controllers
{
    public class SharedController : Controller
    {
        

        public string[] SavePicture(HttpPostedFileWrapper[] pics)
        {
            string[] profilepath = new string[pics.Length];
            try
            {
                if (pics.Length > 0)
                {
                    for (int i = 0; i < pics.Length; i++)
                    {
                        string filename = System.IO.Path.GetRandomFileName().Replace(".", "") + pics[0].FileName;

                        pics[i].SaveAs(Server.MapPath("/ProjectFiles/" + filename));
                        profilepath[i] = "/ProjectFiles/" + filename;
                    }
                }
                return profilepath;

            }
            catch (Exception ex)
            {
                return profilepath;
            }

        }
    }
}