using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class ModuleVersion
    {
        public double VersionNumber { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string VersionDescription { get; set; }
        public DateTime VersionDate { get; set; }
        public string StudyYear { get; set; }
    }
}