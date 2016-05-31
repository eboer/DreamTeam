using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBBS.Models;
using Newtonsoft.Json;

namespace MBBS.Builders
{
    public class AnswerListBuilder
    {
        public List<Answer> BuildAnswerList(string AnswerJSON)
        {
            List<Answer> answers = new List<Answer>();
            JsonConvert.DeserializeObject<Answer>(AnswerJSON);

            return answers;
        }
    }
}