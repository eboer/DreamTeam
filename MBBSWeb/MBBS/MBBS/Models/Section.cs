//===========================================================================================
//Project: MBBS
//Description:
//   Section and subsection names object.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System.Collections.Generic;

namespace MBBS.Models
{
    public class Section
    {
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public List<Subsection> Subsections { get; set; }
    }
}