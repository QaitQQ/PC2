using Newtonsoft.Json;

using Pricecona;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server.Class.IntegrationSiteApi
{
    internal class SructSite : Api
    {
        public SructSite(string _SiteLink) => SiteLink = _SiteLink;
        #region описание сообщений
        internal class _Rows
        {

            [JsonProperty("num_rows")]
            public string Num_rows { get; set; }
            [JsonProperty("rows")]
            public object rows { get; set; }

        }
        internal class _Cat
        {
            [JsonProperty("category_id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("parent_id")]
            public string Parent_id { get; set; }
        }
        internal class Manufactur
        {
            [JsonProperty("manufacturer_id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }

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
                    _Cat D = JsonConvert.DeserializeObject<_Cat>(item.ToString());
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
