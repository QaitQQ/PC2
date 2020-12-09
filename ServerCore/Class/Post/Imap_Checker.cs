using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Class.Net
{
    internal class Imap_Checker
    {

        private Stream Attach;
        private string _Name;
        private string _Subject;
        private readonly CancellationTokenSource source = new CancellationTokenSource();

        public Imap_Checker(CancellationToken token)
        {
            Token = token;
        }
        public Imap_Checker() { }
        public CancellationToken Token { get; }

        public async void Start_Check()
        {
            if (Program.Cash.MailCheckFlag == false)
            {
                Program.Cash.MailCheckFlag = true;
                await Task.Factory.StartNew(() => Cycle(), Token);
            }

        }

        public void Stop_Check() => source.Cancel();

        private void Check()
        {
            Attach = null;
            Attach = new Imap_Connector().GetAttach(out string Name, out string Subject);
            _Name = Name;
            _Subject = Subject;
        }

        private void Cycle()
        {
            while (true)
            {
                Thread.Sleep(20000);
                Check();
                if (Attach != null)
                {
                    string Path = @"price_storage\\" + _Name;
                    try
                    {
                        using (FileStream fstream = new FileStream(Path, FileMode.OpenOrCreate))
                        {
                            // преобразуем строку в байты
                            byte[] array = ((MemoryStream)Attach).ToArray();
                            // запись массива байтов в файл
                            fstream.Write(array, 0, array.Length);
                        }

                        System.Collections.Generic.List<PriceStorage> PriceStorageList = Program.Cash.PriceStorageList;


                        var MatchingStorageList = PriceStorageList.FindAll(x => x.Name == _Name);


                        if (MatchingStorageList.Count == 0)
                        {
                            PriceStorageList.Add(new PriceStorage() { FilePath = Path, Name = _Name, Attributes = new System.Collections.Generic.List<string>() { _Subject } });
                        }
                        else if (MatchingStorageList.Count == 1)
                        {

                            foreach (var item in PriceStorageList)
                            {
                                if (item == MatchingStorageList[0])
                                {
                                    item.Attributes = new System.Collections.Generic.List<string>() { _Subject };
                                    item.ReceivingData = DateTime.Now;

                                    if (item.DefaultReading)
                                    {
                                        using (ApplicationContext DB = new ApplicationContext())
                                        {
                                            var X = new Network.PriceService.ReadPrice();
                                            X.Attach = item;
                                            X.Post(DB, Program.Cash);
                                        }
                                    }
                                }



                            }




                        }
                        else if (MatchingStorageList.Count > 1)
                        {
                            foreach (var item in PriceStorageList)
                            {
                                if (item == MatchingStorageList[0])
                                {
                                    item.Attributes = new System.Collections.Generic.List<string>() { _Subject };
                                    item.ReceivingData = DateTime.Now;
                                }
                            }

                            for (int i = 1; i < MatchingStorageList.Count; i++)
                            {
                                PriceStorageList.Remove(MatchingStorageList[i]);
                            }

                        }

                        Program.Cash.PriceStorageList = PriceStorageList;
                    }
                    catch (System.Exception e)
                    {

                        System.Console.WriteLine(e.Message);
                    }
                    //   new Imap_Rules(Attach, _Name, _Subject).Apply_rules()
                }
                Thread.Sleep(300000);
            }
        }
    }
}
