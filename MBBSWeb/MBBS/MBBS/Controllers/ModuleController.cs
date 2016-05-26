﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
        //new JavaScriptSerializer().Serialize(ListOfMyObject);

    }
}
