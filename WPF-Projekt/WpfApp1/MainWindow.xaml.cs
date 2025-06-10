using System.IO;
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
        Content_Worker_Header.Content = "Client Vehicles";
        Content_Manager_Employees.Visibility = Visibility.Collapsed;
        Content_Worker_Clients.Visibility = Visibility.Collapsed;
        Content_Worker_Vehicles.Visibility = Visibility.Visible;

        Worker_LoadVehicles();
    }
    private void Worker_LoadVehicles()
    {

    }
    private void Worker_Button_ViewClients_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Current Clients";
        Content_Manager_Employees.Visibility = Visibility.Collapsed;
        Content_Worker_Clients.Visibility = Visibility.Collapsed;
        Content_Worker_Vehicles.Visibility = Visibility.Visible;

        Worker_LoadCLients();
    }
    private void Worker_LoadCLients()
    {

    }
    private void Manager_Button_ViewEmployees_Click(object sender, RoutedEventArgs args)
    {
        Content_Worker_Header.Content = "Current Employees";
        Content_Manager_Employees.Visibility = Visibility.Visible;
        Content_Worker_Clients.Visibility = Visibility.Collapsed;
        Content_Worker_Vehicles.Visibility = Visibility.Collapsed;

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

                var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition());

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
                    if(FoundedEmployeeModifyPermissions <= EmployeeModifyPermissions)
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
            if (empToDelete != null)
            {
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {empToDelete.Firstname} {empToDelete.Lastname} with Email: {empToDelete.Email}?", "Delete employee confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

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
        Content_Client_Vehicles.Visibility = Visibility.Visible;
        Content_Client_Liabilities.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
    }

    private void Client_Button_ViewHistory_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_Header.Content = "Your History!";
        Client_Info_SubmitButton.Visibility = Visibility.Collapsed;
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        Content_Client_Liabilities.Visibility = Visibility.Visible;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
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
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        Content_Client_Liabilities.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Visible;
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

        if (String.IsNullOrWhiteSpace(firstname)
            || String.IsNullOrWhiteSpace(lastname)
            || String.IsNullOrWhiteSpace(phonenumber)
            || String.IsNullOrWhiteSpace(postalcode)
            || String.IsNullOrWhiteSpace(city)
            || String.IsNullOrWhiteSpace(street)
            || String.IsNullOrWhiteSpace(buildingNo)
        )
        {
            MessageBox.Show("All required fields must be filled!");
            ClientInfoLoaded = false;
            return;
        }

        using (WorkshopDbContext context = new WorkshopDbContext())
        {
            bool userExists = context.Users.Any(x => x == _user);

            if (userExists)
            {
                Clients c = new Clients { Firstname = firstname, Lastname = lastname, Phonenumber = phonenumber, Postalcode = postalcode, City = city, Street = street, Building_No = buildingNo, User = _user };
                _user.Client_Id = c.Id;
                context.Clients.Add(c);
                context.Update(_user);
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Something went wrong..");
            }
        }
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
                MessageBoxResult result = MessageBox.Show($"Do you want to delete {vehicleToDelete.Car_Model} Registration Number: {vehicleToDelete.Car_RegNo}?", "Delete vehicle confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

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