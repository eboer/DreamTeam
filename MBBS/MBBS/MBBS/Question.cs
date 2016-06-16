using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBBS
{
    // Class for storing the questions
    class Question
    {
        public string QuestionID { get; set; }
        public int SubsectionID { get; set; }
        public int QuestionTypeID { get; set; }
        public string QuestionText { get; set; }
    }
}
