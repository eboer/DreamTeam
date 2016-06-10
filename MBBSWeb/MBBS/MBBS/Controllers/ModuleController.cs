using System.Web.Http;
using MBBS.Database;
using MBBS.Authentication;

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
            //int userID = authenticate.confirmToken();
            int userID = 1;
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
    public class AllModulesController : ApiController
    {
        [Route("AllModules")]
        public IHttpActionResult Get()
        {           
                ModuleQueries query = new ModuleQueries();
                return Ok(query.GetAllModules());
        }

        [Route("GetSectionNames")]
        public IHttpActionResult Get(string languageID)
        {
            //int userID = authenticate.confirmToken();
            int userID = 1;
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
    public class MatrixController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("GetMatrixData")]
        public IHttpActionResult Get(string moduleID, string languageID)
        {
            //int userID = authenticate.confirmToken();
            int userID = 1;
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
    public class TestingController : ApiController
    {

    }

    //new JavaScriptSerializer().Serialize(ListOfMyObject);
}
