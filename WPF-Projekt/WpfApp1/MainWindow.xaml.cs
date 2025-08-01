﻿using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Contexts;
using WpfApp1.Models;

namespace WpfApp1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Users _user { get; set; }
    public MainWindow(Users user)
    {
        InitializeComponent();
        Content_Worker.Visibility = Visibility.Collapsed;
        Content_Client.Visibility = Visibility.Collapsed;

        _user = user;
        FixUI();
    }

    private void FixUI()
    {
        Nav_WelcomeLabel.Content = $"Welcome {_user.Name}!";
        if (_user.Employee_Title_Id != null)
        {
            if(_user.Employee_Title_Id == 3 || _user.Employee_Title_Id == 4)
                Button_Manager_ViewEmployees.Visibility = Visibility.Visible;
            else
                Button_Manager_ViewEmployees.Visibility = Visibility.Collapsed;

            Buttons_Worker.Visibility = Visibility.Visible;
            Content_Worker.Visibility = Visibility.Visible;
        }
        else
        {
            Buttons_Client.Visibility = Visibility.Visible;
            Content_Client.Visibility = Visibility.Visible;
        }
            
    }
    private void Worker_Button_ManageVehicles_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Currently working on:";
        Content_Manager_Employees.Visibility = Visibility.Collapsed;
        Content_Worker_Clients.Visibility = Visibility.Collapsed;
        Content_Worker_ManageVehicles.Visibility = Visibility.Visible;
        Content_Worker_Clients_FoundClients_ViewVehicles.Visibility = Visibility.Collapsed;

        Worker_LoadVehicles();
    }

    public void Worker_RefreshClientVehiclesManageList()
    {
        Worker_LoadVehicles();
    }

    private void Worker_LoadVehicles()
    {
        List<EmployeeWorkOnVehicles> vehicles = new();
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            vehicles = context.EmployeeWorkOnVehicles.Where(x => !x.IsDone).ToList();
        }

        Content_Worker_ManageVehicles.Children.Clear();

        if (vehicles.Count > 0)
        {
            var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            grid.RowDefinitions.Add(new RowDefinition());

            var headerId = new Label { Content = "Id", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerModel = new Label { Content = "Model", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerYear = new Label { Content = "Year", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerRegNo = new Label { Content = "License plate", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerVIN = new Label { Content = "VIN", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(headerId, 0);
            Grid.SetColumn(headerModel, 1);
            Grid.SetColumn(headerYear, 2);
            Grid.SetColumn(headerRegNo, 3);
            Grid.SetColumn(headerVIN, 4);

            Grid.SetRow(headerId, 0);
            Grid.SetRow(headerModel, 0);
            Grid.SetRow(headerYear, 0);
            Grid.SetRow(headerRegNo, 0);
            Grid.SetRow(headerVIN, 0);

            grid.Children.Add(headerId);
            grid.Children.Add(headerModel);
            grid.Children.Add(headerYear);
            grid.Children.Add(headerRegNo);
            grid.Children.Add(headerVIN);

            int rowIdx = 1;
            foreach (EmployeeWorkOnVehicles vehicle in vehicles)
            {
                ClientVehicles veh = null;
                using (WorkshopDbContext context = new WorkshopDbContext())
                {
                    veh = context.Client_Vehicles.FirstOrDefault(x => x.Id == vehicle.ClientVehicle_Id);
                }
                if(veh == null)
                {
                    MessageBox.Show("Something went wrong...");
                    return;
                }

                grid.RowDefinitions.Add(new RowDefinition());

                var labelId = new Label { Content = $"{veh.Id}", VerticalAlignment = VerticalAlignment.Center };
                var labelModel = new Label { Content = $"{veh.Car_Model}", VerticalAlignment = VerticalAlignment.Center };
                var labelYear = new Label { Content = $"{veh.Car_Year}", VerticalAlignment = VerticalAlignment.Center };
                var labelRegNo = new Label { Content = $"{veh.Car_RegNo}", VerticalAlignment = VerticalAlignment.Center };
                var labelVIN = new Label { Content = $"{veh.Car_Vin}", VerticalAlignment = VerticalAlignment.Center };
                var buttonInfo = new Button { Content = "VIEW", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Worker_ManageVehicles_ViewVehicleButton_{vehicle.Id}" };

                buttonInfo.Click += Content_Worker_ManageVehicles_ViewVehicleButton_Click;

                Grid.SetColumn(labelId, 0);
                Grid.SetColumn(labelModel, 1);
                Grid.SetColumn(labelYear, 2);
                Grid.SetColumn(labelRegNo, 3);
                Grid.SetColumn(labelVIN, 4);
                Grid.SetColumn(buttonInfo, 5);

                Grid.SetRow(labelId, rowIdx);
                Grid.SetRow(labelModel, rowIdx);
                Grid.SetRow(labelYear, rowIdx);
                Grid.SetRow(labelRegNo, rowIdx);
                Grid.SetRow(labelVIN, rowIdx);
                Grid.SetRow(buttonInfo, rowIdx);

                rowIdx++;

                grid.Children.Add(labelId);
                grid.Children.Add(labelModel);
                grid.Children.Add(labelYear);
                grid.Children.Add(labelRegNo);
                grid.Children.Add(labelVIN);
                grid.Children.Add(buttonInfo);


            }

            Content_Worker_ManageVehicles.Children.Add(grid);
        }

    }

    private void Content_Worker_ManageVehicles_ViewVehicleButton_Click(object sender, RoutedEventArgs args)
    {
        int vehId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            EmployeeWorkOnVehicles veh = context.EmployeeWorkOnVehicles.FirstOrDefault(x => x.Id == vehId);
            if (veh != null)
            {
                ManageVehicleWindow mvw = new ManageVehicleWindow(veh, this);
                mvw.ShowDialog();
            }
        }
    }

    private void Worker_Button_ViewClients_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Current Clients";
        Content_Manager_Employees.Visibility = Visibility.Collapsed;
        Content_Worker_Clients.Visibility = Visibility.Visible;
        Content_Worker_ManageVehicles.Visibility = Visibility.Collapsed;
        Content_Worker_Clients_FoundClients_ViewVehicles.Visibility = Visibility.Collapsed;
        Content_Worker_Clients_SearchForClient.Visibility = Visibility.Visible;
        Content_Worker_Clients_FoundClients.Visibility = Visibility.Visible;
    }

    private void Worker_FilterClients_ClearButton_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Clients_LabelId.Clear();
        Content_Worker_Clients_LabelFullname.Clear();
        Content_Worker_Clients_LabelPhonenumber.Clear();
        Content_Worker_Clients_FoundClients.Children.Clear();
    }

    private void Worker_FilterClients_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Clients_FoundClients.Children.Clear();

        var id = Content_Worker_Clients_LabelId.Text;
        var fullname = Content_Worker_Clients_LabelFullname.Text;
        var phonenumber = Content_Worker_Clients_LabelPhonenumber.Text;

        bool isValidFiltered = false;
        List<Clients> filteredList = new();

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            
            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    filteredList = context.Clients.Where(x => x.Id == Convert.ToInt32(id)).ToList();
                    isValidFiltered = true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Id must be an integer!");
                    return;
                }
            }
            if (!String.IsNullOrEmpty(fullname))
            {
                if(filteredList.Count > 0)
                {
                    filteredList = filteredList.Where(x => (x.Firstname + " " + x.Lastname).ToLower().StartsWith(fullname.ToLower())).ToList();
                }
                else
                {
                    filteredList = context.Clients.Where(x => (x.Firstname + " " + x.Lastname).ToLower().StartsWith(fullname.ToLower())).ToList();
                }
                isValidFiltered = true;
            }
            if (!String.IsNullOrEmpty(phonenumber))
            {
                if(filteredList.Count > 0)
                {
                    filteredList = filteredList.Where(x => x.Phonenumber.StartsWith(phonenumber)).ToList();
                }
                else
                {
                    filteredList = context.Clients.Where(x => x.Phonenumber.StartsWith(phonenumber)).ToList();
                }
                isValidFiltered = true;
            }

            if (!isValidFiltered)
            {
                MessageBox.Show("At least one field must be filled!");
                return;
            }
        }

        var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var headerFullname = new Label { Content = "Fullname", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
        var headerPhonenumber = new Label { Content = "Phonenumber", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
        var headerCity = new Label { Content = "City", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
        var headerPostalcode = new Label { Content = "Postal code", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
        var headerStreet = new Label { Content = "Street", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
        var headerBuildingNo = new Label { Content = "Building", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };

        Grid.SetColumn(headerFullname, 0);
        Grid.SetColumn(headerPhonenumber, 1);
        Grid.SetColumn(headerCity, 2);
        Grid.SetColumn(headerPostalcode, 3);
        Grid.SetColumn(headerStreet, 4);
        Grid.SetColumn(headerBuildingNo, 5);

        Grid.SetRow(headerFullname, 0);
        Grid.SetRow(headerPhonenumber, 0);
        Grid.SetRow(headerCity, 0);
        Grid.SetRow(headerPostalcode, 0);
        Grid.SetRow(headerStreet, 0);
        Grid.SetRow(headerBuildingNo, 0);

        grid.Children.Add(headerFullname);
        grid.Children.Add(headerPhonenumber);
        grid.Children.Add(headerCity);
        grid.Children.Add(headerPostalcode);
        grid.Children.Add(headerStreet);
        grid.Children.Add(headerBuildingNo);

        int rowIdx = 1;

        foreach(Clients c in filteredList)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var labelFullname = new Label { Content = $"{c.Firstname} {c.Lastname}", VerticalAlignment = VerticalAlignment.Center };
            var labelPhonenumber = new Label { Content = $"{c.Phonenumber}", VerticalAlignment = VerticalAlignment.Center };
            var labelCity = new Label { Content = $"{c.City}", VerticalAlignment = VerticalAlignment.Center };
            var labelPostalcode = new Label { Content = $"{c.Postalcode}", VerticalAlignment = VerticalAlignment.Center };
            var labelStreet = new Label { Content = $"{c.Street}", VerticalAlignment = VerticalAlignment.Center };
            var labelBuildingNo = new Label { Content = $"{c.Building_No}", VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(labelFullname, 0);
            Grid.SetColumn(labelPhonenumber, 1);
            Grid.SetColumn(labelCity, 2);
            Grid.SetColumn(labelPostalcode, 3);
            Grid.SetColumn(labelStreet, 4);
            Grid.SetColumn(labelBuildingNo, 5);

            Grid.SetRow(labelFullname, rowIdx);
            Grid.SetRow(labelPhonenumber, rowIdx);
            Grid.SetRow(labelCity, rowIdx);
            Grid.SetRow(labelPostalcode, rowIdx);
            Grid.SetRow(labelStreet, rowIdx);
            Grid.SetRow(labelBuildingNo, rowIdx);

            grid.Children.Add(labelFullname);
            grid.Children.Add(labelPhonenumber);
            grid.Children.Add(labelCity);
            grid.Children.Add(labelPostalcode);
            grid.Children.Add(labelStreet);
            grid.Children.Add(labelBuildingNo);

            var button = new Button { Content = "VEHICLES", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Worker_Button_ViewClientVehicles_{c.Id}" };
            button.Click += Worker_Button_ViewClientVehicles_Click;
            Grid.SetColumn(button, 6);
            Grid.SetRow(button, rowIdx);

            grid.Children.Add(button);

            rowIdx++;
        }

        Content_Worker_Clients_FoundClients.Children.Add(grid);
    }

    private void Worker_FilterClients_BackButton_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Current Clients";
        Content_Worker_Clients_FoundClients_ViewVehicles_Data.Children.Clear();
        Content_Worker_Clients_FoundClients.Visibility = Visibility.Visible;
        Content_Worker_Clients_SearchForClient.Visibility = Visibility.Visible;
        Content_Worker_Clients_FoundClients_ViewVehicles.Visibility = Visibility.Collapsed;

    }

    private void Worker_Button_ViewClientVehicles_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Clients_FoundClients.Visibility = Visibility.Collapsed;
        Content_Worker_Clients_SearchForClient.Visibility = Visibility.Collapsed;
        Content_Worker_Clients_FoundClients_ViewVehicles.Visibility = Visibility.Visible;

        int clientId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        List<ClientVehicles> clientVehicles = new();

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            clientVehicles = context.Client_Vehicles.Where(x => x.Client_Id == clientId).ToList();
        }

        if (clientVehicles.Count > 0)
        {
            var grid = Content_Worker_Clients_FoundClients_ViewVehicles_Data;

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            grid.RowDefinitions.Add(new RowDefinition());

            var headerId = new Label { Content = "Id", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerModel = new Label { Content = "Model", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerYear = new Label { Content = "Year", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerRegNo = new Label { Content = "License plate", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerVIN = new Label { Content = "VIN", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };
            var headerIsMaintenanced = new Label { Content = "Is Maintenanced", FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(headerId, 0);
            Grid.SetColumn(headerModel, 1);
            Grid.SetColumn(headerYear, 2);
            Grid.SetColumn(headerRegNo, 3);
            Grid.SetColumn(headerVIN, 4);
            Grid.SetColumn(headerIsMaintenanced, 5);

            Grid.SetRow(headerId, 0);
            Grid.SetRow(headerModel, 0);
            Grid.SetRow(headerYear, 0);
            Grid.SetRow(headerRegNo, 0);
            Grid.SetRow(headerVIN, 0);
            Grid.SetRow(headerIsMaintenanced, 0);

            grid.Children.Add(headerId);
            grid.Children.Add(headerModel);
            grid.Children.Add(headerYear);
            grid.Children.Add(headerRegNo);
            grid.Children.Add(headerVIN);
            grid.Children.Add(headerIsMaintenanced);

            int rowIdx = 1;
            foreach(ClientVehicles veh in clientVehicles)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                var labelId = new Label { Content = $"{veh.Id}", VerticalAlignment = VerticalAlignment.Center };
                var labelModel = new Label { Content = $"{veh.Car_Model}", VerticalAlignment = VerticalAlignment.Center };
                var labelYear = new Label { Content = $"{veh.Car_Year}", VerticalAlignment = VerticalAlignment.Center };
                var labelRegNo = new Label { Content = $"{veh.Car_RegNo}", VerticalAlignment = VerticalAlignment.Center };
                var labelVIN = new Label { Content = $"{veh.Car_Vin}", VerticalAlignment = VerticalAlignment.Center };
                var labelIsMaintenanced = new CheckBox { IsChecked = veh.IsMaintenanced, IsEnabled = false, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };

                var buttonInfo = new Button { Content = "MANAGE", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Worker_ClientVehicles_ManageVehicleButton_{veh.Id}" };

                buttonInfo.Click += Worker_ViewClientVehicles_ManageVehicleButton_Click;

                Grid.SetColumn(labelId, 0);
                Grid.SetColumn(labelModel, 1);
                Grid.SetColumn(labelYear, 2);
                Grid.SetColumn(labelRegNo, 3);
                Grid.SetColumn(labelVIN, 4);
                Grid.SetColumn(labelIsMaintenanced, 5);
                Grid.SetColumn(buttonInfo, 6);

                Grid.SetRow(labelId, rowIdx);
                Grid.SetRow(labelModel, rowIdx);
                Grid.SetRow(labelYear, rowIdx);
                Grid.SetRow(labelRegNo, rowIdx);
                Grid.SetRow(labelVIN, rowIdx);
                Grid.SetRow(labelIsMaintenanced, rowIdx);
                Grid.SetRow(buttonInfo, rowIdx);

                rowIdx++;

                grid.Children.Add(labelId);
                grid.Children.Add(labelModel);
                grid.Children.Add(labelYear);
                grid.Children.Add(labelRegNo);
                grid.Children.Add(labelVIN);
                grid.Children.Add(labelIsMaintenanced);
                grid.Children.Add(buttonInfo);


        }
            Content_Worker_Clients_FoundClients_ViewVehiclesLabel.Content = $"Viewing client id {clientId} vehicles:";
            Content_Worker_Clients_FoundClients_ViewVehiclesLabel.Visibility = Visibility.Visible;
        }
        else
        {
            Content_Worker_Clients_FoundClients_ViewVehiclesLabel.Content = "Client have no vehicles registered!";
        }

    }

    private void Worker_ViewClientVehicles_ManageVehicleButton_Click(object sender, RoutedEventArgs args)
    {
        int vehId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        ClientVehicles foundVeh = null;
        using(WorkshopDbContext context = new WorkshopDbContext())
        {
            foundVeh = context.Client_Vehicles.FirstOrDefault(x => x.Id == vehId);
            if (foundVeh != null)
            {
                if (!foundVeh.IsMaintenanced)
                {
                    MessageBoxResult resultVehDone = MessageBox.Show($"Do you wish to create a new manage request on this vehicle?", "Manage Vehicle", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultVehDone == MessageBoxResult.Yes)
                    {
                        EmployeeWorkOnVehicles newVeh = new EmployeeWorkOnVehicles { ClientVehicle_Id = foundVeh.Id, Employee_Id = _user.Id, Date = DateOnly.FromDateTime(DateTime.Today), WorkOn = "" };
                        foundVeh.IsMaintenanced = true;
                        context.EmployeeWorkOnVehicles.Add(newVeh);
                        context.Client_Vehicles.Update(foundVeh);
                        context.SaveChanges();
                        ManageVehicleWindow mvw = new(newVeh, this);
                        mvw.ShowDialog();
                    }
                }
                else
                {
                    MessageBoxResult resultVehNotDone = MessageBox.Show($"Do you want to view active manage request on this vehicle?", "Manage Vehicle", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (resultVehNotDone == MessageBoxResult.Yes)
                    {
                        EmployeeWorkOnVehicles vehToWork = context.EmployeeWorkOnVehicles.OrderBy(x => x.Id).LastOrDefault(x => x.ClientVehicle_Id == foundVeh.Id);
                        if(vehToWork != null)
                        {
                            ManageVehicleWindow mvw = new(vehToWork, this);
                            mvw.ShowDialog();
                        }
                    }
                }
            }
            else
            {
            MessageBox.Show("Something went wrong...");
            }
        }        
    }


    private void Manager_Button_ViewEmployees_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Current Employees";
        Content_Manager_Employees.Visibility = Visibility.Visible;
        Content_Worker_Clients.Visibility = Visibility.Collapsed;
        Content_Worker_ManageVehicles.Visibility = Visibility.Collapsed;
        Content_Worker_Clients_FoundClients_ViewVehicles.Visibility = Visibility.Collapsed;

        Manager_LoadEmployees();
    }

    private bool Manager_LoadedEmployees { get; set; } = false;
    private void Manager_LoadEmployees()
    {
        if (Manager_LoadedEmployees || _user == null) return;

        Content_Manager_Employees.Children.Clear();
        bool validGenerated = false;

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            int EmployeeModifyPermissions = context.Employee_Titles.FirstOrDefault(x => x.Id == _user.Employee_Title_Id).Modify_Employees_Permissions;
            if (EmployeeModifyPermissions < 1) return;

            List<Users> Employees = context.Users.Where(x => x.Employee_Title_Id != null).ToList();

            if (Employees.Count > 0)
            {
                var scrollView = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled, MaxHeight = 170 };

                var grid = new Grid { Margin = new Thickness(0, 0, 0, 10), HorizontalAlignment = HorizontalAlignment.Center };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition{ Width = GridLength.Auto });

                grid.RowDefinitions.Add(new RowDefinition());

                var headerUsername = new Label { Content = "Username", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerFirstname = new Label { Content = "Firstname", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerLastname = new Label { Content = "Lastname", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerTitle = new Label { Content = "Title", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };

                Grid.SetColumn(headerUsername, 0);
                Grid.SetColumn(headerFirstname, 1);
                Grid.SetColumn(headerLastname, 2);
                Grid.SetColumn(headerTitle, 3);

                Grid.SetRow(headerUsername, 0);
                Grid.SetRow(headerFirstname, 0);
                Grid.SetRow(headerLastname, 0);
                Grid.SetRow(headerTitle, 0);

                grid.Children.Add(headerUsername);
                grid.Children.Add(headerFirstname);
                grid.Children.Add(headerLastname);
                grid.Children.Add(headerTitle);

                var rowIdx = 1;

                foreach(Users user in Employees)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    Employees empData = context.Employees.FirstOrDefault(e => e.Id == user.Employee_Id);
                    EmployeeTitles empTitle = context.Employee_Titles.FirstOrDefault(t => t.Id == user.Employee_Title_Id);

                    var empFirstname = empData.Firstname != null ? empData.Firstname : "Not set";
                    var empLastname = empData.Lastname != null ? empData.Lastname : "Not set";

                    var labelUsername = new Label { Content = $"{user.Name}", VerticalAlignment = VerticalAlignment.Center};
                    var labelFirstname = new Label { Content = $"{empFirstname}", VerticalAlignment = VerticalAlignment.Center};
                    var labelLastname = new Label { Content = $"{empLastname}", VerticalAlignment = VerticalAlignment.Center};
                    var labelEmpTitle = new Label { Content = $"{empTitle.Name}", VerticalAlignment = VerticalAlignment.Center};

                    Grid.SetColumn(labelUsername, 0);
                    Grid.SetColumn(labelFirstname, 1);
                    Grid.SetColumn(labelLastname, 2);
                    Grid.SetColumn(labelEmpTitle, 3);

                    Grid.SetRow(labelUsername, rowIdx);
                    Grid.SetRow(labelFirstname, rowIdx);
                    Grid.SetRow(labelLastname, rowIdx);
                    Grid.SetRow(labelEmpTitle, rowIdx);

                    grid.Children.Add(labelUsername);
                    grid.Children.Add(labelFirstname);
                    grid.Children.Add(labelLastname);
                    grid.Children.Add(labelEmpTitle);

                    //
                    int FoundedEmployeeModifyPermissions = empTitle.Modify_Employees_Permissions;
                    if(FoundedEmployeeModifyPermissions <= EmployeeModifyPermissions && _user.Id != user.Id)
                    {
                        var button = new Button { Content = "MODIFY", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Manager_Employee_Modify_{empData.Id}" };
                        var button2 = new Button { Content = "X", Style = (Style)Application.Current.FindResource("Client_Vehicle_DeleteBtn"), VerticalAlignment = VerticalAlignment.Center, Name = $"Manager_Employee_Delete_{empData.Id}" };
                        button.Click += Manager_Employee_Edit_Click;
                        button2.Click += Manager_Employee_Delete_Click;
                        Grid.SetColumn(button, 4);
                        Grid.SetColumn(button2, 5);
                        Grid.SetRow(button, rowIdx);
                        Grid.SetRow(button2, rowIdx);
                        grid.Children.Add(button);
                        grid.Children.Add(button2);
                    }

                    rowIdx++;
                }

                scrollView.Content = grid;

                Content_Manager_Employees.Children.Add(scrollView);

                validGenerated = true;
            }
            else
            {
                var label = new Label { Content = "There are no Employees, huh?", HorizontalContentAlignment = HorizontalAlignment.Center };

                validGenerated = true;
            }
        }

        if (validGenerated)
        {
            var submitButton = new Button { Style = (Style)Application.Current.FindResource("VehicleAddButtonStyle"), Content = "Add new employee", Margin = new Thickness(0, 10, 0, 10) };
            submitButton.Click += Manager_AddEmployee_Click;
            var refreshButton = new Button { Style = (Style)Application.Current.FindResource("VehicleRefreshButtonStyle"), Content = "Refresh employees list", Margin = new Thickness(0, 0, 0, 10) };
            refreshButton.Click += Manager_RefreshEmployees_Click;
            Content_Manager_Employees.Children.Add(submitButton);
            Content_Manager_Employees.Children.Add(refreshButton);

            Manager_LoadedEmployees = true;
        }
    }

    private void Manager_Employee_Edit_Click(object sender, RoutedEventArgs args)
    {
        int empId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        Employees empToEdit;
        Users empUser = null;
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            empToEdit = context.Employees.FirstOrDefault(x => x.Id == empId);
            empUser = context.Users.FirstOrDefault(x => x.Employee_Id == empId);
        }
        if (empToEdit != null && empUser != null)
        {
            AddEmployeeWindow ademp = new(_user, this, empToEdit, empUser);
            ademp.ShowDialog();
        }
        else
        {
            MessageBox.Show("Something went wrong...");
        }
    }
    private void Manager_Employee_Delete_Click(object sender, RoutedEventArgs args)
    {
        int empId = Convert.ToInt32((sender as Button).Name.Split("_").Last());

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            var empToDelete = context.Employees.FirstOrDefault(x => x.Id == empId);
            var empAccToDelete = context.Users.FirstOrDefault(x => x.Employee_Id == empId);
            if (empToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {empToDelete.Firstname} {empToDelete.Lastname}", "Delete employee confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    context.Users.Remove(empAccToDelete);
                    context.SaveChanges();
                    context.Employees.Remove(empToDelete);
                    context.SaveChanges();
                    MessageBox.Show("Employee successfully deleted!");
                    RefreshEmployees();
                }
            }
        }
    }
    private void Manager_AddEmployee_Click(object sender, RoutedEventArgs args)
    {
        AddEmployeeWindow ademp = new(_user, this);
        ademp.ShowDialog();
    }
    private void Manager_RefreshEmployees_Click(object sender, RoutedEventArgs args)
    {
        RefreshEmployees();
    }
    public void RefreshEmployees()
    {
        Manager_LoadedEmployees = false;
        Manager_LoadEmployees();
    }

    private void LogOut_Button_Click(object sender, RoutedEventArgs args)
    {
        _user = null;
        LoginWindow lw = new();
        this.Close();
        lw.Show();
    }


    // client buttons

    private bool ClientVehiclesLoaded { get; set; } = false;
    private void Client_Button_ViewVehicles_Click(object sender, RoutedEventArgs args)
    {
        if (!ClientVehiclesLoaded)
        {
            ClientVehiclesLoaded = true;
            LoadClientVehicles();
        }

        Content_Client_Header.Content = "Your Vehicles!";
        Client_Info_SubmitButton.Visibility = Visibility.Collapsed;
        Client_Info_ChangePasswordButton.Visibility = Visibility.Collapsed;
        Content_Client_Vehicles.Visibility = Visibility.Visible;
        StackPanel_Client_VehicleHistory.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
    }

    private bool ClientVehicleHistoryLoaded { get; set; } = false;
    private void Client_Button_ViewHistory_Click(object sender, RoutedEventArgs args)
    {
        if (!ClientVehicleHistoryLoaded)
        {
            ClientVehicleHistoryLoaded = true;
            LoadClientVehicleHistory();
        }

        Content_Client_Header.Content = "Your History!";
        Client_Info_SubmitButton.Visibility = Visibility.Collapsed;
        Client_Info_ChangePasswordButton.Visibility = Visibility.Collapsed;
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        StackPanel_Client_VehicleHistory.Visibility = Visibility.Visible;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
    }
    private class ClientVehicleHistoryByVehicle
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string RegNo { get; set; }
        public string MaintenanceDate { get; set; }
        public string totalCost { get; set; }
        public string WorkDone { get; set; }
    }
    private List<ClientVehicleHistoryByVehicle> _clientVehicleHistoryList = new();
    private int _filteredClientVehicleId { get; set; } = -1;
    private void LoadClientVehicleHistory()
    {

        Content_Client_VehicleHistory.Children.Clear();

        List<ClientVehicles> clientVehicles = new();

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            if (_filteredClientVehicleId != -1)
            {
                clientVehicles = context.Client_Vehicles.Where(x => x.Id == _filteredClientVehicleId).ToList();
            }
            else
            {
                clientVehicles = context.Client_Vehicles.Where(x => x.Client_Id == _user.Client_Id).ToList();
            }

            if (clientVehicles.Count > 0)
            {
                var scrollView = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled, MaxHeight = 170 };

                var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                grid.RowDefinitions.Add(new RowDefinition());

                var headerVehicleName = new Label { Content = "Vehicle", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerVehicleRegNo = new Label { Content = "License plate", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerVehicleMaintenanceDate = new Label { Content = "Date", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerVehicleMaintenanePrice = new Label { Content = "Price", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                var headerVehicleIsFinished = new Label { Content = "Finished", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };

                Grid.SetColumn(headerVehicleName, 0);
                Grid.SetColumn(headerVehicleRegNo, 1);
                Grid.SetColumn(headerVehicleMaintenanceDate, 2);
                Grid.SetColumn(headerVehicleMaintenanePrice, 3);
                Grid.SetColumn(headerVehicleIsFinished, 4);

                Grid.SetRow(headerVehicleName, 0);
                Grid.SetRow(headerVehicleRegNo, 0);
                Grid.SetRow(headerVehicleMaintenanceDate, 0);
                Grid.SetRow(headerVehicleMaintenanePrice, 0);
                Grid.SetRow(headerVehicleIsFinished, 0);

                grid.Children.Add(headerVehicleName);
                grid.Children.Add(headerVehicleRegNo);
                grid.Children.Add(headerVehicleMaintenanceDate);
                grid.Children.Add(headerVehicleMaintenanePrice);
                grid.Children.Add(headerVehicleIsFinished);

                int rowIdx = 1;

                List<EmployeeWorkOnVehicles> empWorkDoneList = context.EmployeeWorkOnVehicles.Where(x => clientVehicles.Select(v => v.Id).Contains(x.ClientVehicle_Id)).OrderByDescending(x => x.Date).ToList();
                foreach (EmployeeWorkOnVehicles empWorkDone in empWorkDoneList)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    var totalCost = empWorkDone.WorkOn
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries))
                        .Sum(x => decimal.Parse(x[1], CultureInfo.InvariantCulture))
                        .ToString("0.00");

                    var vehicleNameLabel = new Label { Content = $"{clientVehicles.FirstOrDefault(x => x.Id == empWorkDone.ClientVehicle_Id).Car_Model}", VerticalAlignment = VerticalAlignment.Center };
                    var vehicleRegNo = new Label { Content = $"{clientVehicles.FirstOrDefault(x => x.Id == empWorkDone.ClientVehicle_Id).Car_RegNo}", VerticalAlignment = VerticalAlignment.Center };
                    var vehicleMaintenanceDate = new Label { Content = $"{empWorkDone.Date}", VerticalAlignment = VerticalAlignment.Center };
                    var vehicleMaintenancePrice = new Label { Content = $"{totalCost}", VerticalAlignment = VerticalAlignment.Center };
                    var vehicleWorkDone = new Label { Content = $"{(empWorkDone.IsDone ? "Done" : "In progress")}", VerticalAlignment = VerticalAlignment.Center };

                    _clientVehicleHistoryList.Add(new ClientVehicleHistoryByVehicle { Id = empWorkDone.Id, Name = vehicleNameLabel.Content.ToString(), RegNo = vehicleRegNo.Content.ToString(), MaintenanceDate = vehicleMaintenanceDate.Content.ToString(), totalCost = vehicleMaintenancePrice.Content.ToString(), WorkDone = vehicleWorkDone.Content.ToString() });

                    Grid.SetColumn(vehicleNameLabel, 0);
                    Grid.SetColumn(vehicleRegNo, 1);
                    Grid.SetColumn(vehicleMaintenanceDate, 2);
                    Grid.SetColumn(vehicleMaintenancePrice, 3);
                    Grid.SetColumn(vehicleWorkDone, 4);

                    Grid.SetRow(vehicleNameLabel, rowIdx);
                    Grid.SetRow(vehicleRegNo, rowIdx);
                    Grid.SetRow(vehicleMaintenanceDate, rowIdx);
                    Grid.SetRow(vehicleMaintenancePrice, rowIdx);
                    Grid.SetRow(vehicleWorkDone, rowIdx);

                    grid.Children.Add(vehicleNameLabel);
                    grid.Children.Add(vehicleRegNo);
                    grid.Children.Add(vehicleMaintenanceDate);
                    grid.Children.Add(vehicleMaintenancePrice);
                    grid.Children.Add(vehicleWorkDone);

                    var button = new Button { Width=100, Content = "INFO", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Client_VehicleHistory_InfoButton_{empWorkDone.Id}" };
                    button.Click += Client_VehicleHistory_InfoButton_Click;
                    Grid.SetColumn(button, 5);
                    Grid.SetRow(button, rowIdx);
                    grid.Children.Add(button);

                    rowIdx++;
                }

                Client_VehicleHistoryFilter_Generate();

                scrollView.Content = grid;
                Content_Client_VehicleHistory.Children.Add(scrollView);
            }
            else
            {

            }
        }
    }

    private bool isFilterGenerated { get; set; } = false;
    private void Client_VehicleHistoryFilter_Generate()
    {
        if (!isFilterGenerated)
        {
            isFilterGenerated = true;
            var filterComboBox = Content_Client_VehicleHistory_Filter;

            filterComboBox.SelectionChanged += Client_VehicleHistory_FilterVehicles;
            filterComboBox.ItemsSource = _clientVehicleHistoryList;
            filterComboBox.DisplayMemberPath = "Name";
            filterComboBox.SelectedValuePath = "Id";
        }
    }

    private void Client_VehicleHistory_FilterVehicles(object sender, RoutedEventArgs args)
    {
        _filteredClientVehicleId = (int)Content_Client_VehicleHistory_Filter.SelectedValue;
        Client_VehicleHistory_Refresh();
    }

    private void Client_VehicleHistory_FilterResetButton_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_VehicleHistory_Filter.SelectionChanged -= Client_VehicleHistory_FilterVehicles;
        _filteredClientVehicleId = -1;
        Content_Client_VehicleHistory_Filter.SelectedIndex = -1;
        Content_Client_VehicleHistory_Filter.SelectionChanged += Client_VehicleHistory_FilterVehicles;
        Client_VehicleHistory_Refresh();
    }

    private void Client_VehicleHistory_Refresh()
    {
        ClientVehicleHistoryLoaded = false;
        LoadClientVehicleHistory();
    }

    private void Client_VehicleHistory_InfoButton_Click(object sender, RoutedEventArgs args)
    {
        int vehMaintenanceId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        using(WorkshopDbContext context = new WorkshopDbContext())
        {
            EmployeeWorkOnVehicles veh = context.EmployeeWorkOnVehicles.FirstOrDefault(x => x.Id == vehMaintenanceId);
            if(veh != null)
            {
                var workDoneSplitted = veh.WorkOn.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Split(':', StringSplitOptions.RemoveEmptyEntries)).ToList();
                string result = String.Join("\n", workDoneSplitted.Select(x => $"{x[0]}, cena: {x[1]}, {(x[2] == "1" ? "wykonane" : "w trakcie")}"));
                MessageBox.Show($"info {vehMaintenanceId}:\n{result}");
            }
            else
            {
                MessageBox.Show("Something went wrong...");
            }
        }
    }

    private bool ClientInfoLoaded { get; set; } = false;
    private void Client_Button_ViewInformations_Click(object sender, RoutedEventArgs args)
    {
        if (!ClientInfoLoaded)
        {
            ClientInfoLoaded = true;
            LoadClientInfo();
        }

        Content_Client_Header.Content = "Your Informations!";
        Client_Info_SubmitButton.Visibility = Visibility.Visible;
        Client_Info_ChangePasswordButton.Visibility = Visibility.Visible;
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        StackPanel_Client_VehicleHistory.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Visible;
    }

    private bool isValidMail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private void Client_Info_SubmitButton_Verify(object sender, RoutedEventArgs args)
    {
        string firstname = Client_Info_TxtBox_Firstname.Text;
        string lastname = Client_Info_TxtBox_Lastname.Text;
        string phonenumber = Client_Info_TxtBox_Phonenumber.Text;
        string postalcode = Client_Info_TxtBox_Postalcode.Text;
        string city = Client_Info_TxtBox_City.Text;
        string street = Client_Info_TxtBox_Street.Text;
        string buildingNo = Client_Info_TxtBox_BuildingNo.Text;
        string email = Client_Info_TxtBox_Email.Text;

        if (String.IsNullOrWhiteSpace(firstname)
            || String.IsNullOrWhiteSpace(lastname)
            || String.IsNullOrWhiteSpace(phonenumber)
            || String.IsNullOrWhiteSpace(postalcode)
            || String.IsNullOrWhiteSpace(city)
            || String.IsNullOrWhiteSpace(street)
            || String.IsNullOrWhiteSpace(buildingNo)
            || String.IsNullOrWhiteSpace(email)
        )
        {
            MessageBox.Show("All required fields must be filled!");
            ClientInfoLoaded = false;
            return;
        }

        if(!Regex.IsMatch(phonenumber, @"^\d{9}$"))
        {
            MessageBox.Show("Phone number is invalid!\nValid: xxxxxxxxx");
            return;
        }

        if (!isValidMail(email))
        {
            MessageBox.Show("Email is invalid! example email: test@test.com");
            return;
        }

        if (!Regex.IsMatch(postalcode, @"^\d{2}-\d{3}$"))
        {
            MessageBox.Show("Postal code is not valid!\nWe serve in polish system: xx-xxx");
            return;
        }

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            bool mailExists = context.Users.Any(x => x.Email == email);
            if (_user.Email != email && mailExists)
            {
                MessageBox.Show("Email is already taken by another account, Huh?");
                return;
            }
            bool userExists = context.Users.Any(x => x == _user);

            if (userExists)
            {
                if(_user.Client_Id != null)
                {
                    Clients c = context.Clients.FirstOrDefault(x => x.Id == _user.Client_Id);
                    c.Firstname = firstname;
                    c.Lastname = lastname;
                    c.Phonenumber = phonenumber;
                    c.Postalcode = postalcode;
                    c.City = city;
                    c.Street = street;
                    c.Building_No = buildingNo;
                    _user.Email = email;
                    context.Clients.Update(c);
                    context.Users.Update(_user);
                    context.SaveChanges();
                }
                else
                {
                    Clients c = new Clients { Firstname = firstname, Lastname = lastname, Phonenumber = phonenumber, Postalcode = postalcode, City = city, Street = street, Building_No = buildingNo };
                    context.Clients.Add(c);
                    context.SaveChanges();
                    c.User = _user;
                    _user.Client_Id = c.Id;
                    _user.Email = email;
                    context.Clients.Update(c);
                    context.Users.Update(_user);
                    context.SaveChanges();
                }
                ClientVehiclesLoaded = false;
                MessageBox.Show("Successfully saved your info!");
            }
            else
            {
                MessageBox.Show("Something went wrong..");
            }
        }
    }

    private void Client_Info_ChangPasswordButton_Click(object sender, RoutedEventArgs args)
    {
        ChangePasswordWindow cpw = new(this, _user);
        cpw.ShowDialog();
    }

    private void LoadClientInfo()
    {
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            bool userExists = context.Users.Any(x => x == _user);

            if (userExists)
            {
                Clients? c = context.Clients.FirstOrDefault(x => x.Id == _user.Client_Id);
                if(c != null)
                {
                    Client_Info_TxtBox_Firstname.Text = c.Firstname;
                    Client_Info_TxtBox_Lastname.Text = c.Lastname;
                    Client_Info_TxtBox_Phonenumber.Text = c.Phonenumber;
                    Client_Info_TxtBox_Postalcode.Text = c.Postalcode;
                    Client_Info_TxtBox_City.Text = c.City;
                    Client_Info_TxtBox_Street.Text = c.Street;
                    Client_Info_TxtBox_BuildingNo.Text = c.Building_No;
                    Client_Info_TxtBox_Email.Text = _user.Email;
                }
            }
            else
            {
                MessageBox.Show("Something went wrong..");
            }
        }
    }

    public void RefreshClientVehicles()
    {
        LoadClientVehicles();
    }

    private void Client_Vehicles_DeleteVehicle(object sender, RoutedEventArgs args)
    {
        int carId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        //MessageBox.Show($"{carId}");
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            var vehicleToDelete = context.Client_Vehicles.FirstOrDefault(x => x.Id == carId);
            if (vehicleToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {vehicleToDelete.Car_Model} Registration Number: {vehicleToDelete.Car_RegNo}?\n*deleting this vehicle will delete its maintenance history!", "Delete vehicle confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (vehicleToDelete.IsMaintenanced)
                    {
                        MessageBox.Show("You cannot delete vehicle if its being maintenanced!");
                        return;
                    }
                    List<EmployeeWorkOnVehicles> vehicleWorkToDelete = context.EmployeeWorkOnVehicles.Where(x => x.ClientVehicle_Id == vehicleToDelete.Id).ToList();
                    foreach(EmployeeWorkOnVehicles vehWork in vehicleWorkToDelete)
                    {
                        context.EmployeeWorkOnVehicles.Remove(vehWork);
                    }
                    context.Client_Vehicles.Remove(vehicleToDelete as ClientVehicles);
                    context.SaveChanges();
                    MessageBox.Show("Vehicle successfully deleted!");
                    RefreshClientVehicles();
                }
            } 
        }
    }

    private void Client_Vehicles_EditVehicle(object sender, RoutedEventArgs args)
    {
        int carId = Convert.ToInt32((sender as Button).Name.Split("_").Last());
        ClientVehicles vehicleToEdit;
        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            vehicleToEdit = context.Client_Vehicles.FirstOrDefault(x => x.Id == carId);
        }
        if(vehicleToEdit != null)
        {
            AddVehicleWindow adw = new(_user, this, vehicleToEdit);
            adw.ShowDialog();
        }
        else
        {
            MessageBox.Show("Something went wrong...");
        }
    }

    private void LoadClientVehicles()
    {

        bool validGenerated = false;

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            bool userExists = context.Users.Any(x => x == _user);

            Content_Client_Vehicles.Children.Clear();

            if (userExists)
            {
                Clients? c = context.Clients.FirstOrDefault(x => x.Id == _user.Client_Id);
                if (c != null)
                {
                    var vehiclesFound = context.Client_Vehicles.Where(v => v.Client_Id == c.Id).ToList();
                    if (vehiclesFound.Count > 0)
                    {
                        var scrollView = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled, MaxHeight = 170 };

                        var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.RowDefinitions.Add(new RowDefinition());
                        var headerModel = new Label { Content = "Model", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                        var headerYear = new Label { Content = "Year", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                        var headerRegNo = new Label { Content = "Registration No.", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };
                        var headerActionBtn = new Label { Content = "Action buttons", VerticalAlignment = VerticalAlignment.Center, FontWeight = FontWeights.Bold };

                        Grid.SetColumn(headerModel, 0);
                        Grid.SetColumn(headerYear, 1);
                        Grid.SetColumn(headerRegNo, 2);
                        Grid.SetColumn(headerActionBtn, 4);

                        Grid.SetRow(headerModel, 0);
                        Grid.SetRow(headerYear, 0);
                        Grid.SetRow(headerRegNo, 0);
                        Grid.SetRow(headerActionBtn, 0);

                        grid.Children.Add(headerModel);
                        grid.Children.Add(headerYear);
                        grid.Children.Add(headerRegNo);
                        grid.Children.Add(headerActionBtn);

                        var rowIdx = 1;

                        foreach (ClientVehicles veh in vehiclesFound)
                        {
                            grid.RowDefinitions.Add(new RowDefinition());

                            var labelModel = new Label { Content = $"{veh.Car_Model}", VerticalAlignment = VerticalAlignment.Center };
                            var labelYear = new Label { Content = $"{veh.Car_Year}", VerticalAlignment = VerticalAlignment.Center };
                            var labelRegNo = new Label { Content = $"{veh.Car_RegNo}", VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(labelModel, 0);
                            Grid.SetColumn(labelYear, 1);
                            Grid.SetColumn(labelRegNo, 2);
                            Grid.SetRow(labelModel, rowIdx);
                            Grid.SetRow(labelYear, rowIdx);
                            Grid.SetRow(labelRegNo, rowIdx);
                            
                         
                            grid.Children.Add(labelModel);
                            grid.Children.Add(labelYear);
                            grid.Children.Add(labelRegNo);

                            var button = new Button { Content = "MODIFY", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"Client_Vehicle_Modify_{veh.Id}" };
                            var button2 = new Button { Content = "X", Style = (Style)Application.Current.FindResource("Client_Vehicle_DeleteBtn"), VerticalAlignment = VerticalAlignment.Center, Name = $"Client_Vehicle_Delete_{veh.Id}" };
                            button.Click += Client_Vehicles_EditVehicle;
                            button2.Click += Client_Vehicles_DeleteVehicle;
                            Grid.SetColumn(button, 3);
                            Grid.SetColumn(button2, 4);
                            Grid.SetRow(button, rowIdx);
                            Grid.SetRow(button2, rowIdx);
                            grid.Children.Add(button);
                            grid.Children.Add(button2);

                            rowIdx += 1;
                            validGenerated = true;
                        }
                        scrollView.Content = grid;
                        Content_Client_Vehicles.Children.Add(scrollView);
                    }
                    else
                    {
                        var label = new Label { Content = "You don't have any vehicles signed yet!", VerticalAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                        var label2 = new Label { Content = "Click button below to sign new vehicle!", VerticalAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };
                        var stackpanel = new StackPanel();
                        stackpanel.Children.Add(label);
                        stackpanel.Children.Add(label2);
                        Content_Client_Vehicles.Children.Add(stackpanel);

                        validGenerated = true;
                    }
                }
                else
                {
                    var label = new Label { Content = "Your personal information are incomplete!", VerticalAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center };

                    var txtblock = new TextBlock { TextAlignment = TextAlignment.Center };
                    var hyperlink = new Hyperlink(new Run("Click here"));
                    hyperlink.Click += Client_Button_ViewInformations_Click;
                    var run2 = new Run(" to fill it up!");
                    txtblock.Inlines.Add(hyperlink);
                    txtblock.Inlines.Add(run2);

                    var stackpanel = new StackPanel();
                    stackpanel.Children.Add(label);
                    stackpanel.Children.Add(txtblock);
                    Content_Client_Vehicles.Children.Add(stackpanel);
                }
            }
            else
            {
                MessageBox.Show("Something went wrong..");
            }
        }

        if (validGenerated)
        {
            var submitButton = new Button { Style = (Style)Application.Current.FindResource("VehicleAddButtonStyle"), Content = "Add new vehicle", Margin = new Thickness(0, 10, 0, 10) };
            submitButton.Click += Client_AddVehicle;
            var refreshButton = new Button { Style = (Style)Application.Current.FindResource("VehicleRefreshButtonStyle"), Content = "Refresh vehicle list", Margin = new Thickness(0, 0, 0, 10) };
            refreshButton.Click += Client_RefreshVehicles;
            Content_Client_Vehicles.Children.Add(submitButton);
            Content_Client_Vehicles.Children.Add(refreshButton);
        }
        
    }

    private void Client_AddVehicle(object sender, RoutedEventArgs args)
    {
        AddVehicleWindow adw = new(_user, this);
        adw.ShowDialog();
    }

    private void Client_RefreshVehicles(object sender, RoutedEventArgs args)
    {
        LoadClientVehicles();
    }
}