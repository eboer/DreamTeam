//===========================================================================================
//Project: MBBS
//Description:
//   Subsection info object.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;

namespace MBBS.Models
{
    public class Subsection
    {
        public int SubsectionID { get; set; }
        public string SubsectionCode { get; set; }
        public string SubsectionName { get; set; }
        public Boolean RequiredBool { get; set; }
        public int SectionID { get; set; }
    }
}