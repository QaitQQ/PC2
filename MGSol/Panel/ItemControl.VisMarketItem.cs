using StructLibCore;
using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
namespace MGSol.Panel
{
    public partial class ItemControl
    {
        private class VisMarketItem
        {
            public VisMarketItem(MarketItem item, List<WS> warehouses)
            {
                Item = item;
                Warehouses = warehouses;
            }
            private List<WS> Warehouses { get; set; }
            public MarketItem Item { get; set; }
            public string Name { get { return Item.Name; } set { Item.Name = value; } }
            public double Price { get { return Item.Price; } set { Item.Price = value; } }
            public string Id { get { return Item.Id; } set { Item.Id = value; } }
            public string SKU { get { return Item.SKU; } set { Item.SKU = value; } }
            public int BaseID { get { return Item.BaseID; } set { Item.BaseID = value; } }
            public string Art1C { get { return Item.Art1C; } set { Item.Art1C = value; } }
            public ObservableCollection<StructLibCore.Marketplace.IMarketItem> Items
            {
                get { return Item.Items; }
                set { Item.Items = value; }
            }
            public ObservableCollection<KeyValuePair<String, int>> STList
            {
                get
                {
                    var X = new ObservableCollection<KeyValuePair<String, int>>();
                    if (Item.StoregeList == null)
                    {
                        return X;
                    }
                    foreach (var item in Item.StoregeList)
                    {
                        var Name = Warehouses.First(X => X.Id == item.WID).N;
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
                    foreach (var item in Items)
                    {
                        if (item.Stocks != "" && item.Stocks != null)
                        {
                            SaleItemCount = SaleItemCount + Convert.ToInt32(item.Stocks);
                        }
                    }
                    int BayItemCount = 0;
                    if (Item.StoregeList != null)
                    {
                        foreach (var item in Item.StoregeList)
                        {
                            if (item.C != 0 && item.C != null)
                            {
                                BayItemCount = BayItemCount + item.C;
                            }
                        }
                    }
                    if (SaleItemCount > BayItemCount)
                    {
                        if (BayItemCount > 0)
                        {
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.YellowGreen.R, System.Drawing.Color.YellowGreen.G, System.Drawing.Color.YellowGreen.B));
                        }
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.OrangeRed.R, System.Drawing.Color.OrangeRed.G, System.Drawing.Color.OrangeRed.B));
                    }
                    else
                    {
                        if (SaleItemCount <= 3 && BayItemCount >= 3)
                        {
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Blue.R, System.Drawing.Color.Blue.G, System.Drawing.Color.Blue.B));
                        }
                        if (SaleItemCount == 0 && BayItemCount == 0)
                        {
                            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Gray.R, System.Drawing.Color.Gray.G, System.Drawing.Color.Gray.B));
                        }
                        return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(System.Drawing.Color.Green.R, System.Drawing.Color.Green.G, System.Drawing.Color.Green.B));
                    }
                }
            }
        }
    }
}
