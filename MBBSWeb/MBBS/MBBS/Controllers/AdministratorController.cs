using MBBS.Authentication;
using MBBS.Database;
using MBBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Administrator")]
    public class AdministratorController : ApiController
    {

        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Returns list of modules which docent manages
        /// </summary>
        [Route("DocentModules")]
        public IHttpActionResult Get()
        {
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("CreateModule")]
        public IHttpActionResult Post()
        {
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0 && user.UserTypeID.Equals(3))
            {
                try
                {
                    AdministratorQueries query = new AdministratorQueries();
                    query.PostDocentModule();
                    return Ok();
                }
                catch(Exception e)
                {
                    return InternalServerError(e);
                }
                
            }
            else
            {
                return Unauthorized();
            }
        }
        
    }
}
