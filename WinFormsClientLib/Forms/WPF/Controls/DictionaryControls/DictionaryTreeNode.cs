
using Object_Description;

using System.Windows.Controls;
using System.Windows.Input;

namespace WinFormsClientLib.Forms.WPF.Controls.DictionaryControls
{
    public partial class DictionaryControl
    {
        sealed class DictionaryTreeNode
        {
            IDictionaryPC _Dic;
            MouseButtonEventHandler[] _action;
            MouseEventHandler _DropEvents;
            public DictionaryTreeNode(IDictionaryPC Dic, MouseButtonEventHandler[] action, MouseEventHandler DropEvents) { _Dic = Dic; _action = action; _DropEvents = DropEvents; }
            public TreeViewItem GetNode() 
            {
                var box = new TextBlock { Text = _Dic.Name };
                box.MouseLeftButtonDown += _action[0];
                box.PreviewMouseLeftButtonDown += _action[1];
                box.PreviewMouseMove += _DropEvents;
                TreeViewItem Node = new TreeViewItem { Header = box,   DataContext = _Dic, AllowDrop = true, HorizontalContentAlignment =  System.Windows.HorizontalAlignment.Left, VerticalContentAlignment = System.Windows.VerticalAlignment.Center };
                if (_Dic.Branches != null)
                {              
                    foreach (var item in _Dic.Branches)
                    {
                        Node.Items.Add(new DictionaryTreeNode(item, _action, _DropEvents).GetNode());
                    }
                }
                return Node;
            }
        }
    }
}
