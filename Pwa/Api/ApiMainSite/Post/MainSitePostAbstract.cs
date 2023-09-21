﻿using System.Net;

namespace SiteApi.IntegrationSiteApi.ApiMainSite.Post
{
    public abstract class MainSitePostAbstract : MainSiteV1Abstract
    {
        public MainSitePostAbstract(string Token, string SiteUrl) : base(Token, SiteUrl)
        {
            Method = "POST";
        }
        internal void pPost(string PostUrl, string JsonPostMessage)
        {
            HttpWebRequest httpWebRequest = GetRequest(PostUrl);
            HttpWebResponse? httpResponse = null;
            //попытка отправить
            try
            {
                using (StreamWriter streamWriter = new(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonPostMessage);
                }
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            //попытка принять
            try
            {
                using StreamReader streamReader = new(httpResponse?.GetResponseStream()!);
                result = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
        }
    }
}
