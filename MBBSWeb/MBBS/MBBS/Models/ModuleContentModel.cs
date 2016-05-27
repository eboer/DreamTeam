using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class ModuleContentModel
    {
        public string ModuleID { get; set; }
        public int SubsectionID { get; set; }
        public string LanguageID { get; set; }
        public int AuthorID { get; set; }
        public string Content { get; set; }
        public float VersionNumber { get; set; }
    }
}