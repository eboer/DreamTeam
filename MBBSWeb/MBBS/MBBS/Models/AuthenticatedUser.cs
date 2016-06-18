//===========================================================================================
//Project: MBBS
//Description:
//   Contains authenticated user ID and authorization level.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBBS.Models
{
    public class AuthenticatedUser
    {
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
    }
}