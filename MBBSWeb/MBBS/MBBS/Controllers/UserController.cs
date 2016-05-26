using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MBBS.Database;
using MBBS.Authentication;

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
                query.CreateUser(firstName, lastName, email, userType);
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }
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

       
    }

    
    
}
