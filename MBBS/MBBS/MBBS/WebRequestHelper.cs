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
        // Get Data using the token
        public string getData(string url, string authorization)
        {
            return url;
        }

        // Get Data without the token (for loging/register)
        public async Task<string> getData(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = await request.GetResponseAsync();
            Stream ressStream = response.GetResponseStream();
            
            // Read the webresponse
            StreamReader reader = new StreamReader(ressStream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
