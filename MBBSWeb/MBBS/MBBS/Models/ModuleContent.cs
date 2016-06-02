using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class ModuleContent
    {
        public int AuthorID { get; set; }
        public string Content { get; set; }
        public double VersionNumber { get; set; }
    }
}