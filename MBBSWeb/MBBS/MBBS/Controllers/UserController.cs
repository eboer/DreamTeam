//===========================================================================================
//Project: MBBS
//Description:
//   
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

using System;
using System.Web.Http;
using MBBS.Database;
using MBBS.Authentication;
using System.Web;

namespace MBBS.Controllers
{
    [RoutePrefix("api/Account")]
    public class UserController : ApiController
    {
        Authenticate authenticate = new Authenticate();

        [Route("Register")]
        public IHttpActionResult Get(string email, string firstName, string lastName, string password)
        {
            return Get(email, firstName, lastName, password, 1);
        }

        [Route("Register")]
        public IHttpActionResult Get(string email, string firstName, string lastName, string password, int userType)
        {
            int userID = 0;
            UserQueries query = new UserQueries();
            try
            {
                if (!email.Contains("stenden.com"))
                {
                    if (!email.Contains("@stenden.com") && userType.Equals(2))
                    {
                        return BadRequest("Stenden email address required for docent registration.");
                    }
                    return BadRequest("Stenden email address required for registration.");
                }
                userID = query.CreateUser(firstName, lastName, email, userType);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
            
            if (userID != 0)
            {
                try
                {
                    query.SetPassword(userID, password); 

                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            else
            {
                return InternalServerError();
            }
            return Ok("Success");
        }

        [Route("RegisterDocent")]
        public IHttpActionResult Get()
        {
            HttpContext context = HttpContext.Current;
            int userID = 0;
            UserQueries query = new UserQueries();
            try
            {
            
                if (!context.Request["email"].Contains("@stenden.com") && context.Request["userType"].Equals(2))
                {
                    return BadRequest("Stenden email address required for docent registration.");
                }
     
                userID = query.CreateUser();
                query.AddDocentData(userID);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }           
            if (userID != 0)
            {
                try
                {
                    query.SetPassword(userID, context.Request["password"]);

                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            else
            {
                return InternalServerError();
            }
            return Ok("Success");
        }

        [Route("Login")]
        public IHttpActionResult Get(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Unauthorized();
            }
            int userID = 0;
            UserQueries query = new UserQueries();
            try
            {
                userID = query.GetUserId(email);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            if(userID != 0)
            {
                string retrievedPassword = query.GetPassword(userID);

                if (password.Equals(retrievedPassword))
                {
                    
                        return Ok(authenticate.setToken(userID));
                }
            }
            return Unauthorized();
        }

        [Route("Login")]
        public IHttpActionResult Post()
        {
            string email = System.Web.HttpContext.Current.Request["email"];
            string password = System.Web.HttpContext.Current.Request["password"];
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Unauthorized();
            }
            int userID = 0;
            UserQueries query = new UserQueries();
            try
            {
                userID = query.GetUserId(email);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            if (userID != 0)
            {
                string retrievedPassword = query.GetPassword(userID);

                if (password.Equals(retrievedPassword))
                {

                    return Ok(authenticate.setToken(userID));
                }
            }
            return Unauthorized();
        }


    }

    
    
}
