//===========================================================================================
//Project: MBBS
//Description:
//   Completed survey object. Contains list of student answers.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;
using System.Collections.Generic;

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