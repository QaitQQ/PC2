using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
namespace MGSol.Panel
{
    public partial class ItemControl
    {
        private class VisMarketItem : INotifyPropertyChanged
        {
            public VisMarketItem(MarketItem item, List<WS> warehouses)
            {
                Item = item;
                Warehouses = warehouses;
            }
            private List<WS> Warehouses { get; set; }
            public MarketItem Item { get; set; }
            private bool _checked
            {
                get; set;
            }
            public bool Checked { get => _checked; set { _checked = value; OnPropertyChanged(nameof(Checked)); } }
            public string Name { get => Item.Name; set => Item.Name = value; }
            public double Price { get => Item.Price; set => Item.Price = value; }
            public string Id { get => Item.Id; set => Item.Id = value; }
            public string SKU { get => Item.SKU; set => Item.SKU = value; }
            public int BaseID { get => Item.BaseID; set => Item.BaseID = value; }
            public string Art1C { get => Item.Art1C; set => Item.Art1C = value; }
            public ObservableCollection<StructLibCore.Marketplace.IMarketItem> Items
            {
                get => Item.Items;
                set => Item.Items = value;
            }
            public ObservableCollection<KeyValuePair<string, int>> STList
            {
                get
                {
                    ObservableCollection<KeyValuePair<string, int>> X = new();
                    if (Item.StoregeList == null)
                    {
                        return X;
                    }
                    foreach (IC item in Item.StoregeList)
                    {
                        string Name = Warehouses.First(X => X.Id == item.WID).N;
                        X.Add(new KeyValuePair<string, int>(Name, item.C));
                    }
                    return X;
                }
            }
            public System.Windows.Media.SolidColorBrush Color
            {
                get
                {
                    int SaleItemCount = 0;
                    if (Items != null)
                    {
                        foreach (IMarketItem item in Items)
                        {
                            if (item.Stocks is not "" and not null)
                            {
                                SaleItemCount += Convert.ToInt32(item.Stocks);
                            }
                        }
                    }
                    int BayItemCount = 0;
                    if (Item.StoregeList != null)
                    {
                        foreach (IC item in Item.StoregeList)
                        {
                            if (item.C != 0)
                            {
                                BayItemCount += item.C;
                            }
                        }
                    }
                    if (SaleItemCount > BayItemCount)
                    {
                        return BayItemCount > 0
                            ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.YellowGreen.R, System.Drawing.Color.YellowGreen.G, System.Drawing.Color.YellowGreen.B))
                            : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.OrangeRed.R, System.Drawing.Color.OrangeRed.G, System.Drawing.Color.OrangeRed.B));
                    }
                    else
                    {
                        if (SaleItemCount <= 3 && BayItemCount >= 3)
                        {
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Blue.R, System.Drawing.Color.Blue.G, System.Drawing.Color.Blue.B));
                        }
                        return SaleItemCount == 0 && BayItemCount == 0
                            ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Gray.R, System.Drawing.Color.Gray.G, System.Drawing.Color.Gray.B))
                            : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Green.R, System.Drawing.Color.Green.G, System.Drawing.Color.Green.B));
                    }
                }
            }
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
