using Server.Class.HDDClass;

using System.Collections.Generic;

namespace WindowsFormsClientLibrary.Class
{
    public static class ClientConfig
    {

        private static List<KeyValuePair<string, string>> configValue;

        public static List<KeyValuePair<string, string>> ConfigValue { get => configValue; set { configValue = value; Save();} }

        public static string GetValue(string Key) {

          var X=  ConfigValue.FindAll(x => x.Key == Key);
            if (X.Count == 0)
            {
                ConfigValue.Add(new KeyValuePair<string, string>(Key, "0"));
                return ConfigValue.Find(x => x.Key == Key).Value;
            }
            else 
            {
                return X[0].Value;
            }         

        }
        public static void SetValue(string Key,string Value)
        {

            var X= ConfigValue.FindAll(x => x.Key == Key);

            if (X.Count == 0)
            {
                ConfigValue.Add(new KeyValuePair<string, string>(Key, Value));
            }
            else 
            {
                ConfigValue.Remove(X[0]);
                ConfigValue.Add(new KeyValuePair<string, string>(Key, Value));
            }
            Save();
        }
        private static void Save()
        {
            Serializer<object> Serializer = new Serializer<object>();
            Serializer.Doit("ClientConfig.bin", configValue);
        }
    }
}
