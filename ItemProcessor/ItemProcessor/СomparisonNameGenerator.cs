using Pricecona;

namespace Server.Class.ItemProcessor
{
    public static class СomparisonNameGenerator
    {
        private static string _СomparisonName;
        public static string Get(string NameString)
        {
            _СomparisonName = null;
            foreach (char item in NameString)
            {
                if (char.IsDigit(item) || char.IsLetter(item) || item == '+')
                {
                    _СomparisonName += item;
                    if (_СomparisonName.Length > 30)
                    {
                        break;
                    }
                }
            }
            return _СomparisonName?.ToLower();
        }
        public static PriceStruct GetForPriceStruct(PriceStruct item) => new PriceStruct(item, СomparisonName: Get(item.Name));
    }
}
