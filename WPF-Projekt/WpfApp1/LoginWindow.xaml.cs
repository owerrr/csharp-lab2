using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
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
using WpfApp1.Contexts;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Users _user { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            _user = null;
        }

        // top bar
        
        private void Bar_Minimize_Click(object sender, RoutedEventArgs args)
        {
            WindowState = WindowState.Minimized;
        }

        private void Bar_Close_Click(object sender, RoutedEventArgs args)
        {
            Close();
        }

        private void Border_MoveOnMouseDown(object sender, MouseButtonEventArgs args)
        {
            if (args.ChangedButton == MouseButton.Left)
                DragMove();
        }

        // login
        public void OnLoginSuccessful()
        {
            MainWindow mw = new MainWindow(_user);
            this.Close();
            mw.Show();
        }

        private  void Login_Validate(object sender, RoutedEventArgs args)
        {
            var userName = Login_TxtBox_Username.Text;
            var userPswd = Login_TxtBox_Password.Password;

            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(userPswd))
            {
                MessageBox.Show("Username or Password cannot be empty!");
                return;
            }

            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                var user =  context.Users.FirstOrDefault(x => x.Name == userName);

                if (user != null && BCrypt.Net.BCrypt.Verify(userPswd, user.Password))
                {
                    _user = user;
                    OnLoginSuccessful();
                }
                else
                {
                    MessageBox.Show("Invalid username or password! If you don't have an account create a new one!");
                }
            }
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
            Login_TxtBox_Username.Text = "";
            Login_TxtBox_Password.Password = "";
            Bar_Label_Header.Content = "Workshop - Sign Up";
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
        }


// register

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
            Register_TxtBox_Username.Text = "";
            Register_TxtBox_Password.Password = "";
            Bar_Label_Header.Content = "Workshop - Sign In";
            RegisterForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }

        private void OnRegisterSuccessful()
        {
            MessageBox.Show("Successfully created an account!");
            RegisterForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }

        private void Register_Validate(object sender, RoutedEventArgs args)
        {
            var userName = Register_TxtBox_Username.Text;
            var userPswd_Plain = Register_TxtBox_Password.Password;

            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(userPswd_Plain))
            {
                MessageBox.Show("Username or Password cannot be empty!");
                return;
            }

            var userPswd = BCrypt.Net.BCrypt.HashPassword(userPswd_Plain);

            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                bool userExists = context.Users.Any(x => x.Name == userName);

                if (userExists)
                {
                    MessageBox.Show("Username already exists!");
                }
                else
                {
                    var user = new Users { Name=userName, Password=userPswd, Employee_Title_Id=null };
                    context.Users.Add(user);
                    context.SaveChanges();
                    OnRegisterSuccessful();
                    
                }
            }
        }

        private void Login_TxtBox_CheckForEnterClicked(object sender, KeyEventArgs args)
        {
            if (args.Key == Key.Enter)
                Login_Validate(sender, args);
;        }
    }
}
