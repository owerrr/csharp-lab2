using BCrypt.Net;
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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private const string _mailVerificationCode = "1111";
        private Users _userToChange { get; set; }
        private Window _window { get; set; }
        public ChangePasswordWindow(Window window, Users userToChange = null)
        {
            InitializeComponent();
            _window = window;
            if (userToChange != null)
                _userToChange = userToChange;
            FixUI();
        }

        private void FixUI()
        {
            if(_window is LoginWindow)
            {
                ForgotPassword_StackPanel.Visibility = Visibility.Visible;
                ForgotPassword_Content_BeforeEmailCode.Visibility = Visibility.Visible;
                ForgotPassword_Content_AfterEmailCode.Visibility = Visibility.Collapsed;
                ChangePassword_StackPanel.Visibility = Visibility.Collapsed;
            }
            else if(_window is AddEmployeeWindow)
            {
                ForgotPassword_StackPanel.Visibility = Visibility.Collapsed;
                ChangePassword_StackPanel.Visibility = Visibility.Visible;
                ChangePassword_Password_Old.Visibility = Visibility.Collapsed;
                ChangePassword_Label_Old.Visibility = Visibility.Collapsed;
            }
            else if(_window is MainWindow)
            {
                ForgotPassword_StackPanel.Visibility = Visibility.Collapsed;
                ChangePassword_StackPanel.Visibility = Visibility.Visible;
                ChangePassword_Password_Old.Visibility = Visibility.Visible;
                ChangePassword_Label_Old.Visibility = Visibility.Visible;
            }
        }

        private void BackToWindow()
        {
            _userToChange = null;
            this.Close();
        }

        private void ChangePassword_ActionButton_Click(object sender, RoutedEventArgs args)
        {
            if (!(_window is AddEmployeeWindow))
            {
                var oldPassword = ChangePassword_Password_Old.Password;

                if (String.IsNullOrEmpty(oldPassword))
                {
                    MessageBox.Show("Password cannot be empty!");
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(oldPassword, _userToChange.Password))
                {
                    MessageBox.Show("Invalid password!");
                    return;
                }
            }
            
            var newPassword = ChangePassword_Password_New.Password;
            var newPasswordConfirm = ChangePassword_Password_NewConfirm.Password;

            if (String.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Password cannot be empty!");
                return;
            }

            if (newPassword != newPasswordConfirm)
            {
                MessageBox.Show("Passwords are not the same!");
                return;
            }

            if (BCrypt.Net.BCrypt.Verify(newPassword, _userToChange.Password))
            {
                MessageBox.Show("New password cannot be the same as old password!");
                return;
            }

            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                _userToChange.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                context.Users.Update(_userToChange);
                context.SaveChanges();
                MessageBox.Show("Password successfully changed!");
                BackToWindow();
            }
        }

        private void ChangePassword_BackButton_Click(object sender, RoutedEventArgs args)
        {
            BackToWindow();
        }
        
        private void ForgotPassword_BackButton_Click(object sender, RoutedEventArgs args)
        {
            BackToWindow();
        }
        private void ForgotPassword_ResendCodeButton_Click(object sender, RoutedEventArgs args)
        {
            //
        }
        private void ForgotPassword_ActionButton_Click(object sender, RoutedEventArgs args)
        {
            var verificationCode = ForgotPassword_TextBox_VerificationCode.Text;
            if(verificationCode != _mailVerificationCode)
            {
                MessageBox.Show("Invalid Email Verification Code! Try Again!");
                return;
            }

            var newPassword = ForgotPassword_Password_New.Password;
            var newPasswordConfirm = ForgotPassword_Password_NewConfirm.Password;

            if (String.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Password cannot be empty!");
                return;
            }

            if (newPassword != newPasswordConfirm)
            {
                MessageBox.Show("Passwords are not the same!");
                return;
            }

            if(BCrypt.Net.BCrypt.Verify(newPassword, _userToChange.Password))
            {
                MessageBox.Show("New password cannot be the same as old password!");
                return;
            }

            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                _userToChange.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                context.Users.Update(_userToChange);
                context.SaveChanges();
                MessageBox.Show("Password successfully changed!");
                BackToWindow();
            }


        }
        private void ForgotPassword_EmailButton_Click(object sender, RoutedEventArgs args)
        {
            var email = ForgotPassword_TextBox_Mail.Text;
            if (String.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email cannot be empty!");
                return;
            }

            Users foundUser = null;
            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                foundUser = context.Users.FirstOrDefault(u => u.Email == email);
            }
            if(foundUser != null)
            {
                _userToChange = foundUser;
                ForgotPassword_Content_BeforeEmailCode.Visibility = Visibility.Collapsed;
                ForgotPassword_Content_AfterEmailCode.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("User not found!");
            }
        }
    }
}
