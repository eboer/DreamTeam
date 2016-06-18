//===========================================================================================
//Project: MBBS
//Description:
//   Docent object with docent information.
//
//Date: 10-6-2016
//Author: Janine Lanting
//===========================================================================================

namespace MBBS.Models
{
    public class Docent
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DocentCode { get; set; }
        public string Room { get; set; }
    }
}