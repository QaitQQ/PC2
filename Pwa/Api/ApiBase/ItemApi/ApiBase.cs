using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pwa.Api.ApiBase.ItemApi
{
    public class ApiBase:IDisposable
    {
        private string token;
        private string siteUrl;

        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        public string SiteUrl
        {
            get { return siteUrl; }
            set { siteUrl = value; }
        }


        public T Go<T>() 
        {
            object result = Activator.CreateInstance(typeof(T), new object[] { token, SiteUrl });
            return (T)result;
        }

        public void Dispose()
        {
            token = null;
            siteUrl = null;
        }
    }
}
