
using System;
using System.IO;
using System.Text;

namespace Server.Class.HDDClass
{
    public static class Loging
    {
        public static void Add(string FileName, string text)
        {
            if (File.Exists(FileName))
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(FileName, FileMode.Append, FileAccess.Write), Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString() + text + "\n");
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write)))
                {
                    sw.WriteLine(DateTime.Now.ToString() + text + "\n");
                    sw.Close();
                }
            }
        }


    }
}
