
using System;
using System.Collections.Generic;

namespace Server
{
    public static class TargetDictionary
    {
        private static Dictionary<string, Action> dictionarys;

        public static Dictionary<string, Action> Dictionarys
        {
            get { return dictionarys; }
            set { dictionarys = value; }
        }

        static TargetDictionary()
        {
            dictionarys = new Dictionary<string, Action>();

        }
    }
}

