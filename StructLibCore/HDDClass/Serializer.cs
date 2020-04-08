using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server.Class.HDDClass
{
    public class Serializer<T>
    {
        public T _Obj;
        private string _FilePath;
        public Serializer()
        {

        }

        public void Doit(string FilePath, T Obj)
        {
            _Obj = Obj;
            _FilePath = FilePath;
            BinaryFormatter Serializer = new BinaryFormatter();
            try
            {
                using FileStream fs = new FileStream(_FilePath, FileMode.OpenOrCreate);
                Serializer.Serialize(fs, _Obj);
            }
            catch (System.Exception E)
            {

                System.Console.WriteLine(E.ToString());
            }

        }
    }
}
