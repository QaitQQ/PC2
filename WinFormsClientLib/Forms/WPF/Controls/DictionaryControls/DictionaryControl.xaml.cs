using Client;

using Object_Description;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using WinFormsClientLib.Forms.WPF.ItemControls;

namespace WinFormsClientLib.Forms.WPF.Controls.DictionaryControls
{
    /// <summary>
    /// Логика взаимодействия для DictionaryControl.xaml
    /// </summary>
    public partial class DictionaryControl : UserControl
    {
        private List<IDictionaryPC> Dictionaries;
        private ObservableCollection<TreeViewItem> DictionariesSourse;
        private ObservableCollection<UIElement> PropPanel;
        private IDictionaryPC ActiveDic;
        System.Windows.Point _startPoint;
        bool _IsDragging = false;
        public DictionaryControl()
        {
            InitializeComponent();
            DictionariesSourse = new ObservableCollection<TreeViewItem>();
            DictionaryRelateBox.ItemsSource = Enum.GetValues(typeof(DictionaryRelate));
            Dictionaries = new Network.Dictionary.GetDictionares().Get<Dictionaries>(new WrapNetClient()).GetAll();
            PropPanel = new ObservableCollection<UIElement>();
            PPanel.ItemsSource = PropPanel;
            DictionaryTree.ItemsSource = DictionariesSourse;
            this.Loaded += DictionaryControl_Loaded;
        }
        private void DictionaryControl_Loaded(object sender, RoutedEventArgs e)
        {
            ((UniversalWPFContainerForm)this.DataContext).FormClosing += DictionaryControl_FormClosing;
        }
        private void DictionaryControl_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            var Result = new Dictionaries();
            foreach (var item in Dictionaries)
            {
                item.Branches = null;
                Result.Add(item);
            }

