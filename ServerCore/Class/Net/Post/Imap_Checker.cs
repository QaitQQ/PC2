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
        private readonly CancellationToken token;
        public async void Start_Check()
        {
            if (Program.Cash.MailCheckFlag == false)
            {
                Program.Cash.MailCheckFlag = true;
                await Task.Factory.StartNew(() => Cycle(), token);
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
                    new Imap_Rules(Attach, _Name, _Subject).Apply_rules();
                }
                Thread.Sleep(300000);
            }
        }
    }
}
