//===========================================================================================
//Project: MBBS
//Description:
//   
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using System;
using System.Collections.Generic;
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
                    ObjectBuilder listBuilder = new ObjectBuilder();
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

       
    }

    [RoutePrefix("api/Survey")]
    public class RatingController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("AverageRatingPerYear")]
        public IHttpActionResult Get(string moduleID)
        {
            int userID = authenticate.confirmToken();

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
            int userID = authenticate.confirmToken();
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
    public class SubsectionSurveyController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("AverageRatingSubsections")]
        public IHttpActionResult Get(string moduleID)
        {
            int userID = authenticate.confirmToken();

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

    [RoutePrefix("api/Survey")]
    public class CommentsController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetComments")]
        public IHttpActionResult Get(string moduleID, string languageID)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                SurveyQueries query = new SurveyQueries();
                return Ok(query.GetComments(moduleID, languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("PostSurveyAnswers")]
        public IHttpActionResult Post()
        {
            int test = 5;
            return Ok("success1");
        }

        [Route("PostSurveyAnswers")]
        public IHttpActionResult Get()
        {
            int test = 5;
            return Ok("success_Get");
        }

        [Route("PostSurveyAnswers")]
        public IHttpActionResult Get(string content)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ObjectBuilder builder = new ObjectBuilder();
                SurveyQueries query = new SurveyQueries();
                CompletedSurvey completedSurvey = builder.BuildCompletedSurvey(content, userID);
                query.PostAnswers(completedSurvey);

                return Ok("success");
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("PostSurveyAnswers")]
        public IHttpActionResult Post([FromBody] string value)
        {
            
            return Ok("success");
        }
    }

    
}
