//using StructLibs;

//using System;
//using System.Collections.Generic;

//using static NetEnum.Selector;

//namespace Client.Class.Net
//{
//    ///<summary>
//    ///2 - Работа с позициями
//    ///   0 - возвращает названия позиции из кэша(поиск)
//    ///   1 - возвращает позицию как класс с картинкой
//    ///   2 - обрабатываем прайс с почты и возвращаем Compare_Item_Resultass ItemNetClass
//    ///   </summary>
//    ///   
//    public static class Item_Flag
//    { public static bool CheckEmail; }
//    public class ItemNetClass
//    {
//        private readonly object _SendObj;
//        private readonly FirstSelector ItemSelector;

//        public object Retun_Item_And_Image() => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.GetPositionFromDB, _SendObj);
//        public List<СomparisonItems> Retun_Compare_Names() => (List<СomparisonItems>)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.WithMailDB_Selektor, _SendObj, TripleSelector.EnumMailDB_TS.GetMappedPositionName);
//        public List<СomparisonItems> Retun_New_Names() => (List<СomparisonItems>)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.WithMailDB_Selektor, _SendObj, TripleSelector.EnumMailDB_TS.GetNewPositionName);
//        public bool EditItemFromDBAndDelFromMappedList(ItemDBStruct item) => (bool)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.WithMailDB_Selektor, item, TripleSelector.EnumMailDB_TS.EditItemFromDBAndDelFromMappedList);

//        #region Api
//        public void LoadAllItemFromSite() => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.ApiSiteItem_Selektor, _SendObj, TripleSelector.ApiSiteItem.LoadAllItem);
//        public void Del_Item_From_ID(int id) => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.Del_From_DB, id);
//        public void Del_Item_From_СhangeList(object v) => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.WithMailDB_Selektor, v, TripleSelector.EnumMailDB_TS.DelItemFromСhangeList);
//        public List<СomparisonItems> Retun_Compare_RC() => (List<СomparisonItems>)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.WithMailDB_Selektor, _SendObj, TripleSelector.EnumMailDB_TS.Retun_Compare_RC);
//        public void Add_New_position_from_СhangeList(string СomparisonName) => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.Add_New_position_from_СhangeList, СomparisonName);
//        public List<СomparisonItems> CompareFromSite() => (List<СomparisonItems>)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.ApiSiteItem_Selektor, null, TripleSelector.ApiSiteItem.SiteItemCompared);
//        public List<СomparisonItems> Retun_Compare_Site() => (List<СomparisonItems>)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.ApiSiteItem_Selektor, _SendObj, TripleSelector.ApiSiteItem.GetComparedItems);
//        public bool SetPrice(int v1, double v2)
//        {
//            KeyValuePair<int, double> valuePair = new KeyValuePair<int, double>(v1, v2);
//            return (bool)new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.ApiSiteItem_Selektor, valuePair, TripleSelector.ApiSiteItem.SetPrice);
//        }
//        public void AllowAll(FillTable fillTable) => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.AllowAllСhange, new object[] { fillTable });
//        public void DelDouble() => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.DelDouble);
//        public void EditItem() => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.EditItem, _SendObj);
//        public void AllowAll(FillTable fillTable, List<string> list) => new TCP_Client_GetObj(ItemSelector).Get(SecondSelector.EnumItem.AllowAllСhange, new object[] { fillTable, list });
//        #endregion
//    }
//}