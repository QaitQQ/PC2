using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace Server
{
    internal class FTP
    {
        private readonly string FtpUri;
        private readonly string FtpUser;
        private readonly string FtpPass;
        public FTP(string[] FtpSetting) { this.FtpUri = FtpSetting[0]; this.FtpUser = FtpSetting[1]; this.FtpPass = FtpSetting[2]; }
        public void FTPUploadFile(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = FtpUri + fileInf.Name;
            FtpWebRequest reqFTP;
            // Создаем объект FtpWebRequest
            reqFTP = FtpWebRequest.Create(new Uri(FtpUri + fileInf.Name)) as FtpWebRequest;
            // Учетная запись
            reqFTP.Credentials = new NetworkCredential(FtpUser, FtpPass);
            reqFTP.KeepAlive = false;
            // Задаем команду на закачку
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Тип передачи файла
            reqFTP.UseBinary = true;
            // Сообщаем серверу о размере файла
            reqFTP.ContentLength = fileInf.Length;
            // Буффер в 2 кбайт
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            // Файловый поток
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                // Читаем из потока по 2 кбайт
                contentLen = fs.Read(buff, 0, buffLength);
                // Пока файл не кончится
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Закрываем потоки
                strm.Close();
                fs.Close();
            }
            catch (Exception) { throw; }
        }
        public void FTPUploadStream(MemoryStream stream, string FileName)
        {
            string uri = FtpUri + FileName;
            FtpWebRequest reqFTP;
            // Создаем объект FtpWebRequest
#pragma warning disable SYSLIB0014 // Тип или член устарел
            reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(FtpUri + FileName));
#pragma warning restore SYSLIB0014 // Тип или член устарел
                                  // Учетная запись
            reqFTP.Credentials = new NetworkCredential(FtpUser, FtpPass);
            reqFTP.KeepAlive = false;
            // Задаем команду на закачку
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            // Тип передачи файла
            reqFTP.UseBinary = true;
            // Сообщаем серверу о размере файла
            reqFTP.ContentLength = stream.Length;
            // Буффер в 2 кбайт
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                // Читаем из потока по 2 кбайт
                contentLen = stream.Read(buff, 0, buffLength);
                // Пока файл не кончится
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = stream.Read(buff, 0, buffLength);
                }
                // Закрываем потоки
                strm.Close();
                stream.Close();
            }
            catch (Exception) { throw; }
        }

    }
}
