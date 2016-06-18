//===========================================================================================
//Project: MBBS
//Description:
//   Competency object, contains coordinate (location within the matrix), description and 
//   level.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class Competency
    {
        public string Coordinate { get; set; }
        public string CompetencyDescription { get; set; }
        public int CompetencyLevel { get; set; }
    }
}