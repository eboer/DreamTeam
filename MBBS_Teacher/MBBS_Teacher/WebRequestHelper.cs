using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace MBBS_Teacher
{
   static class WebRequestHelper
    {
        public static string getData(string url, string authorization)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", authorization);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string text = reader.ReadToEnd();
            return text;
        }
        public static string getData(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
          
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string text = reader.ReadToEnd();
            return text;
        }
    }
}
