using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PassportEditControl
{
    #region EditControl
    public partial class PassportEdit : UserControl, IDataGridViewEditingControl
    {
        private int _row;
        private DataGridView _dgv;
        private bool _valueChanged = false;
        private Passport _psForEditSession;

        public PassportEdit ( ) => InitializeComponent();

        private void _cb_SelectedIndexChanged ( object sender, EventArgs e ) => OnValueChanged();

        private void _dtp_ValueChanged ( object sender, EventArgs e ) => OnValueChanged();

        private void _mskEdit_Validating ( object sender, CancelEventArgs e )
        {
            if (_mskEdit.MaskCompleted)
            {
                OnValueChanged();
            }

            e.Cancel = !_mskEdit.MaskCompleted;
        }

        private void OnValueChanged ( )
        {
            _valueChanged = true;
            _psForEditSession.Series = (_cb.SelectedItem == null ? string.Empty : _cb.SelectedItem.ToString());
            _psForEditSession.Number = (_mskEdit.Text == null ? string.Empty : _mskEdit.Text);
            _psForEditSession.IssueDate = _dtp.Value;
            DataGridView dgv = EditingControlDataGridView;
            if (dgv != null)
            {
                dgv.NotifyCurrentCellDirty(true);
            }
        }

        public void SetupControls ( Passport ps )
        {
            _psForEditSession = new Passport();
            _cb.SelectedItem = ps.Series;
            _mskEdit.Text = ps.Number;
            _dtp.Value = ps.IssueDate;
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl ( DataGridViewCellStyle dataGridViewCellStyle )
        {
            _cb.Font = _mskEdit.Font = _dtp.Font = dataGridViewCellStyle.Font;
            MinimumSize = Size;
        }

        public DataGridView EditingControlDataGridView
        {
            get => _dgv;
            set => _dgv = value;
        }

        public object EditingControlFormattedValue
        {
            get => _psForEditSession;
            set
            {
                //nothing to do...
            }
        }

        public int EditingControlRowIndex
        {
            get => _row;
            set => _row = value;
        }

        public bool EditingControlValueChanged
        {
            get => _valueChanged;
            set => _valueChanged = value;
        }

        public bool EditingControlWantsInputKey ( Keys keyData, bool dataGridViewWantsInputKey )
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Home:
                case Keys.End:
                    return true;
                default:
                    return false;
            }
        }

        public Cursor EditingPanelCursor => base.Cursor;

        public object GetEditingControlFormattedValue ( DataGridViewDataErrorContexts context ) => EditingControlFormattedValue;

        public void PrepareEditingControlForEdit ( bool selectAll ) { }

        public bool RepositionEditingControlOnValueChange => false;

        #endregion
    }
    #endregion

    #region Custom Cell
    public class DataGridViewPassportCell : DataGridViewTextBoxCell
    {
        private const string DEFAULT_STRING = "Паспортные данные неизвестны...";
        private int _heightOfRowBeforeEditMode;

        public DataGridViewPassportCell ( ) : base() { }

        public override void InitializeEditingControl ( int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle )
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            PassportEdit pasCtrl = DataGridView.EditingControl as PassportEdit;
            _heightOfRowBeforeEditMode = OwningRow.Height;
            OwningRow.Height = pasCtrl.Height;
            Passport pasInCell = Value as Passport;
            if (pasInCell == null)
            {
                pasInCell = new Passport();
            }

            pasCtrl.SetupControls(pasInCell);
        }

        public override void DetachEditingControl ( )
        {
            if (_heightOfRowBeforeEditMode > 0)
            {
                OwningRow.Height = _heightOfRowBeforeEditMode;
            }

            base.DetachEditingControl();
        }

        public override Type EditType => typeof(PassportEdit);

        public override Type ValueType => typeof(Passport);

        public override Type FormattedValueType => typeof(Passport);

        public override object DefaultNewRowValue => DEFAULT_STRING;

        protected override object GetFormattedValue ( object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context )
        {
            if (value == null)
            {
                return DEFAULT_STRING;
            }
            else
            {
                return TypeDescriptor.GetConverter(value).ConvertToString(value);
            }
        }
    }
    #endregion

    #region Custom Column
    public class DataGridViewPassportColumn : DataGridViewColumn
    {
        public DataGridViewPassportColumn ( ) : base(new DataGridViewPassportCell()) { }

        public override DataGridViewCell CellTemplate
        {
            get => base.CellTemplate;
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewPassportCell)))
                {
                    throw new InvalidCastException("Cell must be a PassportCell");
                }

                base.CellTemplate = value;
            }
        }
    }
    #endregion

    #region Passport object
    [TypeConverter(typeof(PassportConverter))]
    public class Passport
    {
        private string _series;
        private string _number;
        private DateTime _issueDate;

        public string Series
        {
            get => _series;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length != 2)
                {
                    throw new ArgumentOutOfRangeException("value", "Series must contain exactly 2 digits");
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    if (!uint.TryParse(value, out uint result))
                    {
                        throw new ArgumentOutOfRangeException("value", "Series must contain only digits");
                    }

                    if (result != 0 && result % 10 != result / 10)
                    {
                        throw new ArgumentOutOfRangeException("value", "Series must folow template - 00, 11, 22, ...");
                    }
                }
                _series = value;
            }
        }
        public string Number
        {
            get => _number;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length != 6)
                {
                    throw new ArgumentOutOfRangeException("value", "Series must contain exactly 6 digits");
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    if (!uint.TryParse(value, out uint result))
                    {
                        throw new ArgumentOutOfRangeException("value", "Series must contain only digits");
                    }
                }
                _number = value;
            }
        }
        public DateTime IssueDate
        {
            get => _issueDate;
            set => _issueDate = value;
        }


        /// <summary>
        /// Creates a new instance of Passport
        /// </summary>
        /// <param name="series">Passport Series</param>
        /// <param name="number">Number of Passport</param>
        /// <param name="issueDate">Date of issue</param>
        public Passport ( string series, string number, DateTime issueDate )
        {
            Series = series;
            Number = number;
            IssueDate = issueDate;
        }


        /// <summary>
        /// Creates a new instance of "default" Passport
        /// </summary>
        public Passport ( ) : this("00", "000000", new DateTime(1970, 1, 1)) { }
    }
    #endregion

    #region PassportConverter
    public class PassportConverter : TypeConverter
    {
        private const string DEFAULT_FORMAT_STRING = "Серия: {0} " + " №: {1} " + " Выдан: {2} ";

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo ( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
        {
            if (destinationType == typeof(string))
            {
                Passport pas = value as Passport;
                return string.Format(DEFAULT_FORMAT_STRING, pas.Series, pas.Number, pas.IssueDate.ToString("d"));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion
}