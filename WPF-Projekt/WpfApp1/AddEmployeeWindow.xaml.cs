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
    /// Interaction logic for AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        private Users _user { get; set; }
        private Users _empUser { get; set; }
        private MainWindow _mw { get; set; }
        private Employees _employee { get; set; }
        public AddEmployeeWindow(Users user, MainWindow mw, Employees employee = null, Users empUser = null)
        {
            InitializeComponent();
            _user = user;
            _empUser = empUser;
            _mw = mw;
            if (employee != null)
                _employee = employee;

            FixUI();
        }
        
        private void ClearTextBoxes()
        {
            Employee_TextBox_Username.Clear();
            Employee_TextBox_Password.Clear();
            Employee_TextBox_Firstname.Clear();
            Employee_TextBox_Lastname.Clear();
            Employee_TextBox_Phonenumber.Clear();
            Employee_TextBox_Email.Clear();
        }

        private void FixUI()
        {
            List<EmployeeTitles> EmployeeTitles = new();
            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                int userModifyEmployeePermissions = context.Employee_Titles.FirstOrDefault(x => x.Id == _user.Employee_Title_Id).Modify_Employees_Permissions;
                EmployeeTitles = context.Employee_Titles.Where(x => x.Modify_Employees_Permissions <= userModifyEmployeePermissions).ToList();
            }

            Employee_ComboBox_Titles.ItemsSource = EmployeeTitles;
            Employee_ComboBox_Titles.DisplayMemberPath = "Name";
            Employee_ComboBox_Titles.SelectedValuePath = "Id";

            if (_employee != null)
            {
                EmployeeWindow_Header.Content = "Edit Employee";

                Employee_TextBox_Username.IsEnabled = false;
                Employee_TextBox_Password.Visibility = Visibility.Collapsed;
                Employee_Label_Password.Visibility = Visibility.Collapsed;
                Employee_Button_ChangePassword.Visibility = Visibility.Visible;

                Employee_TextBox_Username.Text = _user.Name;
                Employee_TextBox_Firstname.Text = _employee.Firstname;
                Employee_TextBox_Lastname.Text = _employee.Lastname;
                Employee_TextBox_Phonenumber.Text = _employee.Phonenumber;
                Employee_TextBox_Email.Text = _employee.Email;

                Employee_ComboBox_Titles.SelectedValue = _user.Employee_Title_Id;
            }
            else
            {
                EmployeeWindow_Header.Content = "Add new Employee";

                Employee_TextBox_Username.IsEnabled = true;
                Employee_TextBox_Password.Visibility = Visibility.Visible;
                Employee_Label_Password.Visibility = Visibility.Visible;
                Employee_Button_ChangePassword.Visibility = Visibility.Collapsed;


            }
        }

        private void Employee_Button_ChangePassword_Click(object sender, RoutedEventArgs args)
        {
            MessageBox.Show("test");
        }

        private void Employee_Button_Action_Click(object sender, RoutedEventArgs args)
        {
            var firstname = Employee_TextBox_Firstname.Text;
            var lastname = Employee_TextBox_Lastname.Text;
            var phonenumber = Employee_TextBox_Phonenumber.Text;
            var email = Employee_TextBox_Email.Text;

            if (String.IsNullOrEmpty(firstname)
                || String.IsNullOrEmpty(lastname)
                || String.IsNullOrEmpty(phonenumber)
                || String.IsNullOrEmpty(email)
                )
            {
                MessageBox.Show("You must fill all the Employee data!");
                return;
            }

            if(!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email is invalid! example email: test@test.com");
                return;
            }

            if (_employee != null)
            {
                using (WorkshopDbContext context = new WorkshopDbContext())
                {
                    _employee.Firstname = firstname;
                    _employee.Lastname = lastname;
                    _employee.Phonenumber = phonenumber;
                    _employee.Email = email;

                    context.Employees.Update(_employee);
                    context.SaveChanges();
                    MessageBox.Show("Employee successfully changed!");
                }
            }
            else
            {
                var username = Employee_TextBox_Username.Text;
                var password_plain = Employee_TextBox_Password.Password;

                if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password_plain))
                {
                    MessageBox.Show("You must fill all the Employee Login");
                    return;
                }

                var password = BCrypt.Net.BCrypt.HashPassword(password_plain);

                using (WorkshopDbContext context = new WorkshopDbContext())
                {

                    Employees newEmp = new Employees { Firstname = firstname, Lastname = lastname, Phonenumber = phonenumber, Email = email };
                    context.Employees.Add(newEmp);
                    context.SaveChanges();
                    Users newUser = new Users { Name = username, Password = password, Employee_Title_Id = 1, Employee_Id = newEmp.Id };
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    MessageBox.Show($"Successfully added new Employee!");
                }
            }

            _mw.RefreshEmployees();
            Employee_Button_Back_Click(sender, args);
        }

        private void Employee_Button_Back_Click(object sender, RoutedEventArgs args)
        {
            _user = null;
            _mw = null;
            _employee = null;
            ClearTextBoxes();
            this.Close();
        }
    }
}
