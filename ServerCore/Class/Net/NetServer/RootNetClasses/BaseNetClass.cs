
using Object_Description;

using Server.Class.Base;
using Server.Class.IntegrationSiteApi;

using StructLibs;

using System;

using static NetEnum.Selector;

namespace Server.Class.Net.NetServer
{
    internal class BaseNetClass : AbstractNetClass
    {
        private readonly SecondSelector.EnumBase _Code;
        internal BaseNetClass(TCP_CS_Obj Data) { this.Data = Data; _Code = (SecondSelector.EnumBase)this.Data.Code[1]; DoSwitch(); }
        private void DoSwitch()
        {
            switch (_Code)
            {
                case SecondSelector.EnumBase.GetDictionaries:
                    this.Data.Obj = Program.Cash.Dictionaries;
                    break;
                case SecondSelector.EnumBase.SetDictionaries:
                    Program.Cash.Dictionaries = (Dictionaries)this.Data.Obj;
                    break;
                case SecondSelector.EnumBase.ManufFromSite:

                    var X = new SructSite(Settings.SiteLink).ManufactorsId();
                    foreach (var item in X)
                    {
                        if (!Program.Cash.Dictionaries.Contains(item.Name))
                        {
                            var D = new DictionaryBase(item.Name, DictionaryRelate.Manufactor) { Id = Convert.ToInt32(item.Id) };
                            Program.Cash.Dictionaries.Add(D);
                        }
                    }
                    break;
                default:
                    break;

            }
        }
    }
}
