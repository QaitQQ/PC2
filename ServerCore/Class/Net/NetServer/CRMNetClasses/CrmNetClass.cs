using CRMLibs;

using StructLibs;

using System.Collections.Generic;
using System.Linq;

using static NetEnum.Selector.SecondSelector;

namespace Server.Class.Net.NetServer
{
    internal class CrmNetClass : AbstractNetClass
    {
        public CrmNetClass(TCP_CS_Obj _Data)
        {
            Data = _Data;
            string Name = null;
            switch (Data.Code[1])
            {
                case EnumCrmNetClass.GetPartner:
                    if (Data.Obj != null) { Name = Data.Obj.ToString(); }
                    List<Partner> _list = new Class.Query.PartnersQuery().Find(Name).ToList();
                    if (_list.Count == 0) { _list.Add(new Partner() { Name = "not found" }); }
                    Data.Obj = _list;
                    break;
                case EnumCrmNetClass.GetEvents:
                    int ID_Partner = System.Convert.ToInt32(Data.Obj);
                    List<Event> Events = new Class.Query.EventQuery().FindID(ID_Partner).ToList();
                    Data.Obj = Events;
                    break;
                case EnumCrmNetClass.AddPartner:
                    break;
                case EnumCrmNetClass.DelPartner:
                    break;
                case EnumCrmNetClass.AddEvents:
                    Event Event = (Event)Data.Obj;
                    new Query.EventQuery().Add(Event);
                    break;
                case EnumCrmNetClass.DelEvents:
                    break;
                default:
                    break;
            }
        }
    }
}
