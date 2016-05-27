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
        //getData using the token
        public static string getData(string url, string authorization)
        {
            //create the httpwebrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //add the token in the header
            request.Headers.Add("Authorization", authorization);
            //get the webresponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            //read the webresponse
            StreamReader reader = new StreamReader(resStream);
            string text = reader.ReadToEnd();
            return text;
        }
        //getData without the token (for loging/register)
        public static string getData(string url)
        {
            //create the webrequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //get the respnse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            //read the webresponse
            StreamReader reader = new StreamReader(resStream);
            string text = reader.ReadToEnd();
            return text;
        }
        //send data using POST
        public static string sendPostData(string url, string authorization, string moduleId, string subId, string langId, string content)
        {
            try
            {
                //create the request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //add the token
                request.Headers.Add("Authorization", authorization);
                //create the post data
                var postMsg = "moduleId=" + moduleId;
                postMsg += "&subsectionId=" + subId;
                postMsg += "&languageId=" + langId;
                postMsg += "&content=" + content;
                //encode the post data
                var data = Encoding.ASCII.GetBytes(postMsg);
                //set request method to most
                request.Method = "POST";
                //set the content type and length
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                //write the post data
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //get the response
                var response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (WebException exept)
            {
                return exept.ToString();
            }
            

        }
    }
}
