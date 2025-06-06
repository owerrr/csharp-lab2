using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddVehicleWindow.xaml
    /// </summary>
    public partial class AddVehicleWindow : Window
    {
        public AddVehicleWindow()
        {
            InitializeComponent();
        }

        private void NewVehicle_ButtonAdd_Click(object sender, RoutedEventArgs args)
        {
            MessageBox.Show("add");
        }

        private void ClearTextBoxes()
        {
            NewVehicles_TxtBox_Model.Clear();
            NewVehicles_TxtBox_RegNo.Clear();
            NewVehicles_TxtBox_VIN.Clear();
            NewVehicles_TxtBox_Year.Clear();
        }

        private void NewVehicle_ButtonBack_Click(object sender, RoutedEventArgs args)
        {
            ClearTextBoxes();
            this.Close();
        }
    }
}
