using Network;
using Server.Class.HDDClass;
using StructLibCore.Marketplace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace MGSol
{
    public class MainModel
    {
        private event Action<string, object> ChangeList;
        private MarketPlaceCash options;
        private BaseInfoPrice baseApi;
        private string Token;
        private List<ShipmentOrder> shipmentOrders;
        public SyncShipment syncShipment;
        public List<ShipmentOrder> ShipmentOrders
        {
            get
            {
                if (shipmentOrders == null)
                {
                    LoadFromFile(ref shipmentOrders, "ShipmentOrders.bin");
                }
                shipmentOrders ??= new List<ShipmentOrder>();
                return shipmentOrders;
            }
            set { shipmentOrders = value; ChangeList?.Invoke("ShipmentOrders.bin", ShipmentOrders); }
        }
        public INetClient GetClient()
        {
            Token ??= new Network.Аuthorization.SetToken().Get<string>(new WrapNetClient(baseApi.AddressPort.Address, baseApi.AddressPort.Port, Token), new object[] { baseApi.LogPass.User, baseApi.LogPass.Pass });
            return new WrapNetClient(baseApi.AddressPort.Address, baseApi.AddressPort.Port, Token);
        }
        public BaseInfoPrice BaseInfoPrice
        {
            get => baseApi;
            set { baseApi = value; ChangeList?.Invoke("baseApi.bin", baseApi); }
        }
        public MarketPlaceCash OptionMarketPlace
        {
            get => options;
            set { options = value; ChangeList?.Invoke("Option.bin", options); }
        }
        internal ObservableCollection<Control> Tabs { get; set; }
        public List<string> GetApi()
        {
            List<string> lst = new();
            foreach (APISetting item in options.APISettings)
            {
                if (item != null)
                {
                    lst.Add(item.Name);
                }
            }
            return lst;
        }
        public List<string> GetINN()
        {
            List<string> lst = new();
            foreach (InnString item in options.SellerINN)
            {
                lst.Add(item.MarketName.ToString());
            }
            return lst;
        }
        public InnString GetInnFromName(string Name) { return options.SellerINN.Find(x => x.MarketName.ToString() == Name); }
        public APISetting GetApiFromName(string Name) { return options.APISettings.Find(x => x.Name == Name); }
        public void Save() { ChangeList?.Invoke("Option.bin", options); ChangeList?.Invoke("baseApi.bin", baseApi); }
        public MainModel()
        {
            Tabs = new ObservableCollection<Control>();
            options = new MarketPlaceCash();
            LoadFromFile(ref options, "Option.bin");
            LoadFromFile(ref baseApi, "baseApi.bin");
            syncShipment = new SyncShipment(this);
            syncShipment.SetLastUpTime(options.LastUpTime);
            baseApi ??= new BaseInfoPrice();
            ChangeList += serializer.Doit;
        }
        private readonly Serializer<object> serializer = new();
        private static void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() =>
            new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null)
            {
                Object = Obj;
            }
        }
     
    }
}
