//===========================================================================================
//Project: MBBS
//Description:
//   Controllers for survey related calls.
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
            AuthenticatedUser user = authenticate.confirmToken();
            try
            {
                if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();

            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();

            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
            {
                SurveyQueries query = new SurveyQueries();
                return Ok(query.GetComments(moduleID, languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Xamarin app group could not get POST working. 
        /// Temporary workaround: Posting with a GET.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [Route("PostSurveyAnswers")]
        public IHttpActionResult Get(string content)
        {
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
            {
                ObjectBuilder builder = new ObjectBuilder();
                SurveyQueries query = new SurveyQueries();
                CompletedSurvey completedSurvey = builder.BuildCompletedSurvey(content, user.UserID);
                query.PostAnswers(completedSurvey);

                return Ok("success");
            }
            else
            {
                return Unauthorized();
            }
        }

    }

    
}
