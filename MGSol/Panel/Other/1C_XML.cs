using System.Collections.Generic;
using System.Xml.Serialization;

namespace MGSol.Panel
{
    [XmlRoot(ElementName = "Номенклатура")]
    public class Номенклатура
    {
        [XmlElement(ElementName = "Наименование")]
        public string Наименование;

        [XmlElement(ElementName = "Артикул")]
        public string Артикул;

        [XmlElement(ElementName = "Код")]
        public string Код;

        [XmlElement(ElementName = "БазоваяЕдиницаИзмерения")]
        public string БазоваяЕдиницаИзмерения;

        [XmlElement(ElementName = "ВидНоменклатуры")]
        public string ВидНоменклатуры;
    }
    [XmlRoot(ElementName = "Выгрузка")]
    public class Выгрузка
    {
        [XmlElement(ElementName = "Номенклатура")]
        public List<Номенклатура> Номенклатура;
    }
}
