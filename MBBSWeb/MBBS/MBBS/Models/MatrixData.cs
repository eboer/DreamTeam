using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
	public class MatrixData
	{
        public string matrix_name { get; set; }
        public List<string> xAxis { get; set; }
        public List<string> yAxis { get; set; }
        public List<Competency> competencies { get; set; }
        
    }
}