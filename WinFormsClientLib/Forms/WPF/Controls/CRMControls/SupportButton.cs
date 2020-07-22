using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WinFormsClientLib.Forms.WPF.Controls
{
    public static class SupportButton
    {

        public static void AddButtons(ObservableCollection<Button> Collection, RoutedEventHandler[] Methods) 
        {
            foreach (var item in Methods)
            {
                AddButton(Collection, item);
            }          
        }

        public static void AddButton(ObservableCollection<Button> Collection, RoutedEventHandler Method) 
        {
            var Button = new Button();
            Button.Height = 20;
            Button.Click += Method;
            Button.Content = Method.Method.Name;
            Collection.Add(Button);
        }       
    }
}
