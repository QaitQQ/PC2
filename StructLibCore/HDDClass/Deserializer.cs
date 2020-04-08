using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server.Class.HDDClass
{
    public class Deserializer<T>
    {
        public T _Obj;
        private readonly string _FilePath;
        public Deserializer(string FilePath) => _FilePath = FilePath;
        public T Doit()
        {
            if (File.Exists(_FilePath))
            {
                try
                {
                    using Stream openFileStream = File.OpenRead(_FilePath);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    _Obj = (T)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                }
                catch
                {
                }

            }
            return _Obj;
        }
    }
}
