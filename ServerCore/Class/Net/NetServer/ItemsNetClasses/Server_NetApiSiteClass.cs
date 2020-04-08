using Server.Class.Base;
using Server.Class.IntegrationSiteApi;

using StructLibs;

using System.Collections.Generic;
using System.Threading.Tasks;

using static NetEnum.Selector;

namespace Server.Class.Net.NetServer
{
    class Server_NetApiSiteClass : AbstractNetClass
    {
        public Server_NetApiSiteClass(TCP_CS_Obj Data)
        {
            this.Data = Data;
            List<string[]> MassName = new List<string[]>();

            switch (this.Data.Code[2])
            {
                case 0:
                    break;
                case TripleSelector.ApiSiteItem.LoadAllItem:
                    RenewSiteListAsync();
                    break;
                case TripleSelector.ApiSiteItem.SiteItemCompared:

                    Program.Cash.SiteItemsСhanged = new Сompare_PriceStruct_with_DB(Program.Cash.SiteItems).StartCompare().Result;
                    break;

                case TripleSelector.ApiSiteItem.GetComparedItems:

                    List<KeyValuePair<Pricecona.PriceStruct, ItemDBStruct>> list = Program.Cash.SiteItemsСhanged.FindAll(x => x.Value != null);
                    string t = "=>";
                    foreach (KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> item in list)
                    {
                        if (item.Key.PriceRC != item.Value.PriceRC)
                        {
                            MassName.Add(new string[] { item.Key.Id.ToString(), item.Key.PriceRC.ToString() + t, item.Value.PriceRC.ToString(), item.Value.Name.ToString(), item.Value.Description });
                        }

                    }
                    this.Data.Obj = MassName;
                    break;

                case TripleSelector.ApiSiteItem.SetPrice:

                    KeyValuePair<int, double> ID_Price = (KeyValuePair<int, double>)this.Data.Obj;
                    SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);

                    bool Result = Task.Factory.StartNew(() => SiteApi.SetPrice(ID_Price)).Result;

                    this.Data.Obj = Result;

                    if (Result)
                    {
                        KeyValuePair<Pricecona.PriceStruct, ItemDBStruct> accept = Program.Cash.SiteItemsСhanged.Find(x => x.Key.Id == ID_Price.Key);
                        Program.Cash.SiteItemsСhanged.Remove(accept);
                    }

                    break;

                default:
                    break;
            }
        }
        private void RenewSiteListAsync()
        {
            SiteItem SiteApi = new SiteItem(Settings.ApiSettngs);
            ItemDeligate itemDeligate = new ItemDeligate(SiteApi.RetunItemList);
            Task.Factory.StartNew(() => SiteApi.GetAllItemAsync());
            SiteApi.ItemListReady += itemDeligate;
        }
    }
}