using Newtonsoft.Json;

using Pricecona;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Class.IntegrationSiteApi
{
    public class StructSite : Api
    {
        public StructSite(string _SiteLink) => SiteLink = _SiteLink;
        #region описание сообщений
        internal class _Rows
        {

            [JsonProperty("num_rows")]
            public string Num_rows { get; set; }
            [JsonProperty("rows")]
            public object rows { get; set; }

        }
        public class Cat
        {
            [JsonProperty("category_id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("parent_id")]
            public string Parent_id { get; set; }
        }
        public class Manufactur
        {
            [JsonProperty("manufacturer_id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }

        }
        [Serializable]
        public class product_attribute
        {
            [JsonProperty("product_id")]
            public string product_id { get; set; }
            [JsonProperty("attribute_id")]
            public string attribute_id { get; set; }
            [JsonProperty("language_id")]
            public string language_id { get; set; }
            [JsonProperty("text")]
            public string text { get; set; }

        }
        [Serializable]
        public class attribute_description
        {
            [JsonProperty("attribute_id")]
            public string attribute_id { get; set; }
            [JsonProperty("language_id")]
            public string language_id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }

        }

        public class Void_result
        {
            [JsonProperty("result")]
            public string result { get; set; }

        }


        #endregion
        public Manufactur[] ManufactorsId()
        {
            var result = new List<Manufactur>();

            Post($"/RC/GetManufactors");

            _Rows RowArrey = JsonConvert.DeserializeObject<_Rows>(JsonConvert.DeserializeObject<_Message>(Response.Result.Content.ReadAsStringAsync().Result).msg.ToString());



            foreach (object item in RowArrey.rows as IEnumerable)
            {
                Manufactur D = JsonConvert.DeserializeObject<Manufactur>(item.ToString());
                result.Add(D);
            }

            return result.ToArray();
        }
        public T[] GetTable<T>(string Table)
        {
            var result = new List<T>();

            Post($"/RC/Get_" + Table);

            _Rows RowArrey = JsonConvert.DeserializeObject<_Rows>(JsonConvert.DeserializeObject<_Message>(Response.Result.Content.ReadAsStringAsync().Result).msg.ToString());

            foreach (object item in RowArrey.rows as IEnumerable)
            {
                var D = JsonConvert.DeserializeObject<T>(item.ToString());
                result.Add(D);
            }

            return result.ToArray();
        }
        public async void DeleteAttribute(int attribute_id)
        {


            Task<HttpResponseMessage> response;
            FormUrlEncodedContent Json;
            HttpClient client = new HttpClient();

            Json = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("attribute_id", attribute_id.ToString()) });
            response = client.PostAsync(SiteLink + $"/RC/deleteAttribute/", Json);

            using (StreamReader reader = new StreamReader(await response.Result.Content.ReadAsStreamAsync()))
            {

                string m = reader.ReadToEnd();
                if (m.Contains("success"))
                {

                }
            }
        }
        public async void Attribute_replacement(int old_attribute_id, int New_attribute_id)
        {
            Task<HttpResponseMessage> response;
            FormUrlEncodedContent Json;
            HttpClient client = new HttpClient();

            Json = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Old_Id", old_attribute_id.ToString()),
                new KeyValuePair<string, string>("New_Id", New_attribute_id.ToString())
            });



            while (true)
            {

            


            response = client.PostAsync(SiteLink + $"/RC/Attribute_replacement/", Json);
                using (StreamReader reader = new StreamReader(await response.Result.Content.ReadAsStreamAsync()))
                {

                    string m = reader.ReadToEnd();

                    for (int i = 0; i < m.Length; i++)
                    {
                        if (m[i] == '{')
                        {
                            m = m.Remove(0, i);
                            break;
                        }
                    }

                    var D = JsonConvert.DeserializeObject<_Message>(m);

                    var reslt = D.msg.ToString();

                    if (reslt.Contains("Duplicate"))
                    {
                        for (int i = 0; i < reslt.Length; i++)
                        {
                            if (reslt[i] == '\'')
                            {
                                reslt = reslt.Remove(0, i + 1);
                                break;
                            }
                        }
                        for (int i = 0; i < reslt.Length; i++)
                        {
                            if (reslt[i] == '\'')
                            {
                                reslt = reslt.Remove(i);
                                break;
                            }
                        }

                        var ID = reslt.Split('-')[0];


                        Json = new FormUrlEncodedContent(new[]
                          {
                           new KeyValuePair<string, string>("Old_Id", old_attribute_id.ToString()),
                           new KeyValuePair<string, string>("New_Id", New_attribute_id.ToString()),
                           new KeyValuePair<string, string>("Product_ID", ID.ToString())
                       });
                        var httpResponse = client.PostAsync(SiteLink + $"/RC/Attribute_glue/", Json);
                        using (StreamReader read = new StreamReader(await httpResponse.Result.Content.ReadAsStreamAsync()))
                        {

                            string G = read.ReadToEnd();

                        }


                    }
                    else
                    {
                        break;
                    }



                }




            }
        }
        public WordTable GetCategoryTree()
        {

            List<WordTable> List = new List<WordTable>();
            WordTable Result = new WordTable();
            Result.SetName("SiteStruct");
            Post($"/RC/GetCategory");

            try
            {
                _Rows RowArrey = JsonConvert.DeserializeObject<_Rows>(JsonConvert.DeserializeObject<_Message>(Response.Result.Content.ReadAsStringAsync().Result).msg.ToString());

                foreach (object item in RowArrey.rows as IEnumerable)
                {
                    Cat D = JsonConvert.DeserializeObject<Cat>(item.ToString());
                    List.Add(new WordTable(int.Parse(D.Id), D.Name, int.Parse(D.Parent_id)));
                }

                List<WordTable> NullCatList = List.FindAll(item => item.GetParent_id() == 0);
                foreach (WordTable item in NullCatList) { Result.AddChild(item); }
                List<WordTable> FindList = new List<WordTable> { new WordTable() };

                int i = 0;

                while (FindList.Count != 0)
                {
                    FindList = new List<WordTable>();
                    i++;
                    if (i > 5) { foreach (WordTable item in FindList) { Result.AddChild(item); } break; }

                    int[] RelultID = Result.GetAllScionsID();

                    foreach (WordTable _th in List)
                    {
                        if (_th.GetParent_id() != 0 && !RelultID.Any(x => x == _th.GetID()))
                        {
                            try { Result.GetOneScion(_th.GetParent_id()).AddChild(_th); }
                            catch { FindList.Add(_th); }
                        }
                    }
                }
            }
            catch (Exception e) { throw e; }

            return Result;
        }
    }
}
