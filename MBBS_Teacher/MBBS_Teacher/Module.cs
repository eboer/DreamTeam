using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBBS_Teacher
{
    class Module
    {
        [JsonProperty("module_id")]
        public string module_id { get; set; }
        [JsonProperty("module_name")]
        public string module_name { get; set; }
        public static Dictionary<string,string> getModuleData(string token, string moduleId)
        {
            Dictionary<string,string> moduleDetails = new Dictionary<string, string>();
            List<Subsecties> sub = new List<Subsecties>();
            string text = null;
            Task t = Task.Factory.StartNew(() => {
                text = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetSubsectionNames?languageID=en",token);
                sub = JsonConvert.DeserializeObject<List<Subsecties>>(text);

            });

            Task.WaitAll(t);

            foreach (Subsecties subsec in sub)
            {

                string moduleResond = WebRequestHelper.getData("http://mbbsweb.azurewebsites.net/api/Module/GetData?moduleID=+" + moduleId + "&subsectionID=" + subsec.SubsectionID + "&languageID=en", token);
                SubsectionData respondString = JsonConvert.DeserializeObject<SubsectionData>(moduleResond);
                moduleDetails.Add(subsec.SubsectionName, respondString.Content);
               
               

            }

            return moduleDetails;
        }

        public class SubsectionData
        {
            [JsonProperty("AuthorID")]
            public string AuthorID { get; set; }
            [JsonProperty("Content")]
            public string Content { get; set; }
            [JsonProperty("VersionNumber")]
            public string VersionNumber { get; set; }
        }
    }
    
}
