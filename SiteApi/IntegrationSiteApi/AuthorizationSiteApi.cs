using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Class.IntegrationSiteApi
{
    internal class Authorization
    {
        internal class AuthorizationToken
        {
            [JsonProperty("error")]
            public object Error { get; set; }
            [JsonProperty("success")]
            public string Result { get; set; }
            [JsonProperty("token")]
            public string Token { get; set; }
        }

        public string Token
        {
            private set => Token = value;

            get
            {

                if (Token != null)
                {
                    return Token;
                }
                else
                {
                    LoadToken();
                    if (Token == null) { GetToken(); }
                    if (!CheckToken(Token)) { GetToken(); }
                    return Token;
                }
            }
        }
        private readonly string SiteLink;
        private readonly string Key;

        public Authorization(string SiteLink, string Key)
        {
            this.SiteLink = SiteLink;
            this.Key = Key;
        }
        private string LoadToken()
        {
            if (File.Exists("Token.txt"))
            {
                string[] Array = File.ReadAllLines("Token.txt");
                return Array[Array.Length];

            }
            return null;

        }
        private string GetToken()
        {

            string Token = null;

            Task<HttpResponseMessage> response;

            FormUrlEncodedContent Json = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("key",Key)});
            try
            {
                response = new HttpClient().PostAsync(SiteLink + $"/login", Json);

                AuthorizationToken key = JsonConvert.DeserializeObject<AuthorizationToken>(response.Result.Content.ReadAsStringAsync().Result);

                Token = key.Token;

                if (key.Error != null)
                {
                    throw new Exception(key.Error.ToString());
                }
            }
            catch 
            {
            }

            SaveToken(Token);

            return Token;

        }
        private void SaveToken(string Token)

        {

            if (File.Exists("Token.txt") != true)
            {
                using (StreamWriter sw = new StreamWriter(new FileStream("Token.txt", FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine(Token);
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(new FileStream("Token.txt", FileMode.Open, FileAccess.Write)))
                {
                    (sw.BaseStream).Seek(0, SeekOrigin.End);
                    sw.WriteLine(Token);
                }
            }
        }
        private bool CheckToken(string Token)
        {
            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> response = client.PostAsync(SiteLink + $"/RC/CheckToken", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("token", Token) }));
            AuthorizationToken key = JsonConvert.DeserializeObject<AuthorizationToken>(response.Result.Content.ReadAsStringAsync().Result);
            return Convert.ToBoolean(key.Result);
        }
    }

}
