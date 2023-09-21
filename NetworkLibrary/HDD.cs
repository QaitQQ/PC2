using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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

                    long T = openFileStream.Seek(0, SeekOrigin.Begin);
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