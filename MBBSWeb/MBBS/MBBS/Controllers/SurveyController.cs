using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MBBS.Authentication;
using MBBS.Calculators;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Survey")]
    public class SurveyController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("AverageRatingPerYear")]
        public IHttpActionResult Get(string moduleID)
        {
            int userID = authenticate.confirmToken();
            //int userID = 1;
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
    }
}
