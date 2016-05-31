using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class Subsection
    {
        public int SubsectionID { get; set; }
        public string SubsectionCode { get; set; }
        public string SubsectionName { get; set; }
        public Boolean RequiredBool { get; set; }
    }
}