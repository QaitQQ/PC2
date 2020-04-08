using Object_Description;

using Pricecona;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Server.Class.Net
{
    internal abstract class Imap_Handler
    {


        internal IDictionaryPC _Dictionary;
        public Imap_Handler() { }
        public virtual void Process(Stream Attach, string FileName) { }

    }

    internal class Imap_Handler_Storege : Imap_Handler
    {
        public Imap_Handler_Storege(IDictionaryPC Dictionary) => _Dictionary = Dictionary;
        public override void Process(Stream Attach, string FileName)
        {

        }

    }

    internal class Imap_Handler_Price : Imap_Handler
    {


        private List<PriceStruct> _Result;

        private event Action СhangeResult;
        public Imap_Handler_Price(IDictionaryPC Dictionary) { _Dictionary = Dictionary; СhangeResult += Comparer; }
        public override async void Process(Stream Attach, string FileName) => await Task.Factory.StartNew(() => GetList(Attach, FileName));
        public List<PriceStruct> Result
        {
            get => _Result;
            set
            {
                if (value != null)
                {
                    _Result = value;
                    if (_Result.Count != 0)
                    {
                        СhangeResult?.Invoke();
                    }
                }
            }
        }

        private void GetList(Stream Attach, string FileName)
        {
            using XLS.XLS_TO_LIST X = new XLS.XLS_TO_LIST(Attach);
            Result = X.Read(null, FileName, _Dictionary);
        }
        private void Comparer() => Program.Cash.СhangedItems = new Сompare_PriceStruct_with_DB(Result).StartCompare().Result;

    }








}
