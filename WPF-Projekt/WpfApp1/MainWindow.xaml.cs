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
            Buttons_Worker.Visibility = Visibility.Visible;
            Content_Worker.Visibility = Visibility.Visible;
        }
        else
        {
            Buttons_Client.Visibility = Visibility.Visible;
            Content_Client.Visibility = Visibility.Visible;
        }
            
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

    private void Client_Button_ViewLiabilities_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_Header.Content = "Your Liabilities!";
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
                        foreach (ClientVehicles veh in vehiclesFound)
                        {
                            var grid = new Grid { Margin = new Thickness(0, 0, 0, 10) };
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                            var label = new Label { Content = $"{veh.Car_Model} {veh.Car_Year} | {veh.Car_RegNo}", VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(label, 0);
                            grid.Children.Add(label);

                            var button = new Button { Content = "MODIFY", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center };
                            Grid.SetColumn(button, 1);
                            grid.Children.Add(button);

                            Content_Client_Vehicles.Children.Add(grid);

                            validGenerated = true;
                        }
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
        AddVehicleWindow adw = new();
        adw.ShowDialog();
    }

    private void Client_RefreshVehicles(object sender, RoutedEventArgs args)
    {
        LoadClientVehicles();
    }
}