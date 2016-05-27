using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Http;
using MBBS.Database;
using MBBS.Authentication;
using MBBS.Calculators;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Module")]
    public class ModuleController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("DocentModules")]
        public IHttpActionResult Get()
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                
                return Ok(query.GetDocentModules(userID));
            }
            else
            {
                return Unauthorized();
            }

        }

        //[Route("CreateModule")]
        // Only administrator can create modules

        //[Route("GetData")]
        //[Route("GetData")]
        //public IHttpActionResult Get(string moduleID, int subsection, string languageID)
        //{
        
        //}
        
        [Route("PostData")]
        public IHttpActionResult Post()
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                query.PostModuleData(userID);
            return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        //new JavaScriptSerializer().Serialize(ListOfMyObject);

    }
}
