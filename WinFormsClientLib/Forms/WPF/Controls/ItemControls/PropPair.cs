using System.Reflection;
using System.ComponentModel;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{
    public class PropPair : INotifyPropertyChanged
    {
        public PropertyInfo PropertyInfo { get; set; }
        private object _Value;

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value { get => _Value; set => Setvalue(value); }
        private void Setvalue(object Value) { this._Value = Value; OnPropertyChanged(nameof(PropPair)); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
