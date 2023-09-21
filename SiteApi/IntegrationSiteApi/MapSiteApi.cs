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
        public class Urlset : List<Urlset.Url>
        {
            [Serializable]
            public class Url
            {
                public string Loc { get; set; }
                public string Lastmod { get; set; }
                public string Priority { get; set; }
                public Url() { }

                public Url(string loc, string lastmod, string priority)
                {
                    this.Loc = loc;
                    this.Lastmod = lastmod;
                    this.Priority = priority;
                }
            }
        }
        #endregion
        public Urlset GetMap()
        {
            Urlset Result = new Urlset();


            try
            {
                Urlset RowArrey = JsonConvert.DeserializeObject<Urlset>(JsonConvert.DeserializeObject<_Message>(Response.Result.Content.ReadAsStringAsync().Result).msg.ToString());

            }
            catch {  }

            return Result;

        }
    }
}
