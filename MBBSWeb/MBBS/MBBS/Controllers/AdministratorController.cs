//===========================================================================================
//Project: MBBS
//Description:
//   Controllers for administrator use.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using MBBS.Authentication;
using MBBS.Database;
using MBBS.Models;
using System;
using System.Web.Http;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Administrator")]
    public class AdministratorController : ApiController
    {

        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Allows administrator to create new module.
        /// </summary>
        /// <returns></returns>
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
