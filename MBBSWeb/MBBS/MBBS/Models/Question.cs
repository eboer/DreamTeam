//===========================================================================================
//Project: MBBS
//Description:
//   Question object. Contains question text and info for survey.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

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