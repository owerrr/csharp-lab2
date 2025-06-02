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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_FocusTxtBox(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Label).Name)
            {
                case "Login_Label_Username":
                    Login_TxtBox_Username.Focus();
                    break;
                case "Login_Label_Password":
                    Login_TxtBox_Password.Focus();
                    break;
                default:break;
            }
        }

        private void SignUp_OpenForm(object sender, RoutedEventArgs args)
        {
            
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
        }

        private void Register_FocusTxtBox(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Label).Name)
            {
                case "Register_Label_Username":
                    Register_TxtBox_Username.Focus();
                    break;
                case "Register_Label_Password":
                    Register_TxtBox_Password.Focus();
                    break;
                default: break;
            }
        }

        private void SignIn_OpenForm(object sender, RoutedEventArgs args)
        {
            RegisterForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }
    }
}
