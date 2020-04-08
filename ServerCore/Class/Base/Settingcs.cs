namespace Server.Class.Base
{


    internal static class Settings
    {

        public const int NetPort = 12001;
        public const string WordTree = @"WordTree.bin";
        public const string Config = @"Config.bin";
        public const string Genprice = @"generallist.bin"; // основной накопитель позиций
        public const string SiteList = @"SiteList.bin"; // накопитель сайта
        public const string SiteListСhanged = @"SiteListСhanged.bin"; // накопитель сайта
        public const string Dictionaries = @"Dictionaries.bin"; // накопитель сайта
        public const string NewItems = @"NewItems.bin"; // накопитель новый позиций
        public const string СhangedItems = @"СhangedItems.bin"; // накопитель измененных позиций
        public const string FormatedList = @"FormatedList.bin";
        public const string LogFile = @"log.txt";
        public static string[] SQLnphb = { "u0219115_User", "EGikcSMIxY]v", "scp24.hosting.reg.ru", "u0219115_default" };
        public static string FtpUri = "ftp://" + "ftp.u0219115.cp.regruhosting.ru/image/catalog/";
        public static string[] FtpUP = { "admin@salessab.su", "3011656" };
        public static string[] ImapMail = { "price@sabsb.ru", "123456Q" };
        public static string SiteLink = "https://salessab.su" + "/index.php?route=api";
        public static string Key = "gFhNIqYweY5o5mzLEEcDsjtmUt2vWphpzrXdkPxoV7pU0zLFXTSicerd0SholH3V6e7OEN3CPtjKPD0ZM6IXTmm4XyHkbRl6aGBzIVs7ReOzjHaZOVqYEDPiAhBxOfLmvcEXQWYPVczSNih71ntRx2e5E7syR3jhJY7FTRlPxEx5uqRHqMRlpmvTjw4kCaRBJKtk7yXPpXCE5Po2xuTCv7gtdBrnZmzPMqeTdaX2QaHrHEn64ewH7GCqkzudbeMb";
        public static string tokenfile = "token.bin";
        public const string DBContext = "Host=localhost;Port=5432;Database=mydb;Username=postgres;Password=3011656";
        public static string[] ApiSettngs = new string[] { SiteLink, Key, tokenfile };
    }

}
