using Pricecona;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Server
{
    internal class OpenAtFile
    {
        private readonly List<PriceStruct> List;
        private readonly string FilePath = "generallist.bin";
        public OpenAtFile()
        {
            if (File.Exists(FilePath))
            {
                Stream openFileStream = File.OpenRead(FilePath);
                BinaryFormatter deserializer = new BinaryFormatter();
                List<PriceStruct> generallist = deserializer.Deserialize(openFileStream) as List<PriceStruct>;
                openFileStream.Close();
                List = generallist;
            }
        }
        public List<PriceStruct> RetunList() => List;
    }
}
