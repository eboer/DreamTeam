using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class Question
    {
        public double QuestionID { get; set; }
        public int SubsectionID { get; set; }
        public int QuestionTypeID { get; set; }
        public string QuestionText { get; set; }
    }
}