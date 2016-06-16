using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class CompletedSurvey
    {
        public string ModuleID { get; set; }
        public int StudentID { get; set; }
        public DateTime DateCompleted { get; set; }
        public List<Answer> Answers { get; set; }
    }
}