using static SiteApi.IntegrationSiteApi.ApiBase.ItemApi.AbstractItemApi;
namespace Pwa.Pages.Controls;
public partial class ItemControl : ContentView
{
    IAbstractItem Item {  get; set; }
    public ItemControl(IAbstractItem item)
	{
        Item = item;
        InitializeComponent();
        LabelName.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Name", Mode = BindingMode.OneWay });
        LabelDescription.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Description", Mode = BindingMode.OneWay });
        LabelPrice.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Price", Mode = BindingMode.OneWay });
    }
}
