using Newtonsoft.Json;

using Pricecona;

using System;
using System.Collections.Generic;

namespace Server.Class.IntegrationSiteApi
{
    internal class Map : Api
    {
        public Map(string _SiteLink) => SiteLink = _SiteLink;
        #region описание сообщений
        public class urlset : List<urlset.url>
        {
            [Serializable]
            public class url
            {
                public string loc { get; set; }
                public string lastmod { get; set; }
                public string priority { get; set; }
                public url() { }

                public url(string loc, string lastmod, string priority)
                {
                    this.loc = loc;
                    this.lastmod = lastmod;
                    this.priority = priority;
                }
            }
        }
        #endregion
        public urlset GetMap()
        {

            List<WordTable> List = new List<WordTable>();
            urlset Result = new urlset();


            try
            {
                urlset RowArrey = JsonConvert.DeserializeObject<urlset>(JsonConvert.DeserializeObject<_Message>(Response.Result.Content.ReadAsStringAsync().Result).msg.ToString());

            }
            catch (Exception e) { throw e; }

            return Result;

        }
    }
}
