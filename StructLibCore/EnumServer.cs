namespace NetEnum
{
    public static class Selector
    {
        public enum FillTable { СhangedItemsTable, NewItemTable, СhangedSiteTable }
        public enum FirstSelector
        {
            Ok_code,
            Аuthorization,
            ItemNetClass,
            CrmNetClass,
            BaseNetClass
        }
        public static class SecondSelector
        {
            public enum EnumCrmNetClass
            {
                GetPartner, GetEvents, AddPartner, DelPartner, AddEvents, DelEvents
            }
            public enum EnumBase
            {
                GetDictionaries,
                SetDictionaries,
                ManufFromSite
            }
            public enum EnumАuthorization
            {
                UserNames,
                GetToken,
            }


            public enum EnumItem
            {
                ItemsSearch,
                GetPositionFromDB,
                WithMailDB_Selektor,
                ApiSiteItem_Selektor,
                EditPositionFromDB,
                Del_From_DB,
                Add_New_position_from_СhangeList,
                AllowAllСhange,
                DelDouble
            }

        }
        public static class TripleSelector
        {
            public enum ApiSiteItem
            {
                LoadAllItem, SiteItemCompared, GetComparedItems, SetPrice
            }
            public enum EnumMailDB_TS
            {
                // 1 проверяем почту
                // 2 имена сопоставленных 
                // 3 имена новых
                // 4 сопоставленная пара по имена
                // 5 новая позиция по имени

                CheckMail,
                GetMappedPositionName,
                GetNewPositionName,
                SearchMappedPairName,
                SearchNewPairName,
                EditItemFromDBAndDelFromMappedList,
                DelItemFromСhangeList
            }
        }
    }
}
