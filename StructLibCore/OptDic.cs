using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Pricecona
{
    [Serializable]
    public class OptDic : NameValueCollection
    {

        public string FileName { get; set; }
        public string Name { get; set; }


        public void SetName(string Name) { this.Name = Name; FileName = "bin\\" + Name + ".bin"; }
        public string DictionaryiAlias { get; set; }
        public OptDic(string Name) { this.Name = Name; FileName = "bin\\" + Name + ".bin"; }
        public OptDic() { }

        public OptDic(NameValueCollection col) : base(col) { }
        public OptDic(int capacity) : base(capacity) { }
        public OptDic(IEqualityComparer equalityComparer) : base(equalityComparer) { }
        public OptDic(int capacity, IEqualityComparer equalityComparer) : base(capacity, equalityComparer) { }
        public OptDic(int capacity, NameValueCollection col) : base(capacity, col) { }
        protected OptDic(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }







}