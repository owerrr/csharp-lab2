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
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        checkBoxes = new CheckBox[]
        {
            Cb_Read_Owner,
            Cb_Write_Owner,
            Cb_Execute_Owner,

            Cb_Read_Group,
            Cb_Write_Group,
            Cb_Execute_Group,

            Cb_Read_Other,
            Cb_Write_Other,
            Cb_Execute_Other
        };
    }

    private string numericValue { get; set; }
    private string symbolicValue { get; set; }
    private int[] values = new int[3];

    private CheckBox[] checkBoxes;

    private void FixCheckboxes(object sender, RoutedEventArgs e)
    {
        string name = (e.Source as FrameworkElement).Name.ToString();
        bool? isChecked = (e.Source as CheckBox).IsChecked;
        //MessageBox.Show($"{name} : {isChecked}");
        values = new int[3];
        int idx = 0;
        foreach(CheckBox cb in checkBoxes)
        {
            if ((bool)cb.IsChecked)
            {
                if (cb.Name.EndsWith("Owner"))
                    idx = 0;
                else if (cb.Name.EndsWith("Group"))
                    idx = 1;
                else if (cb.Name.EndsWith("Other"))
                    idx = 2;

                if (cb.Name.Contains("Read"))
                {
                    values[idx] += 4;
                }
                else if (cb.Name.Contains("Write"))
                {
                    values[idx] += 2;
                }
                else if (cb.Name.Contains("Execute"))
                {
                    values[idx] += 1;
                }
            }
        }

        var numVal = values[0]*100 + values[1]*10 + values[2];
        if (numVal < 10)
            numericValue = "00" + numVal.ToString();
        else if (numVal < 100)
            numericValue = "0"+numVal.ToString();
        else
            numericValue = numVal.ToString();

        symbolicValue = ChmodConverter.NumericToSymbolic(numericValue);

        UpdateTextBoxes();
    }

    private void UpdateTextBoxes()
    {
        TextBox numericTxtBox = TxtBox_Numeric;
        TextBox symbolicTxtBox = TxtBox_Symbolic;

        numericTxtBox.Text = numericValue;
        symbolicTxtBox.Text = symbolicValue;
    }
}