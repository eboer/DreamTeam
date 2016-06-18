//===========================================================================================
//Project: MBBS
//Description:
//   Module info object. Contains module book titel page content.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System.Collections.Generic;

namespace MBBS.Models
{
    public class ModuleInfo
    {
        public string ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string LocationName { get; set; }
        public string Sector { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string Location { get; set; }
        public int POBox { get; set; }
        public string POBoxPostalCode { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Docent Coordinater { get; set; }
        public List<Docent> Author { get; set; }
    }
}