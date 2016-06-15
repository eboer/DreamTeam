using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class Answer
    {
        public double QuestionID { get; set; }
        public string QuestionText { get; set; }
        public int SubsectionID { get; set; }
        public string SubsectionName { get; set; }
        public DateTime DateCompleted { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}