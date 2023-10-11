using static SiteApi.IntegrationSiteApi.ApiBase.ItemApi.AbstractItemApi;
namespace Pwa.Pages.Controls;
public partial class ItemControl : ContentView,IDisposable
{
    IAbstractItem Item {  get; set; }
    public ItemControl(IAbstractItem item)
	{
        Item = item;
        InitializeComponent();
        LabelName.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Name", Mode = BindingMode.OneTime });
        LabelDescription.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Description", Mode = BindingMode.OneTime });
        LabelPrice.SetBinding(Label.TextProperty, new Binding() { Source = Item, Path = "Price", Mode = BindingMode.OneTime });
    }

    public void Dispose()
    {
        LabelName.RemoveBinding(Label.TextProperty);
        LabelDescription.RemoveBinding(Label.TextProperty);
        LabelPrice.RemoveBinding(Label.TextProperty);
        Item = null; 
    }
}
