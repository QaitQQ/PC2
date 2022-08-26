using Network;

using Server.Class.HDDClass;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace MGSol
{
    public class MainModel
    {
        private event Action<string, object> СhangeList;
        private MarketPlaceCash options;
        private BaseInfoPrice baseApi;
        private string Token;
        public INetClient GetClient()
        {
            if (Token== null)
            {
                Token = new Network.Аuthorization.SetToken().Get<string>(new WrapNetClient(baseApi.AddressPort.Address, baseApi.AddressPort.Port, Token), new object[] { baseApi.LogPass.User, baseApi.LogPass.Pass });
            }
            return new WrapNetClient(baseApi.AddressPort.Address, baseApi.AddressPort.Port, Token); 
        }
        public BaseInfoPrice BaseInfoPrice
        {
            get => baseApi;
            set { baseApi = value; СhangeList?.Invoke("baseApi.bin", baseApi); }
        }
        public MarketPlaceCash OptionMarketPlace
        {
            get => options;
            set { options = value; СhangeList?.Invoke("Option.bin", options); }
        }
        internal ObservableCollection<Control> Tabs { get; set; }
        public List<string> GetApi()
        {
            List<string> lst = new System.Collections.Generic.List<string>();
            foreach (APISetting item in options.APISettings)
            {
                lst.Add(item.Name);
            }
            return lst;
        }
        public List<string> GetINN()
        {
            List<string> lst = new System.Collections.Generic.List<string>();
            foreach (InnString item in options.SellerINN)
            {
                lst.Add(item.MarketName.ToString());
            }
            return lst;
        }
        public InnString GetInnFromName(string Name) { return options.SellerINN.Find(x => x.MarketName.ToString() == Name); }
        public APISetting GetApiFromName(string Name) { return options.APISettings.Find(x => x.Name == Name); }
        public void Save() { СhangeList?.Invoke("Option.bin", options); СhangeList?.Invoke("baseApi.bin", baseApi); }
        public MainModel()
        {
            Tabs = new ObservableCollection<Control>();
            options = new MarketPlaceCash();
            LoadFromFile(ref options, "Option.bin");
            LoadFromFile(ref baseApi, "baseApi.bin");
            if (baseApi == null)
            {
                baseApi = new BaseInfoPrice();
            }
            СhangeList += Serializer.Doit;
        }
        private Serializer<object> Serializer = new Serializer<object>();
        private static void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null)
            {
                Object = Obj;
            }
        }

    }


}
