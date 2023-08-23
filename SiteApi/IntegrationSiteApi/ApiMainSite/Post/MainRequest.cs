using Newtonsoft.Json;

namespace SiteApi.IntegrationSiteApi.ApiMainSite.Post
{

        public class MainRequest
        {
            [JsonProperty("token")]
            public string Token { get; set; }


        }

    
}
