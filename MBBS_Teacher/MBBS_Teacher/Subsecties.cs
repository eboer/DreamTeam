using Newtonsoft.Json;

namespace MBBS_Teacher
{
    class Subsecties
    {
        [JsonProperty("SubsectionID")]
        public string SubsectionID { get; set; }
        [JsonProperty("SubsectionCode")]
        public string SubsectionCode { get; set; }

        [JsonProperty("SubsectionName")]
        public string SubsectionName { get; set; }
        [JsonProperty("RequiredBool")]
        public string RequiredBool { get; set; }
        
    }
}
