using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SiteApi.IntegrationSiteApi.APIMarket.Yandex
{
    public class Deserialize<T>
    {
        private readonly HttpWebRequest httpWebRequest;

        public Deserialize(HttpWebRequest httpWebRequest)
        {
            this.httpWebRequest = httpWebRequest;
        }
        public T Get(string result) 
        {
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            T root = JsonConvert.DeserializeObject<T>(result);
            return root;
        }

    }
}
