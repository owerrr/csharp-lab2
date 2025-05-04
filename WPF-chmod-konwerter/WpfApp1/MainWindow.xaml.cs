using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChmodConverterLib;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// 
public class PermissionsViewModel : INotifyPropertyChanged {
    private string _numericValue = "000";
    private string _symbolicValue = "---------";

    private bool _isReadOwner;
    private bool _isWriteOwner;
    private bool _isExecuteOwner;
    private bool _isReadGroup;
    private bool _isWriteGroup;
    private bool _isExecuteGroup;
    private bool _isReadOther;
    private bool _isWriteOther;
    private bool _isExecuteOther;

    private string _errorMessage;

    private bool IsUpdating = false;

    public bool IsReadOwner
    {
        get => _isReadOwner;
        set
        {
            _isReadOwner = value;
            OnPropertyChanged(nameof(IsReadOwner));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsWriteOwner
    {
        get => _isWriteOwner;
        set
        {
            _isWriteOwner = value;
            OnPropertyChanged(nameof(IsWriteOwner));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsExecuteOwner
    {
        get => _isExecuteOwner;
        set
        {
            _isExecuteOwner = value;
            OnPropertyChanged(nameof(IsExecuteOwner));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsReadGroup
    {
        get => _isReadGroup;
        set
        {
            _isReadGroup = value;
            OnPropertyChanged(nameof(IsReadGroup));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsWriteGroup
    {
        get => _isWriteGroup;
        set
        {
            _isWriteGroup = value;
            OnPropertyChanged(nameof(IsWriteGroup));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsExecuteGroup
    {
        get => _isExecuteGroup;
        set
        {
            _isExecuteGroup = value;
            OnPropertyChanged(nameof(IsExecuteGroup));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsReadOther
    {
        get => _isReadOther;
        set
        {
            _isReadOther = value;
            OnPropertyChanged(nameof(IsReadOther));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsWriteOther
    {
        get => _isWriteOther;
        set
        {
            _isWriteOther = value;
            OnPropertyChanged(nameof(IsWriteOther));
            UpdateSymbolicPermsCb();
        }
    }
    public bool IsExecuteOther
    {
        get => _isExecuteOther;
        set
        {
            _isExecuteOther = value;
            OnPropertyChanged(nameof(IsExecuteOther));
            UpdateSymbolicPermsCb();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public string NumericValue
    {
        get => _numericValue;
        set
        {
            if(_numericValue != value)
            {
                _numericValue = value;
                OnPropertyChanged(nameof(NumericValue));
                try
                {
                    SymbolicValue = ChmodConverter.NumericToSymbolic(NumericValue);
                    UpdateCheckBoxes();
                    ErrorMessage = "";
                }
                catch(ArgumentException ex)
                {
                    ErrorMessage = ex.Message;
                }
            }
        }
    }

    public string SymbolicValue
    {
        get => _symbolicValue;
        set
        {
            if (_symbolicValue != value)
            {
                _symbolicValue = value;
                OnPropertyChanged(nameof(SymbolicValue));
                try
                {
                    NumericValue = ChmodConverter.SymbolicToNumeric(SymbolicValue);
                    ErrorMessage = "";
                }
                catch(ArgumentException ex){
                    ErrorMessage = ex.Message;
                }
            }
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }

    private void UpdateSymbolicPermsCb()
    {
        if (IsUpdating) return;
        IsUpdating = true;
        string symbolic = "";
        symbolic += IsReadOwner ? 'r' : '-';
        symbolic += IsWriteOwner ? 'w' : '-';
        symbolic += IsExecuteOwner ? 'x' : '-';

        symbolic += IsReadGroup ? 'r' : '-';
        symbolic += IsWriteGroup ? 'w' : '-';
        symbolic += IsExecuteGroup ? 'x' : '-';

        symbolic += IsReadOther ? 'r' : '-';
        symbolic += IsWriteOther ? 'w' : '-';
        symbolic += IsExecuteOther ? 'x' : '-';
        IsUpdating = false;
        SymbolicValue = symbolic;
    }

    private void UpdateCheckBoxes()
    {
        if (string.IsNullOrEmpty(SymbolicValue) || SymbolicValue.Length != 9)
            return;
        if (IsUpdating) return;
        IsUpdating = true;
        IsReadOwner = SymbolicValue[0] == 'r';
        IsWriteOwner = SymbolicValue[1] == 'w';
        IsExecuteOwner = SymbolicValue[2] == 'x';

        IsReadGroup = SymbolicValue[3] == 'r';
        IsWriteGroup = SymbolicValue[4] == 'w';
        IsExecuteGroup = SymbolicValue[5] == 'x';

        IsReadOther = SymbolicValue[6] == 'r';
        IsWriteOther = SymbolicValue[7] == 'w';
        IsExecuteOther = SymbolicValue[8] == 'x';
        IsUpdating = false;
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public partial class MainWindow : Window
{
    public PermissionsViewModel ViewModel { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new PermissionsViewModel();
        DataContext = ViewModel;
    }

    private void Grid_LostFocus(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
        FocusManager.SetFocusedElement(this, (UIElement)sender);
    }
}