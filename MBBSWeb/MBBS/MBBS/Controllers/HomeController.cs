using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MBBS.Authentication;
using Newtonsoft.Json.Linq;

namespace MBBS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }


    public class TestController : ApiController
    {
        public IHttpActionResult Get(int userID)
        {
            Authentication.Authenticate authorization = new Authentication.Authenticate();
            return Ok(authorization.setToken(userID));
        }

        public IHttpActionResult Get()
        {
            string listy = "{'Token':'List:'}";
            
            //try
            //{
            //    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MS_TableConnectionString"].ConnectionString);
            //    SqlCommand cmd = new SqlCommand("SELECT SubsectionName FROM Subsection WHERE SubsectionID = @general", con);
            //    int test = 12;
            //    cmd.Parameters.AddWithValue("@general", test);
            //    con.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();
                
            //    while (reader.Read())
            //    {
            //        listy += reader.GetString(0) + " : ";
            //    }
            //}
            //catch(Exception e)
            //{
            //    return InternalServerError(e.InnerException);
            //}
            
            return Ok(JObject.Parse(listy));
        }
    }
}
