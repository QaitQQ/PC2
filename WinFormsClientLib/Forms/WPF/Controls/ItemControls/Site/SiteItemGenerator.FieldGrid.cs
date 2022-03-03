using StructLibs;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WinFormsClientLib.Forms.WPF.Controls.ItemControls
{
    public partial class SiteItemGenerator
    {
        private sealed class FieldGrid : Grid, INotifyPropertyChanged
        {
            public int ID { get; set; }
            public string FieldName { get; set; }
            public FieldType Type { get; set; }
            private string _description;
            public string Description
            {
                get { return _description; }
                set { _description = value; OnPropertyChanged("Description");}
            }
            public FieldGrid(int id, string fieldName, string description, FieldType type)
            {
                ID = id; FieldName = fieldName; Type = type; _description = description;

                Binding binding = new Binding();
                TextBox BoxOne = new TextBox();
                binding.Source = this;
                binding.Path = new PropertyPath("ID");
                binding.Mode = BindingMode.TwoWay;
                BoxOne.SetBinding(TextBox.TextProperty, binding);
                Grid OneGrid = new Grid() { Children = { BoxOne } };

                Binding binding2 = new Binding();
                TextBox BoxTwo = new TextBox();
                binding2.Source = this;
                binding2.Path = new PropertyPath("FieldName");
                binding2.Mode = BindingMode.TwoWay;
                BoxTwo.SetBinding(TextBox.TextProperty, binding2);
                Grid TwoGrid = new Grid() { Children = { BoxTwo } };

                Binding binding3 = new Binding();
                TextBox BoxThree = new TextBox();
                binding3.Source = this;
                binding3.Path = new PropertyPath("Description");
                binding3.Mode = BindingMode.TwoWay;
                BoxThree.SetBinding(TextBox.TextProperty, binding3);
                Grid ThreeGrid = new Grid() { Children = { BoxThree } };

                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
                ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80) });
                ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(OneGrid, 0);
                Grid.SetColumn(TwoGrid, 1);
                Grid.SetColumn(ThreeGrid, 3);
                Children.Add(OneGrid);
                Children.Add(TwoGrid);
                Children.Add(ThreeGrid);
            }
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(string prop = "")
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
