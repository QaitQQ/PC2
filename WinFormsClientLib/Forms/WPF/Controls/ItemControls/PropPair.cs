using System;
using System.ComponentModel;
using System.Reflection;

namespace WinFormsClientLib.Forms.WPF.ItemControls
{
    public class PropPair : INotifyPropertyChanged, IDisposable
    {
        public PropertyInfo PropertyInfo { get; set; }
        private object _Value;

        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public object Value { get => _Value; set => Setvalue(value); }
        private void Setvalue(object Value) { _Value = Value; OnPropertyChanged(nameof(PropPair)); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            PropertyInfo = null;
            _Value = null;
            _name = null;
            PropertyChanged = null;
            GC.SuppressFinalize(this);
        }
    }
}
