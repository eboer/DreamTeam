//===========================================================================================
//Project: MBBS
//Description:
//   Object containing student answer data.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;

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