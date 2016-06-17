using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using MBBS;

namespace MBBS
{
    class WebRequestHelper
    {
        // Get Data without the token (for loging/register)
        public async Task<string> getData(string url)
        {
            // Create the webrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            // Get the response
            var response = await request.GetResponseAsync();
            Stream ressStream = response.GetResponseStream();
            
            // Read the webresponse
            StreamReader reader = new StreamReader(ressStream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
