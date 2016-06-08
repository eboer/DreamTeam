using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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