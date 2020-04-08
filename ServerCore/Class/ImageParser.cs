using HtmlAgilityPack;

using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Server
{
    internal class Parser
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

            Name = UsePatternName(Name, Pattren);




            string pageContent = LoadPage(SearchPage + Name);
            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(pageContent);
            HtmlNodeCollection links = document.DocumentNode.SelectNodes(".//a");

            System.Collections.Generic.IEnumerable<HtmlAttribute> x = null;

            foreach (HtmlNode link in links)
            {


                if (link.OuterHtml.Contains(Name) && link.OuterHtml.Contains("iblock"))
                {

                    x = link.Attributes.AttributesWithName("href");
                    string linkimg = null;
                    foreach (HtmlAttribute item in x)
                    {

                        linkimg = "https://www.dssl.ru" + item.Value;
                        break;
                    }

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



    }
}
