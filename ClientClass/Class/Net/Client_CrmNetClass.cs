using CRMLibs;

using System.Collections.Generic;

using static NetEnum.Selector;
using static NetEnum.Selector.SecondSelector;

namespace Client.Class.Net
{
    ///<summary>
    ///3 - CRM
    ///   0 - поиск партнера по имени
    ///   1 - возвращаем ивенты по партнеру
    ///   2 - добавляем партнера
    ///   3 - удаляем партнера
    ///   4 - добавляем ивент
    ///   5 - удаляем ивент
    ///   </summary>
    public class CrmNetClass
    {
        private readonly FirstSelector _FirstSelector;
        public CrmNetClass() => _FirstSelector = FirstSelector.CrmNetClass;

        public List<Partner> GetPartnerList() => (List<Partner>)new TCP_Client_GetObj(_FirstSelector).Get(EnumCrmNetClass.GetPartner);

        public List<Event> ShowEvents(int id) => (List<Event>)new TCP_Client_GetObj(_FirstSelector).Get(EnumCrmNetClass.GetEvents, id);
    }
}
