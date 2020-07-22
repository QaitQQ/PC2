using Newtonsoft.Json;

using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Class.IntegrationSiteApi
{
    internal class Api
    {


        protected string SiteLink { get; set; }
        protected Task<HttpResponseMessage> Response;
        protected FormUrlEncodedContent Json = null;

        protected void Post(string Path) => Response = new HttpClient().PostAsync(SiteLink + Path, Json);

        #region описание сообщений
        internal class _Message

        {
            [JsonProperty("success")]
            public bool Result { get; set; }
            [JsonProperty("result")]
            public object msg { get; set; }


        }
        #endregion

    }

}
