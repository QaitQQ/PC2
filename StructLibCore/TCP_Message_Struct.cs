
using System.Drawing;

namespace StructLibs
{
    [System.Serializable]
    public class TCP_CS_Obj
    {
        public object[] Code { get; set; }
        public object Obj { get; set; }
        public string Token { get; set; }
    }
    [System.Serializable]
    public class ItemNetStruct
    {
        public ItemDBStruct Item { get; set; }
        public Image Image { get; set; }
    }
}
