using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

using MimeKit;

using System.IO;

namespace Server.Class.Net
{
    internal class Imap_Connector
    {
        private readonly string Server_Adress;
        private readonly string Accaunt;
        private readonly string Pass;
        private readonly int Port;

        public Imap_Connector()
        {
            Server_Adress = "imap.yandex.ru";
            Accaunt = "price@sabsb.ru";
            Pass = "3011656";
            Port = 993;
        }


        public Stream GetAttach(out string Name, out string Subject)
        {
            try
            {
                using (ImapClient client = new ImapClient())
                {
                    Name = null;
                    Subject = null;
                    Imap_Rules Rules = new Imap_Rules();
                    client.Connect(Server_Adress, Port, true);
                    client.Authenticate(Accaunt, Pass);
                    IMailFolder inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    System.Collections.Generic.IList<UniqueId> uid = inbox.Search(SearchQuery.NotSeen);

                    if (uid.Count != 0)
                    {
                        MimeMessage X = inbox.GetMessage(uid[0]);
                        Subject = X.Subject;

                        foreach (MimePart item in X.Attachments)
                        {
                            Name = item.ContentDisposition.FileName;
                            if (Rules.Search_Extension(Name))
                            {
                                Stream Stream = new MemoryStream();
                                item.Content.DecodeTo(Stream);
                                inbox.AddFlags(inbox.FirstUnread, MessageFlags.Seen, true);
                                return Stream;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        inbox.AddFlags(inbox.FirstUnread, MessageFlags.Seen, true);
                    }
                    return null;
                }
            }
            catch
            {
                Name = null;
                Subject = null;
                return null;
            }
        }
    }
}
