using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Contexts;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for ManageVehicleWindow_AddNewItem.xaml
    /// </summary>
    public partial class ManageVehicleWindow_AddNewItem : Window
    {
        private ManageVehicleWindow _manageVehicleWindow { get; set; }
        private EmployeeWorkOnVehicles _vehicle { get; set; }
        private int _idxToEdit { get; set; }
        public ManageVehicleWindow_AddNewItem(ManageVehicleWindow window ,EmployeeWorkOnVehicles vehicle, int idxToEdit = -1)
        {
            InitializeComponent();
            _vehicle = vehicle;
            _idxToEdit = idxToEdit-1;
            _manageVehicleWindow = window;
            FixUI();
        }

        private void FixUI()
        {
            if (_idxToEdit >= 0)
            {
                string[] scrapedData = _vehicle.WorkOn.Split(";", StringSplitOptions.RemoveEmptyEntries)[_idxToEdit].Split(":", StringSplitOptions.RemoveEmptyEntries);
                Txtbox_Name.Text = scrapedData[0];
                Txtbox_Price.Text = scrapedData[1];
            }
            else
            {
                Txtbox_Name.Clear();
                Txtbox_Price.Clear();
            }
        }

        private void SubmitItemButton_Click(object sender, RoutedEventArgs args)
        {
            string name = Txtbox_Name.Text;
            string price = Txtbox_Price.Text;

            if (String.IsNullOrEmpty(name))
            {
                MessageBox.Show("Item name cannot be empty!");
                return;
            }
            Regex checkForPrice = new Regex(@"^\d+(\.\d{0,2})?$");
            if (!checkForPrice.IsMatch(price))
            {
                MessageBox.Show("Incorrect price format!\nvalid is for example: 127.99");
                return;
            }

            if (_idxToEdit >= 0)
            {
                string[] scrapedData = _vehicle.WorkOn.Split(";", StringSplitOptions.RemoveEmptyEntries);
                scrapedData[_idxToEdit] = $"{name}:{price}:0";
                _vehicle.WorkOn = String.Join(";", scrapedData)+";";
            }
            else
            {
                _vehicle.WorkOn += $"{name}:{price}:0;";
            }
            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                context.EmployeeWorkOnVehicles.Update(_vehicle);
                context.SaveChanges();
            }
            _manageVehicleWindow.RefreshVehicleChangesData();
            _vehicle = null;
            _idxToEdit = -1;
            _manageVehicleWindow = null;
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs args)
        {
            _vehicle = null;
            _idxToEdit = -1;
            _manageVehicleWindow = null;
            this.Close();
        }
    }
}
