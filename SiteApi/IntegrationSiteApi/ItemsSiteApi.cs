using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pricecona;
using StructLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Server.Class.IntegrationSiteApi
{
    public class SiteItem
    {
        /// <summary>
        /// Apilink = Setting[0]; Key = Setting[1]; tokenfile = Setting[2];
        /// </summary>
        public SiteItem(string[] ApiSetting, string[] FtpSetting) {  Apilink = ApiSetting[0]; Key = ApiSetting[1]; FilePath = ApiSetting[2]; this.FtpSetting = FtpSetting; GetToken(); }
        private readonly string Apilink;
        private readonly string Key;
        private readonly string FilePath;
        private readonly string[] FtpSetting;
        private string token = "";
        private List<int> product_id = new List<int>();
        private List<ItemDBStruct> ItemList;
        private List<Dictionary<string, string>> mapList = new List<Dictionary<string, string>>();
        private static readonly HttpClient client = new HttpClient();
        private bool Ok;
        private int product_id_in;
        private readonly List<KeyValuePair<int, bool>> Result = new List<KeyValuePair<int, bool>>();
        public event Action ItemListReady;
        public List<ItemDBStruct> GetItem(List<int> product_id)
        {
            this.product_id = product_id;
            GetProduct();
            foreach (Dictionary<string, string> item in mapList)
            {
                string f = item["name"];
                if (item["name"] != " None")
                {
                    ItemList.Add(
                        new ItemDBStruct
                        {
                            Name = item["name"],
                            Id = Convert.ToInt32(item["product_id"]),
                            Description = item["description"],
                            PriceRC = Convert.ToDouble(item["base_price"].Replace('.', ','))
                        });
                }
            }
            return ItemList;
        }
        private void GetAllItem()
        {
            
            GetAllProduct();

            double RC = 0;
            ItemList = new List<ItemDBStruct>();
            foreach (Dictionary<string, string> item in mapList)
            {
                string f = item["name"];
                if (item["name"] != " None")
                {

                    Dictionary<string, string> Item = item;


                    RC = Convert.ToDouble(item["base_price"].Replace('.', ','));



                    ItemDBStruct Product = new ItemDBStruct
                    {
                        Name = item["name"],
                        Id = Convert.ToInt32(item["product_id"]),
                        PriceRC = RC
                    };
                    ItemList.Add(Product);
                }
            }
            mapList = new List<Dictionary<string, string>>();
      

        }
        public async Task GetAllItemAsync()
        {
            await Task.Factory.StartNew(() => GetAllItem());
            ItemListReady?.Invoke();
        }
        public List<ItemDBStruct> RetunItemList()
        {
            return ItemList;
        }
        public bool SetPrice(KeyValuePair<int, double> ID_Price)
        {
            Ok = false;

            FormUrlEncodedContent Json = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("product_id", ID_Price.Key.ToString()), new KeyValuePair<string, string>("base_price", ID_Price.Value.ToString().Replace(',', '.')) });
            Task<HttpResponseMessage> response = client.PostAsync(Apilink + $"/RC/setPrice/&token={token}", Json);

            SetPrice(response);

            return Ok;

        }
        public int NewProduct(PriceStruct Item)
        {
            product_id_in = 0; // зануляем возвращаемую переменную
            string Manufactorid = ManufactorID(Item); // определяем ID производителя

            List<Detail> Text = null; // переменная для деталий
            string str = null;// переменная для отформатированных деталий для передачи
            string category = null;// переменная для отформатированных категорий для передачи
            string Imagelink = null; // переменная для отформатированного линка на картинку
            List<int> CategoryID = new List<int>(); //переменная для неотформатированнх категорий.

            if (Item.Details == null) // ищем детали если их нету
            {// TextAnalytic.DetailFind analytic = new TextAnalytic.DetailFind(Item.Description); Text = analytic.GetDetales();
            }
            else
            { Text = Item.Details; }

            foreach (Detail Product in Text) // форматируем переменные в строку
            { str = str + Product.GetID().ToString() + "|" + Product.GetDetailValues() + ";"; }
            str = str.Remove(str.Length - 1, 1);

            if (Item.Imagelink != null) { FTP fTP = new FTP(FtpSetting); fTP.FTPUploadFile(Item.Imagelink); } //подсовываем на фтп фотку из прайса если есть
            if (Item.Imagelink != null) { Imagelink = @"catalog/" + Item.Imagelink.Replace(".//pic//", ""); } // форматируем адресс

            //ищем категории
            if (Item.CategoryID != null)
            {
                for (int i = 0; i < Item.CategoryID.Count; i++)
                {
                    category = category + "|" + Item.CategoryID[i].ToString();
                }
                category = category.Trim('|');

            }
            // форматируем категории
            // собираем сообщение для отправки
            FormUrlEncodedContent Json = new FormUrlEncodedContent(new[]
            {

                new KeyValuePair<string, string>("model", Item.Name.ToString()),
                new KeyValuePair<string, string>("sku", 0.ToString()),
                new KeyValuePair<string, string>("manufacturer_id", Manufactorid),
                new KeyValuePair<string, string>("price", Item.PriceRC.ToString()),
                new KeyValuePair<string, string>("name", Item.Name.ToString()),
                new KeyValuePair<string, string>("description", Item.Description.ToString()),
                new KeyValuePair<string, string>("attribute", str),
                new KeyValuePair<string, string>("image", Imagelink),
                new KeyValuePair<string, string>("СomparisonName", Item.СomparisonName.Replace(" ","-").Replace(".","-")),
                new KeyValuePair<string, string>("Category", category)
            }
            );

            Task<HttpResponseMessage> response = client.PostAsync(Apilink + $"/RC/newProduct/&token={token}", Json);

            NewProduct(response); // отправляем

            return product_id_in;

        }
        public int NewProductFromFieldList(List<SiteFieldDesc> fieldDescs)
        {
            product_id_in = 0; // зануляем возвращаемую переменную
            // Имя
            string Name = fieldDescs.First(x => x.Type == FieldType.Name).Desc;
            // Описание
            string Description = fieldDescs.First(x => x.Type == FieldType.Description).Desc;
            // Цена
            string Price = fieldDescs.First(x => x.Type == FieldType.Price).Desc;
            // Цена
            string СomparisonName = fieldDescs.First(x => x.Type == FieldType.СomparisonName).Desc;
            // Производитель
            string Manufactor = fieldDescs.First(x => x.Type == FieldType.Manufactor).Desc;
            // Категории
            string Categories = GetSTR(fieldDescs.FindAll(x => x.Type == FieldType.Category))?.Trim('|');
            // Атребуты
            string Attributes = GetSTR(fieldDescs.FindAll(x => x.Type == FieldType.Attribute));

            //Картинка
            string Imagelink = null;
            var Image = (System.Drawing.Image)fieldDescs.Find(x => x.Type == FieldType.Image)?.Obj;

            if (Image!= null)
            {
                FTP fTP = new FTP(FtpSetting);
                var stream = new System.IO.MemoryStream();
                Image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                fTP.FTPUploadStream(stream, СomparisonName + ".Png");
                Imagelink = @"catalog/" + СomparisonName + ".Png";

            }          
            // собираем сообщение для отправки
            FormUrlEncodedContent Json = new FormUrlEncodedContent(new[]
            {

                new KeyValuePair<string, string>("model", Name),
                new KeyValuePair<string, string>("sku", 0.ToString()),
                new KeyValuePair<string, string>("manufacturer_id", Manufactor),
                new KeyValuePair<string, string>("price", Price),
                new KeyValuePair<string, string>("name", Name),
                new KeyValuePair<string, string>("description", Description),
                new KeyValuePair<string, string>("attribute", Attributes),
                new KeyValuePair<string, string>("image", Imagelink),
                new KeyValuePair<string, string>("ComparisonName", СomparisonName),
                new KeyValuePair<string, string>("Category", Categories)
            }
            );

            var response = client.PostAsync(Apilink + $"/RC/newProduct/&token={token}", Json);
            NewProduct(response);
            return product_id_in;


           static string GetSTR(List<SiteFieldDesc> fieldDescs) 
            {

                string Str = null;

                foreach (var item in fieldDescs)
                {
                    if (item.Type == FieldType.Attribute)
                    {
                        Str = Str + item.Id + "|" + item.Desc + ";";
                    }
                    else
                    {
                        Str = Str + item.Id + "|";
                    }
                  
                }


                return Str;


            }



        }
        public List<KeyValuePair<int, bool>> DeleteItem(List<int> product_id)
        {


            this.product_id = product_id;
            DeleteProduct();
            return Result;



        }
        private async void GetToken()
        {
            if (File.Exists(FilePath))
            {

                Stream openFileStream = File.OpenRead(FilePath);
                if (openFileStream.Length != 0)
                {
                    token = (string)new BinaryFormatter().Deserialize(File.OpenRead(FilePath));
                    openFileStream.Close();

                }
            }
            else
            {
                Task<HttpResponseMessage> response;

                FormUrlEncodedContent Json = new FormUrlEncodedContent(new[] {
            new KeyValuePair<string, string>("key",Key)});
                try
                {
                    response = client.PostAsync(Apilink + $"/login", Json);
                    using (StreamReader reader = new StreamReader(await response.Result.Content.ReadAsStreamAsync()))
                    {
                        if (JsonConvert.DeserializeObject(await reader.ReadToEndAsync()) is System.Collections.IEnumerable collection)
                        {
                            foreach (object item in collection)
                            {
                                if (item.ToString().Contains("token"))
                                {
                                    System.Collections.IEnumerable xx = item as System.Collections.IEnumerable;
                                    foreach (object X in xx) { token = X.ToString(); }

                                }
                            }
                        }
                    }
                }
                catch (Exception) { }
            }
        }
        private async void GetProduct()
        {

            Task<HttpResponseMessage> response;
            FormUrlEncodedContent Json;

            try
            {
                Json = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("product_id", Formatter(product_id)) });
                response = client.PostAsync(Apilink + $"/RC/getProduct/&token={token}", Json);
                JEnumerable<JToken> PositionString;


                using (StreamReader reader = new StreamReader(await response.Result.Content.ReadAsStreamAsync()))
                {
                    JObject collection = (JObject)(JsonConvert.DeserializeObject(await reader.ReadToEndAsync()));
                    foreach (JToken item in collection.Children().Children())
                    {
                        PositionString = item.Children();
                    }

                    foreach (JToken item in PositionString)
                    {
                        Dictionary<string, string> MapDic = new Dictionary<string, string>();
                        foreach (JToken str in item.Children())
                        {
                            string[] PosSplit = str.ToString().Replace(Convert.ToChar(34).ToString(), "").Split(':');
                            MapDic.Add(PosSplit[0], PosSplit[1]);
                        }
                        mapList.Add(MapDic);
                    }

                }
            }
            catch (Exception) { }
        }
        private async void GetAllProduct()
        {

            Task<HttpResponseMessage> response;
            FormUrlEncodedContent Json;


            Json = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("product_id", "300") });
            response = client.PostAsync(Apilink + $"/RC/getAllProduct/&token={token}", Json);
            JEnumerable<JToken> PositionString;
            Stream D = await response.Result.Content.ReadAsStreamAsync();

            using (StreamReader reader = new StreamReader(D))
            {
                JObject collection = (JObject)(JsonConvert.DeserializeObject(await reader.ReadToEndAsync()));
                foreach (JToken item in collection.Children().Children())
                {
                    PositionString = item.Children();
                }

                foreach (JToken item in PositionString)
                {
                    Dictionary<string, string> MapDic = new Dictionary<string, string>();
                    foreach (JToken str in item.Children())
                    {
                        string[] PosSplit = str.ToString().Replace(Convert.ToChar(34).ToString(), "").Split(':');
                        MapDic.Add(PosSplit[0], PosSplit[1]);
                    }
                    mapList.Add(MapDic);
                }

            }

        }
        private async void SetPrice(Task<HttpResponseMessage> message)
        {
            using (StreamReader reader = new StreamReader(await message.Result.Content.ReadAsStreamAsync()))
            {

                JObject collection = (JObject)(JsonConvert.DeserializeObject(await reader.ReadToEndAsync()));

                IJEnumerable<JToken> m = collection.Values();
                foreach (JToken item in m)
                {
                    bool Qs = Convert.ToBoolean(item.ToString().Replace("{", "").Replace("}", ""));
                    if (Qs)
                    {
                        Ok = Qs;
                    }
                }
            }


        }
        private async void NewProduct(Task<HttpResponseMessage> message)
        {
            using (StreamReader reader = new StreamReader(await message.Result.Content.ReadAsStreamAsync()))
            {
                var B = await reader.ReadToEndAsync();
                try
                {
                    JObject collection = (JObject)JsonConvert.DeserializeObject(B);

                    IJEnumerable<JToken> m = collection.Values();
                    foreach (JToken item in m)
                    {
                        int product_id_in = Convert.ToInt32(item.ToString().Replace("{", "").Replace("}", ""));
                        if (product_id_in != 0)
                        {
                            this.product_id_in = product_id_in;
                        }
                    }
                }
                catch (Exception e)
                {

              
                }
              

            }


        }
        private string ManufactorID(PriceStruct Item)
        {
            string Manufactor = null;
            string Manufactorid = "308";

            if (Item.Manufactor == null)
            {
                //  Manufactor = new TextAnalytic.ManufactorFind(Item).Find();
            }
            else
            {
                Manufactor = Item.Manufactor;
            }


            //foreach (string item in Settings.Manufactorid.AllKeys)
            //{
            //    if (item.ToUpper() == Manufactor.ToUpper())
            //    {
            //        Manufactorid = Settings.Manufactorid.GetValues(item)[0];
            //    }
            //}

            return Manufactorid;

        }
        private async void DeleteProduct()
        {


            Task<HttpResponseMessage> response;
            FormUrlEncodedContent Json;

            try
            {
                Json = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("product_id", Formatter(product_id)) });
                response = client.PostAsync(Apilink + $"/RC/deleteProduct/&token={token}", Json);

                using (StreamReader reader = new StreamReader(await response.Result.Content.ReadAsStreamAsync()))
                {

                    string m = reader.ReadToEnd();
                    int product_id = 0;
                    if (m.Contains("success"))
                    {
                        char[] F = { (char)34, (char)44, };
                        string FF = null;
                        char[] GG = m.ToCharArray();

                        foreach (char item in GG)
                        {
                            if (item != 123 && item != 34 && item != 91 && item != 93 && item != 129 && item != 125 && item != 58) { FF = FF + item; }
                            if (item == 58) { FF = FF + (char)44; }
                        }
                        bool SumResult = true;

                        string[] mass = FF.Split(F, StringSplitOptions.RemoveEmptyEntries);
                        int X = 0;

                        for (int i = 0; i < mass.Length; i++)
                        {
                            if (mass[i] == "product_id") { if (X == 0) { product_id = Convert.ToInt32(mass[i + 1]); X++; continue; } else { Result.Add(new KeyValuePair<int, bool>(product_id, SumResult)); product_id = Convert.ToInt32(mass[i + 1]); SumResult = true; } }
                            if (mass[i] == "true" || mass[i] == "false") { SumResult = SumResult & Convert.ToBoolean(mass[i]); }
                        }
                        Result.Add(new KeyValuePair<int, bool>(product_id, SumResult));
                    }
                }
            }
            catch (Exception) { }


        }
        private string Formatter(List<int> str)
        {
            string formstr = null;

            int i = 0;

            while (i < str.Count - 1)
            {
                formstr = formstr + str[i] + ";";
                i++;
            }
            formstr = formstr + str[str.Count - 1];

            return formstr;
        }
    }

}
