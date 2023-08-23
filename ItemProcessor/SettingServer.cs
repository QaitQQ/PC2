namespace Server.Class.Base
{
    public class Settings
    {
        public const int NetPort = 12001;
        public const string WordTree = @"WordTree.bin";
        public const string PriceStoragePath = @"PriceStorage.bin";
        public const string Config = @"Config.bin";
        public const string Genprice = @"generallist.bin"; // основной накопитель позиций
        public const string SiteList = @"SiteList.bin"; // накопитель сайта
        public const string NewItem = @"NewItem.bin";
        public const string SiteListСhanged = @"SiteListСhanged.bin"; // накопитель сайта
        public const string Dictionaries = @"Dictionaries.bin"; // накопитель сайта
        public const string NewItems = @"NewItems.bin"; // накопитель новый позиций
        public const string СhangedItems = @"СhangedItems.bin"; // накопитель измененных позиций
        public const string FormatedList = @"FormatedList.bin";
        public const string LogFile = @"log.txt";
        public static string SiteLink = "https://salessab.su" + "/index.php?route=api";
        public static string Key = "3EjSWpLrm8ZEZB8IQ6fQWniJ0vD6Lwn7SB7aZWm78FSlAVZ1Hqtg8MydBzOt6OSXKc2gUQiYVNRyXpLkY7lptKhj7mU1FNA6UOcXPgWQnEhCtctx4fjuhpX8QhUbtXVyy4zAbc8T0QRQMxZPhwtpwhFOkuFFLdnIwYbl8Eogfu5835IK7CzZskA6jd0MqQZNttGc64t2CToXzatu2uRAzclwEbCgJuSnsu5UC7k8BwCBxGWwpgJRqNeOHtam6xPb";
        public static string tokenfile = "token.bin";
        public const string DBContext = "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=3011656";
        public static string[] SQLnphb = { "u0219115_User", "EGikcSMIxY]v", "scp24.hosting.reg.ru", "u0219115_default" };
        public static string[] FtpSettingsImage = { "ftp://" + "ftp.u0219115.cp.regruhosting.ru/image/catalog/", "admin@salessab.su", "3011656" };
        public static string[] FtpSettingsStorege = { "ftp://" + "ftp.u0219115.cp.regruhosting.ru/", "admin@salessab.su", "3011656" };
        public static string[] ImapMail = { "price@sabsb.ru", "123456Q" };
        public static string[] ApiSettngs = { SiteLink, Key, tokenfile };
        public static string TargetsList = @"TargetsList.bin";
        public static string Marketplace = @"Marketplace.bin";
    }
}
