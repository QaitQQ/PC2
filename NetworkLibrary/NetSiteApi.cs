using HtmlAgilityPack;

using Server;
using Server.Class.IntegrationSiteApi;
using Server.Class.ItemProcessor;

using StructLibs;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Network.Item.SiteApi
{
    [Serializable]
    public class NetSiteApi : NetItem { }
    [Serializable]
    public class RenewSiteList : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);
            SiteItem SiteApi = new SiteItem(cash.ApiSiteSettngs, cash.FtpSiteSettngs);

            List<ItemChanges> Resul = new List<ItemChanges>();
            int Code = Resul.GetHashCode();
            cash.ObjBuffer.Add(new QueueOfObj() { ID = Code, Object = false });

            Message.Obj = Code;

            SiteApi.ItemListReady += RenewList;
            Task.Factory.StartNew(() => SiteApi.GetAllItemAsync());

            return Message;

            void RenewList() { cash.SiteList = SiteApi.RetunItemList(); var X = cash.ObjBuffer.Find(x => x.ID == Code); X.Object = true; }
        }
    }
    [Serializable]
    public class FixNameFromSiteList : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);


            if (((bool)X.Object))
            {

                X.Object = false;

                for (int i = 0; i < cash.SiteList.Count; i++)
                {
                    cash.SiteList[i] = new FixName(cash.Dictionaries).Fix(cash.SiteList[i]);
                    cash.SiteList[i].СomparisonName = new string[] { СomparisonNameGenerator.Get(cash.SiteList[i].Name) };
                }

                X.Object = true;

                Message.Obj = true;
            }
            else
            {
                Message.Obj = false;
            }


            return Message;
        }
    }
    [Serializable]
    public class ComparisonWithDB : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);

            if (((bool)X.Object))
            {

                X.Object = false;
                var DB_list = Db.Item.ToList();
                var cmpr = new Сompare_DB_with_SiteList(cash.SiteList, DB_list);

                cmpr.СhangeResult += AddResult;
                cmpr.StartCompare();
                Message.Obj = true;

                void AddResult()
                {
                    var Rsl = cmpr.Result;
                    X.Object = Rsl;
                }

            }
            else
            {
                Message.Obj = false;
            }


            return Message;


        }
    }
    [Serializable]
    public class ReturnComparisonWithDB : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            var X = cash.ObjBuffer.Find(x => x.ID == (int)Attach);

            if (X.Object is List<ItemChanges>)
            {
                Message.Obj = X.Object;
                cash.ObjBuffer.Remove(X);
            }
            else
            {
                Message.Obj = false;
            }

            return Message;

        }
    }
    [Serializable]
    public class UpdateSiteRC : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);
            SiteItem SiteApi = new SiteItem(cash.ApiSiteSettngs, cash.FtpSiteSettngs);
            var X = (ItemChanges)Attach;

            SiteApi.SetPrice(new KeyValuePair<int, double>(X.ItemID, (double)X.NewValue));

            Message.Obj = X.ItemID.ToString() + " Обновлено";


            return Message;
        }
    }
    [Serializable]
    public class AddNewPosition : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);
            SiteItem SiteApi = new SiteItem(cash.ApiSiteSettngs, cash.FtpSiteSettngs);
            var X = (List<SiteFieldDesc>)Attach;
            Message.Obj = SiteApi.NewProductFromFieldList(X);
            return Message;
        }
    }
    [Serializable]
    public class NetImageParser : NetSiteApi
    {
        public override TCPMessage Post(ApplicationContext Db, object Obj = null)
        {
            CashClass cash = ((CashClass)Obj);

            ImageParser parser = new ImageParser();

            var item = (ItemDBStruct)Attach;

            Image X = null;

            if (new int[] { 102, 12, 52, 102 }.Contains(item.ManufactorID))
            {
                X = parser.DSSLParseImage(item.Name);
            }
            else if (new int[] { 13 }.Contains(item.ManufactorID))
            {
                X = parser.RVIParseImage(item.Name);
            }


            Message.Obj = X;
            return Message;
        }
    }
    internal class ImageParser
    {
        private Image image = null;
        private static string LoadPage(string url)
        {
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            return result;
        }
        private static string UsePatternName(string Name, string[] Pattren)

        {
            foreach (string item in Pattren)
            {
                Regex regex = new Regex(item, RegexOptions.IgnoreCase);

                MatchCollection X = regex.Matches(Name);

                if (X.Count > 0 && X[0] != null) { Name = X[0].ToString().Replace("-", "").Trim().ToLower(); break; }
            }

            return Name;
        }
        public Image DSSLParseImage(string Name)
        {
            string SearchPage = "https://www.dssl.ru/catalog/?q=";
            string[] Pattren = { @"-\S*-", @"-\S* ", @"-\S*", @"\s\S*" };

            string OldName = Name;
            Name = UsePatternName(Name, Pattren);
            if (Name == "" || Name == " ") { Name = OldName; }

            string pageContent = LoadPage(SearchPage + Name);
            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(pageContent);
            HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//a");

            foreach (HtmlNode link in links)
            {
                if (link.OuterHtml.Contains("iblock"))
                {
                    string NС = СomparisonNameGenerator.Get(OldName);
                    string СС = Compressstr(link.OuterHtml);

                    if (link.OuterHtml.Contains(Name) || СС.Contains(NС))
                    {
                        IEnumerable<HtmlAttribute> x = link.Attributes.AttributesWithName("href");
                        string linkimg = null;
                        foreach (HtmlAttribute item in x) { linkimg = "https://www.dssl.ru" + item.Value; break; }

                        pageContent = LoadPage(linkimg);
                        document.LoadHtml(pageContent);
                        links = document.DocumentNode.SelectNodes("//li");

                        foreach (HtmlNode item in links)
                        {
                            string G = item.GetAttributeValue("id", null);
                            if (G == "photo-0")
                            {
                                HtmlNodeCollection F = item.ChildNodes;
                                foreach (HtmlNode XX in F)
                                {
                                    string T = XX.GetAttributeValue("href", null);
                                    if (T != null)
                                    {
                                        linkimg = "https://www.dssl.ru" + T;
                                    }
                                }
                            }
                        }
                        image = null;
                        WebClient client = new WebClient();
                        Stream v = client.OpenRead(linkimg);
                        image = Image.FromStream(v);
                    }
                }
            }
            return image;
        }
        public Image RVIParseImage(string Name)
        {

            string Site = @"https://rvigroup.ru";
            string SearchPage = Site + "/search/?q=";
            string[] Pattren = { @"RVI-\S* " };

            Name = UsePatternName(Name, Pattren).Replace("rvi", "");
            string pageContent = LoadPage(SearchPage + Name);
            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(pageContent);
            HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//a");

            System.Collections.Generic.IEnumerable<HtmlAttribute> x = null;

            foreach (HtmlNode link in links)
            {
                if (link.OuterHtml.Contains(Name) && link.OuterHtml.Contains("catalog"))
                {
                    x = link.Attributes.AttributesWithName("href");
                    string ItemLink = null;
                    foreach (HtmlAttribute item in x)
                    {
                        ItemLink = Site + item.Value;
                        break;
                    }

                    pageContent = LoadPage(ItemLink);
                    document.LoadHtml(pageContent);
                    links = document.DocumentNode.SelectNodes("//li");


                    foreach (HtmlNode item in links)
                    {
                        string G = item.GetAttributeValue("class", null);
                        if (G.Contains("glide__slide"))
                        {
                            HtmlNodeCollection F = item.ChildNodes;
                            foreach (HtmlNode XX in F)
                            {
                                string T = XX.GetAttributeValue("href", null);
                                if (T != null)
                                {
                                    ItemLink = Site + T;

                                    image = null;
                                    WebClient client = new WebClient();
                                    Stream v = client.OpenRead(ItemLink);
                                    image = Image.FromStream(v);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return image;
        }
        public static string Compressstr(string STR)
        {
            string Compressstr = null;
            foreach (char item in STR)
            {
                if (char.IsDigit(item) || char.IsLetter(item) || item == '+')
                {
                    Compressstr += item;
                }
            }
            return Compressstr?.ToLower();
        }
    }
}



