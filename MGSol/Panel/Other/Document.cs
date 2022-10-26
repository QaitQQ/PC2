using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace MGSol.Panel
{
    [Serializable]
    public class Document
    {
        public string INN_Byer;
        public string INN_Seller;
        public string Nomber;
        public string Data;
        public List<Order> Orders;
        public Document()
        {
            Orders = new();
        }
    }
    [Serializable]
    public class Order
    {
        public string DepartureNumber;
        public string DepartureDate;
        public List<OrderItem> Items;
        public SF SchetFaktura;

        public Order() { Items = new List<OrderItem>(); }
    }
    [Serializable]
    public class OrderItem
    {
        public string Article1C;
        public string SKU;
        public string Price;
        public string Count;
        public string Date;
        public OrdersTaypeEnum Type;
    }
    [Serializable]
    public class SF
    {
        public string Date;
        public string Nomber;
        public string NameBuyer;
        public string INN;
    }
    [Serializable]
    public class ParamsColl
    {
        public ParamEnum Param;
        public int X;
        public int Y;
    }
    [Serializable]
    public class ColorCell : INotifyPropertyChanged
    {
        [NonSerialized]
        private Brush color;
        private ParamEnum param;
        private string value;
        private int x;
        private int y;
        private int lst;
        [NonSerialized]
        private TextBlock textBlockLink;
        public string Value { get => value; set => this.value = value; }
        public Brush Color { get => color; set { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("color")); color = value; } }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Lst { get => lst; set => lst = value; }
        public TextBlock TextBlockLink { get => textBlockLink; set => textBlockLink = value; }
        public ParamEnum Param { get => param; set => param = value; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    [Serializable]
    public enum ParamEnum
    {
        SKU, НачСтрок, КонецСтрок, ЦенаПродажи, Количество, НомерЗаказа, Дата, ВозвратСумма, ВозвратКолич, НомерОтчета, Бонус, НомерСФ, НачСтрокВозврат, КонСтрокВозврат, SkuВозврат, НомерВозврата
    }
    [Serializable]
    public enum OrdersTaypeEnum
    {
        Продажа,
        Возврат
    }
    [Serializable]
    public class FileOption : INotifyPropertyChanged
    {
        public FileOption()
        {
            FullPath = null;
            FileName = null;
            ParamsColl = new List<ParamsColl>();
            InnString = null;
            aPISetting = null;
        }
        private APISetting aPISetting;

        public APISetting APISetting
        {
            get => aPISetting;
            set => aPISetting = value;
        }
        private string description;
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public List<ParamsColl> ParamsColl { get; set; }
        public StructLibCore.Marketplace.InnString InnString { get; set; }
        public string InnStringName
        {
            get
            {
                if (InnString != null)
                {
                    return InnString.MarketName.ToString();
                }
                else
                {
                    return "";
                }

            }
        }
        public string Description { get => description; set { description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description))); } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}