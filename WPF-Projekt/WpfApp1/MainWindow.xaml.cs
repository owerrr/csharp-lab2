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

    private void Client_Button_ViewVehicles_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_Header.Content = "Your Vehicles!";
        Content_Client_Vehicles.Visibility = Visibility.Visible;
        Content_Client_Liabilities.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
    }

    private void Client_Button_ViewLiabilities_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_Header.Content = "Your Liabilities!";
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        Content_Client_Liabilities.Visibility = Visibility.Visible;
        Content_Client_Informations.Visibility = Visibility.Collapsed;
    }

    private void Client_Button_ViewInformations_Click(object sender, RoutedEventArgs args)
    {
        Content_Client_Header.Content = "Your Informations!";
        Content_Client_Vehicles.Visibility = Visibility.Collapsed;
        Content_Client_Liabilities.Visibility = Visibility.Collapsed;
        Content_Client_Informations.Visibility = Visibility.Visible;
    }
}