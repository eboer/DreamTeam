//===========================================================================================
//Project: MBBS
//Description:
//   Object of module matrix. Contains headers and list of competencies.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System.Collections.Generic;

namespace MBBS.Models
{
	public class MatrixData
	{
        public string Name { get; set; }
        public List<string> XAxis { get; set; }
        public List<string> YAxis { get; set; }
        public List<Competency> competencies { get; set; }
        
    }
}