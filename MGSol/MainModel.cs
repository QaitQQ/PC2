using Server.Class.HDDClass;
using StructLibCore.Marketplace;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MGSol
{
    public class MainModel
    {
        private event Action<string, object> СhangeList;

        private MarketPlaceCash options;
        public MarketPlaceCash Option
        {
            get => options;
            set { options = value; СhangeList?.Invoke("Option.bin", options); }
        }
        internal ObservableCollection<Control> Tabs { get; set; }
        public System.Collections.Generic.List<string> GetApi() 
        {
            var lst = new System.Collections.Generic.List<string>();

            foreach (var item in options.APISettings)
            {
                lst.Add(item.Name);
            }

            return lst;
        }

        public System.Collections.Generic.List<string> GetINN()
        {
            var lst = new System.Collections.Generic.List<string>();

            foreach (var item in options.SellerINN)
            {
                lst.Add(item.MarketName.ToString());
            }

            return lst;
        }
        public InnString GetInnFromName(string Name) { return options.SellerINN.Find(x => x.MarketName.ToString() == Name); }
        public APISetting GetApiFromName(string Name)     { return options.APISettings.Find(x => x.Name == Name); }
        public void Save() { СhangeList?.Invoke("Option.bin", options); }
        public MainModel()
        {
            Tabs = new ObservableCollection<Control>();
            options = new MarketPlaceCash();
            LoadFromFile(ref options, "Option.bin");
            СhangeList += Serializer.Doit;
        }
        private Serializer<object> Serializer = new Serializer<object>();
        private void LoadFromFile<T>(ref T Object, string Path)
        {
            T Obj = Task.Run(() => new Deserializer<T>(Path).Doit()).Result;
            if (Obj != null)
            {
                Object = Obj;
            }
        }
    }
}
