using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;

using MimeKit;

using Object_Description;

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;

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
        public bool Search_Extension(string Value)
        {
            IDictionaryPC Extension_list = Program.Cash.Dictionaries.Get("xls");
            return Extension_list.Сontain(Value);
        }


        public Stream GetAttach(out string Name, out string Subject)
        {
            try
            {
                using (ImapClient client = new ImapClient())
                {
                    Name = null;
                    Subject = null;
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
                            if (Search_Extension(Name))
                            {
                                Stream Stream = new MemoryStream();
                                item.Content.DecodeTo(Stream);
                                inbox.AddFlags(inbox.FirstUnread, MessageFlags.Seen, true);
                                return Stream;
                            }
                            else if (Name.ToLower().Contains(".zip"))
                            {
                                Stream Stream = new MemoryStream();
                                item.Content.DecodeTo(Stream);
                                inbox.AddFlags(inbox.FirstUnread, MessageFlags.Seen, true);

                                var Unzip = new ZipArchive(Stream);
                                var Z = Unzip.Entries;

                                foreach (ZipArchiveEntry Y in Z)
                                {
                                    if (Y.FullName.ToLower().Contains(".xls"))
                                    {
                                        Y.ExtractToFile("_extract_file", true);
                                        Name = Y.FullName;
                                        using Stream fileStream = new FileStream("_extract_file", FileMode.Open);
                                        Stream = new MemoryStream();
                                        fileStream.CopyTo(Stream);
                                        return Stream;
                                    }

                                }

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
