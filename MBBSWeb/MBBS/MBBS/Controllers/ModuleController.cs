using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MBBS.Authentication;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Module")]
    public class ModuleController : ApiController
    {
        Authenticate authenticate = new Authenticate();
        [Route("DocentModules")]
        public IHttpActionResult Get(int userID)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO LoginCode(UserID, Password) VALUES(@userID, @password)", con);
                cmd.Parameters.AddWithValue("@userID", userID);
                SqlDataReader reader = cmd.ExecuteReader();

            }
            catch (Exception e)
            {
                con.Close();
                //return InternalServerError(e.InnerException);
                return Ok(e.ToString() + " : 3");
            }
            return Unauthorized();
        }
        //new JavaScriptSerializer().Serialize(ListOfMyObject);

    }
}
