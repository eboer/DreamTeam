//===========================================================================================
//Project: MBBS
//Description:
//   Module version object.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;

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