            new Network.Dictionary.SetDictionares().Get<bool>(new WrapNetClient(), Result);
        }
        private void FillTree(DictionaryRelate relate)
        {
            Object_Description.DictionaryBase DicBase = new Object_Description.DictionaryBase();

            List<IDictionaryPC> searchDictionaries = new List<IDictionaryPC>();

            List<IDictionaryPC> tempDictionaries = new List<IDictionaryPC>();

            DictionariesSourse.Clear();

            foreach (var item in Dictionaries)
            {
                if (item.Relate == relate)
                {
                    searchDictionaries.Add(item);
                }
            }

            foreach (var item in searchDictionaries)
            {
                if (item is DictionarySiteCategory category && category.Parent_id != 0)
                {
                    tempDictionaries.Add(item);
                }
            }

            foreach (var item in searchDictionaries)
            {
                if(!(item is DictionarySiteCategory))
                {
                    DicBase.AddBranch(item);
                }
                else if(((DictionarySiteCategory)item).Parent_id == 0)
                {
                    DicBase.AddBranch(item);
                }
            }

            int i = 1;
            int t = 0;

            while (i > 0)
            {
                List<IDictionaryPC> temp2Dictionaries = new List<IDictionaryPC>();

                foreach (var item in tempDictionaries)
                {
                    foreach (var X in DicBase.GetAllBranches())
                    {
                        if (item is DictionarySiteCategory && ((DictionarySiteCategory)item).Parent_id == X.Id)
                        {
                            X.AddBranch(item);
                            temp2Dictionaries.Add(item);
                        }
                    }
                }
                if (t > 8)
                {
                    foreach (var item in tempDictionaries)
                    {
                        DicBase.AddBranch(item);
                        temp2Dictionaries.Add(item);
                    }
                }

                foreach (var item in temp2Dictionaries)
                {
                    tempDictionaries.Remove(item);
                }
                i = tempDictionaries.Count;

                t++;
            }

            foreach (var item in (new DictionaryTreeNode(DicBase, new MouseButtonEventHandler[] { Box_MouseLeftButtonDown, TemplateTreeView_PreviewMouseLeftButtonDown }, TemplateTreeView_PreviewMouseMove).GetNode()).Items)
            {
                DictionariesSourse.Add((TreeViewItem)item);
            }
        }
        void TemplateTreeView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed ||
                e.RightButton == MouseButtonState.Pressed && !_IsDragging)
            {
                System.Windows.Point position = e.GetPosition(null);
                if (Math.Abs(position.X - _startPoint.X) >
                        SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _startPoint.Y) >
                        SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(e);
                }
            }
        }
        void TemplateTreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }
        private void StartDrag(MouseEventArgs e)
        {
            _IsDragging = true;
            object temp = e.OriginalSource;
            DataObject data = null;
            if (temp != null)
            {
                data = new DataObject("inadt", temp);

                if (data != null)
                {
                    DragDropEffects dde = DragDropEffects.Move;
                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        dde = DragDropEffects.All;
                    }
                    DragDropEffects de = DragDrop.DoDragDrop(this.DictionaryTree, data, dde);
                }
            }

            _IsDragging = false;
        }
        private void Box_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ActiveDic = (IDictionaryPC)((TextBlock)sender).DataContext;
            FillPropPanel();
        }
        private void Lst_context_Click2(object sender, RoutedEventArgs e)
        {
            var btn = (TextBlock)sender;
            var pbtn = (ContextMenu)btn.Parent;
            var pbtnp = (ListView)pbtn.PlacementTarget;
            var sourse = (ICollection<string>)pbtnp.ItemsSource;

            var Modalbox = new ModalBox();

            if ((bool)Modalbox.ShowDialog())
            {
                sourse.Add(Modalbox.STR);
            }
            FillPropPanel();
        }
        private void Lst_context_Click(object sender, RoutedEventArgs e)
        {
            var btn = (TextBlock)sender;
            var pbtn = (ContextMenu)btn.Parent;
            var pbtnp = (ListView)pbtn.PlacementTarget;
            var sourse = (ICollection<string>)pbtnp.ItemsSource;
            sourse.Remove((string)pbtnp.SelectedItem);
            FillPropPanel();
        }
        private void Txtbx_TextChanged(object sender, EventArgs e)
        {
            var box = (System.Windows.Controls.TextBox)sender;
            var PP = ((PropPair)box.DataContext).PropertyInfo;

            if (PP.PropertyType.Name.Contains("String"))
            {
                PP.SetValue(ActiveDic, box.Text);
            }
            if (PP.PropertyType.Name.Contains("Int32"))
            {
                PP.SetValue(ActiveDic, Convert.ToInt32(box.Text));
            }
            if (PP.PropertyType.Name.Contains("Double"))
            {
                PP.SetValue(ActiveDic, Convert.ToDouble(box.Text));
            }
        }
        private void Txtbx_SelectionChanged(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            var PP = ((PropPair)box.DataContext).PropertyInfo;

            PP.SetValue(ActiveDic, box.SelectedItem);
        }
        private void DictionaryRelateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DictionaryRelateBox.SelectedIndex != -1)
            {
                FillTree((DictionaryRelate)DictionaryRelateBox.SelectedIndex);
            }
        }
        private void FillPropPanel()
        {
            var propertyInfos = ActiveDic.GetProperties();
            PropPanel.Clear();
            ObservableCollection<PropPair> Lst = new ObservableCollection<PropPair>();
            foreach (System.Reflection.PropertyInfo item in propertyInfos)
            {
                Lst.Add(new PropPair { Name = item.Name, PropertyInfo = item, Value = item.GetValue(ActiveDic) });
            }
            if (ActiveDic.Relate == DictionaryRelate.Price)
            {
                var Dic = (DictionaryPrice)ActiveDic;
                if (Dic.Filling_method_coll != null)
                {
                    Lst.Add(new PropPair { Value = Dic, Name = "Filling_method_coll" });
                }
                if (Dic.Filling_method_string != null)
                {
                    Lst.Add(new PropPair { Value = Dic, Name = "Filling_method_string" });
                }
            }
            foreach (var item in Lst)
            {
                if (item.PropertyInfo == null)
                {
                    var lbl = new Label() { Content = item.Name };
                    Grid.SetColumn(lbl, 0);
                    var stk = new Grid();
                    Grid.SetColumn(stk, 1);
                    if (item.Value != null)
                    {
                        stk.Children.Add(new FillDefinition_Control(item.Value as DictionaryPrice, item.Name) { DataContext = ActiveDic });

                    }

                    PropPanel.Add(new Grid { ColumnDefinitions = { new ColumnDefinition { }, new ColumnDefinition() }, Children = { lbl, stk } });
                }
                else
                {
                    if (item.PropertyInfo.PropertyType.FullName.Contains("Collection"))
                    {
                        var lbl = new Label() { Content = item.Name };
                        Grid.SetColumn(lbl, 0);
                        var stk = new ListView();
                        Grid.SetColumn(stk, 1);

                        if (item.PropertyInfo.PropertyType.FullName.Contains("List") && item.PropertyInfo.PropertyType.FullName.Contains("String") && item.Value == null)
                        {
                            item.Value = new List<string>();
                            item.PropertyInfo.SetValue(ActiveDic, item.Value);
                        }


                        if (item.Value != null)
                        {

                            stk.ItemsSource = item.Value as IEnumerable;
                            var btn = new TextBlock { Text = "Удалить" };
                            btn.MouseDown += Lst_context_Click;
                            var btn2 = new TextBlock { Text = "Добавить" };
                            btn2.MouseDown += Lst_context_Click2;

                            stk.ContextMenu = new ContextMenu { Items = { btn, btn2 } };

                        }

                        PropPanel.Add(new Grid { ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(100) }, new ColumnDefinition() }, Children = { lbl, stk } });
                    }
                    else
                    {

                        var lbl = new Label() { Content = item.Name };
                        Control Txtbx;
                        if (item.Name == "Relate")
                        {
                            Txtbx = new ComboBox { ItemsSource = Enum.GetValues(typeof(DictionaryRelate)) };
                            ((ComboBox)Txtbx).SelectedValue = item.Value;
                            ((ComboBox)Txtbx).SelectionChanged += Txtbx_SelectionChanged;
                        }
                        else
                        {
                            Txtbx = new TextBox { Text = item.Value.ToString() };
                            ((TextBox)Txtbx).TextChanged += Txtbx_TextChanged;
                        }

                        Txtbx.DataContext = item;

                        Grid.SetColumn(lbl, 0);
                        Grid.SetColumn(Txtbx, 1);
                        PropPanel.Add(new Grid { ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(100) }, new ColumnDefinition() }, Children = { lbl, Txtbx } });


                    }
                }
            }
        }
    }
}

