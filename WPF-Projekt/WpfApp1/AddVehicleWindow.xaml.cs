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
    /// Interaction logic for AddVehicleWindow.xaml
    /// </summary>
    public partial class AddVehicleWindow : Window
    {
        private Users _user { get; set; }
        private MainWindow _mw { get; set; }
        private ClientVehicles _vehicle { get; set; }
        public AddVehicleWindow(Users user, MainWindow mw, ClientVehicles vehicle=null)
        {
            InitializeComponent();
            _user = user;
            _mw = mw;
            if (vehicle != null)
            {
                _vehicle = vehicle;
                FixUI_EditVehicle();
            }
            else
            {
                ClearTextBoxes(); 
                Label_WindowHeader.Content = "New Vehicle";
            }
        }

        private void FixUI_EditVehicle()
        {
            NewVehicles_TxtBox_Model.Text = _vehicle.Car_Model;
            NewVehicles_TxtBox_RegNo.Text = _vehicle.Car_RegNo;
            NewVehicles_TxtBox_VIN.Text = _vehicle.Car_Vin;
            NewVehicles_TxtBox_Year.Text = _vehicle.Car_Year.ToString();
            Label_WindowHeader.Content = "Edit Vehicle";
        }

        private void NewVehicle_ButtonAdd_Click(object sender, RoutedEventArgs args)
        {
            VerifyCarDetails();
        }

        private void VerifyCarDetails()
        {
            string model = NewVehicles_TxtBox_Model.Text;
            string regNo = NewVehicles_TxtBox_RegNo.Text;
            string vin = NewVehicles_TxtBox_VIN.Text;
            int year = 0;
            try
            {
                year = Convert.ToInt32(NewVehicles_TxtBox_Year.Text);
            }catch(Exception ex)
            {
                MessageBox.Show("Car year must be an integer! For example: 2016");
                return;
            }

            if (String.IsNullOrWhiteSpace(model)
                || String.IsNullOrWhiteSpace(regNo)
                || String.IsNullOrWhiteSpace(vin)
            )
            {
                MessageBox.Show("You must fill all vehicle informations!");
                return;
            }

            if(regNo.Length > 8)
            {
                MessageBox.Show("Invalid license plate number!");
                return;
            }

            if(!Regex.IsMatch(vin, @"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("Invalid VIN number!\nValid vin is 17 characters long\nIt doesn't contain: I, O, Q letters.");
                return;
            }

            if(year < 1000 || year > DateTime.Now.Year)
            {
                MessageBox.Show("Invalid car year! Please check it out and try again.");
                return;
            }

            if(_vehicle != null)
            {
                using (WorkshopDbContext context = new WorkshopDbContext())
                {
                    _vehicle.Car_Model = model;
                    _vehicle.Car_RegNo = regNo;
                    _vehicle.Car_Vin = vin;
                    _vehicle.Car_Year = year;
                    context.Client_Vehicles.Update(_vehicle);
                    context.SaveChanges();
                    MessageBox.Show("Vehicle successfully changed!");
                    _mw.RefreshClientVehicles();
                    _mw = null;
                    _user = null;
                    _vehicle = null;
                    this.Close();
                }
            }
            else
            {
                using (WorkshopDbContext context = new WorkshopDbContext())
                {
                    bool userExists = context.Users.Any(x => x.Name == _user.Name);

                    if (userExists)
                    {
                        Clients? c = context.Clients.FirstOrDefault(x => x.Id == _user.Client_Id);
                        if (c != null)
                        {
                            var car = new ClientVehicles { Car_Model = model, Client_Id = c.Id, Car_RegNo = regNo, Car_Vin = vin, Car_Year = year };
                            context.Client_Vehicles.Add(car);
                            context.SaveChanges();
                            MessageBox.Show("Vehicle successfully added!");
                            _mw.RefreshClientVehicles();
                            _mw = null;
                            _user = null;
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong...");
                    }
                }
            }
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
