//===========================================================================================
//Project: MBBS
//Description:
//   
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using System.Web.Http;
using MBBS.Database;
using MBBS.Authentication;

namespace MBBS.Controllers
{
    
    [RoutePrefix("api/Module")]
    public class DocentModuleController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Returns list of modules which docent manages
        /// </summary>
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

        [Route("AddDocentModule")]
        public IHttpActionResult Post()
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                query.PostDocentModule(userID);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    [RoutePrefix("api/Module")]
    public class MatrixController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetMatrixData")]
        public IHttpActionResult Get(string moduleID, string languageID)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetMatrixData(moduleID, languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("PostCompetency")]
        public IHttpActionResult Post()
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                query.PostCompetency(userID);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    [RoutePrefix("api/Module")]
    public class ModuleController : ApiController
    {
        Authenticate authenticate = new Authenticate();
        //[Route("CreateModule")]
        // Only administrator can create modules

        [Route("GetModuleInfo")]
        public IHttpActionResult Get(string moduleID)
        {
            // int userID = authenticate.confirmToken();
            int userID = 6;
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                //ModuleInfo 
                //query.GetModuleInfo(moduleID);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    [RoutePrefix("api/Module")]
    public class ModuleDataController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetData")]
        public IHttpActionResult Get(string moduleID, int subsectionID, string languageID)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetModuleData(moduleID, subsectionID, languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

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

        [Route("GetSubsectionNames")]
        public IHttpActionResult Get(string languageID)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                
                return Ok(query.GetSubsectionNames(languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

    }

    [RoutePrefix("api/Module")]
    public class ModuleNamesController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("AllModules")]
        public IHttpActionResult Get()
        {           
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetAllModules());
        }

        [Route("GetSectionNames")]
        public IHttpActionResult Get(string languageID)
        {

            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();

                return Ok(query.GetSectionNames(languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

    }

    [RoutePrefix("api/Module")]
    public class VersionController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetVersions")]
        public IHttpActionResult Get(string moduleID, string languageID)
        {
            int userID = authenticate.confirmToken();
            if (userID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetVersionData(moduleID, languageID));
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("PostVersion")]
        public IHttpActionResult Post()
        {
            ModuleQueries query = new ModuleQueries();
           // query.
            return Ok();
        }
    }
    
}
