using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MBBS.Authentication;
using MBBS.Builders;
using MBBS.Calculators;
using MBBS.Database;
using MBBS.Models;
using System.Web;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Survey")]
    public class SurveyController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetSurveyQuestions")]
        public IHttpActionResult Get(string languageID)
        {
            SurveyQueries query = new SurveyQueries();
            return Ok(query.GetQuestions(languageID));
        }

        [Route("PostAnswers")]
        public IHttpActionResult Post()
        {
            int userID = authenticate.confirmToken();
            try
            {
                if (userID != 0)
                {
                    AnswerListBuilder listBuilder = new AnswerListBuilder();
                    SurveyQueries query = new SurveyQueries();
                    List<Answer> answers = listBuilder.BuildAnswerList(HttpContext.Current.Request["answers"]);
                }
                else
                {
                    return Unauthorized();
                }
                return Ok("success");
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
            
        }

        [RoutePrefix("api/Survey")]
        public class QuestionController : ApiController
        {
            Authenticate authenticate = new Authenticate();

            [Route("AverageRatingPerYear")]
            public IHttpActionResult Get(string moduleID)
            {
                //int userID = authenticate.confirmToken();
                int userID = 1;
                if (userID != 0)
                {
                    SurveyCalculators calculator = new SurveyCalculators();
                    return Ok(calculator.GetAverageRatingPerYear(moduleID));
                }
                else
                {
                    return Unauthorized();
                }
            }

            [Route("AverageRatingPerSubsection")]
            public IHttpActionResult Get(string moduleID, int subsectionID)
            {
                //int userID = authenticate.confirmToken();
                int userID = 1;
                if (userID != 0)
                {
                    SurveyCalculators calculator = new SurveyCalculators();
                    return Ok(calculator.GetAverageRatingPerYearPerSubsection(moduleID, subsectionID));
                }
                else
                {
                    return Unauthorized();
                }
            }

            
        }

        [RoutePrefix("api/Survey")]
        public class SubsectionSurveyController: ApiController
        {
            [Route("AverageRatingSubsections")]
            public IHttpActionResult Get(string moduleID)
            {
                int userID = 1;
                if (userID != 0)
                {
                    SurveyCalculators calculator = new SurveyCalculators();
                    return Ok(calculator.GetAverageRatingPerSubsection(moduleID));
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }
}
