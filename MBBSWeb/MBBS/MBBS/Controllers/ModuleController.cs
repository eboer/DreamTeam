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
using MBBS.Models;

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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
            {
                ModuleQueries query = new ModuleQueries();

                return Ok(query.GetDocentModules(user.UserID));
            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("AddDocentModule")]
        public IHttpActionResult Post()
        {
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0 && !user.UserTypeID.Equals(1))
            {
                ModuleQueries query = new ModuleQueries();
                query.PostDocentModule(user.UserID);
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0 && !user.UserTypeID.Equals(1))
            {
                ModuleQueries query = new ModuleQueries();
                query.PostCompetency(user.UserID);
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

        [Route("GetModuleInfo")]
        public IHttpActionResult Get(string moduleID)
        {
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
            {
                ModuleQueries query = new ModuleQueries();
                ModuleInfo moduleInfo = new ModuleInfo();
                moduleInfo = query.GetModuleInfo(moduleID);
                if(string.IsNullOrEmpty(moduleInfo.ModuleName))
                {
                    return BadRequest("The module you requested does not exist.");
                }
                return Ok(moduleInfo);
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0 && !user.UserTypeID.Equals(1))
            {
                ModuleQueries query = new ModuleQueries();
                query.PostModuleData(user.UserID);
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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

            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0)
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
            AuthenticatedUser user = authenticate.confirmToken();
            if (user.UserID != 0 && !user.UserTypeID.Equals(1))
            {
                ModuleQueries query = new ModuleQueries();
                query.PostVersionData(user.UserID);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
            
            
    }
    
}
