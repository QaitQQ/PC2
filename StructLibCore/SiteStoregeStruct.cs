using System.Collections.Generic;
namespace StructLibCore
{
    public class SiteStoregeStruct
    {
        public SiteStoregeStruct()
        {
            Warehouses = new List<WS>();
            ItmsCount = new List<IC>();
        }
        public List<WS> Warehouses { get; set; }
        public List<IC> ItmsCount { get; set; }
    }
    public class IC
    {
        public int Id { get; set; }
        public int WID { get; set; }
        public int C { get; set; }
    }
    public class WS
    {
        public int Id { get; set; }
        public string N { get; set; }
    }
}
