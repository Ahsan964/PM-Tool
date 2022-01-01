using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMTool.Models
{
    public class DTO
    {
        public bool isSuccessful { get; set; }
        public object data { get; set; }
        public object errors { get; set; }
    }
}