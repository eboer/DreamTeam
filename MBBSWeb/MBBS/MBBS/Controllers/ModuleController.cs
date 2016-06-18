//===========================================================================================
//Project: MBBS
//Description:
//   Controllers for module related calls.
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
    /// <summary>
    /// Controllers for docent module management
    /// </summary>
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

        /// <summary>
        /// Allows docent to add module
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// Matrix controller
    /// </summary>
    [RoutePrefix("api/Module")]
    public class MatrixController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Retrieves module matrix data
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds competency to module matrix
        /// </summary>
        /// <returns></returns>
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

    /// <summary>
    /// Returns module info
    /// </summary>
    [RoutePrefix("api/Module")]
    public class ModuleInfoController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Returns module info
        /// </summary>
        /// <param name="moduleID"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Controller for module data
    /// </summary>
    [RoutePrefix("api/Module")]
    public class ModuleDataController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Returns module data
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="subsectionID"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Posts module data to database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns list of subsection names
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Controller for modules and module section names
    /// </summary>
    [RoutePrefix("api/Module")]
    public class ModuleNamesController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Returns list of all modules
        /// </summary>
        /// <returns></returns>
        [Route("AllModules")]
        public IHttpActionResult Get()
        {           
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetAllModules());
        }

        /// <summary>
        /// Returns section and subsection names
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
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

    /// <summary>
    /// Version controller
    /// </summary>
    [RoutePrefix("api/Module")]
    public class VersionController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        /// <summary>
        /// Retrieves version information for module
        /// </summary>
        /// <param name="moduleID"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Posts new module version info
        /// </summary>
        /// <returns></returns>
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
