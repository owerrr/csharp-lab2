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
    /// Interaction logic for ManageVehicleWindow.xaml
    /// </summary>
    public partial class ManageVehicleWindow : Window
    {
        private EmployeeWorkOnVehicles _vehicle { get; set; }
        private MainWindow _mainWindow { get; set; }
        public ManageVehicleWindow(EmployeeWorkOnVehicles vehicle, MainWindow mainWindow)
        {
            InitializeComponent();

            _vehicle = vehicle;
            _mainWindow = mainWindow;

            FixUI();
        }

        private void FixUI()
        {
            if (_vehicle != null)
            {
                ClientVehicles clientVehicleData = LoadVehicleData();
                
                if(clientVehicleData != null)
                {
                    Label_ManageVehicleHeader.Content = $"Managing Vehicle: {_vehicle.ClientVehicle_Id}";
                    Label_ManageVehicleSubHeader.Content = $"{clientVehicleData.Car_Model} from {clientVehicleData.Car_Year}, License plate: {clientVehicleData.Car_RegNo}";
                    LoadVehicleChanges();
                }
            }
        }

        private ClientVehicles LoadVehicleData()
        {
            ClientVehicles clientVehicleData = null;
            using (WorkshopDbContext context = new WorkshopDbContext())
            {
                clientVehicleData = context.Client_Vehicles.FirstOrDefault(x => x.Id == _vehicle.ClientVehicle_Id);
            }
            return clientVehicleData;
        }

        public void RefreshVehicleChangesData()
        {
            LoadVehicleChanges();
        }

        private void LoadVehicleChanges()
        {
            var grid = Grid_ManageVehicle;
            Grid_ManageVehicle.Children.Clear();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());

            var labelHeaderName = new Label { Content = "Name", FontWeight = FontWeights.Bold, MinWidth = 100, VerticalAlignment = VerticalAlignment.Center };
            var labelHeaderPrice = new Label { Content = "Price", FontWeight = FontWeights.Bold, MinWidth = 100, VerticalAlignment = VerticalAlignment.Center };
            var labelHeaderIsDone = new Label { Content = "Is Done", FontWeight = FontWeights.Bold, MinWidth = 100, VerticalAlignment = VerticalAlignment.Center };

            Grid.SetColumn(labelHeaderName, 0);
            Grid.SetColumn(labelHeaderPrice, 1);
            Grid.SetColumn(labelHeaderIsDone, 2);

            Grid.SetRow(labelHeaderName, 0);
            Grid.SetRow(labelHeaderPrice, 0);
            Grid.SetRow(labelHeaderIsDone, 0);

            grid.Children.Add(labelHeaderName);
            grid.Children.Add(labelHeaderPrice);
            grid.Children.Add(labelHeaderIsDone);

            int rowIdx = 1;

            string[] vehManagementData = _vehicle.WorkOn.Split(";", StringSplitOptions.RemoveEmptyEntries).ToArray();
            foreach(var vehicle in vehManagementData)
            {
                string[] scrapedData = vehicle.Split(":", StringSplitOptions.RemoveEmptyEntries).ToArray();
                grid.RowDefinitions.Add(new RowDefinition());
                var labelName = new Label { Content = $"{scrapedData[0]}", VerticalAlignment = VerticalAlignment.Center };
                var labelPrice = new Label { Content = $"{scrapedData[1]}", VerticalAlignment = VerticalAlignment.Center };
                var labelIsDone = new Label { Content = "No", VerticalAlignment = VerticalAlignment.Center };
                var button = new Button { Width = 150, Content = "Mark as Done", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"ManageVehicle_ActionButton_Click_{rowIdx}" };
                var buttonEdit = new Button { Width = 100, Content = "Edit", Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"ManageVehicle_ActionButtonEdit_Click_{rowIdx}" };
                var buttonDelete = new Button { Width = 100, Content = "Delete", Style = (Style)Application.Current.FindResource("Client_Vehicle_DeleteBtn"), VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 5, 5, 5), Name = $"ManageVehicle_ActionButtonDelete_Click_{rowIdx}" };
                button.Click += ManageVehicle_ActionButton_MarkAsDone_Click;
                buttonEdit.Click += ManageVehicle_ActionButton_EditItem_Click;
                buttonDelete.Click += ManageVehicle_ActionButton_DeleteItem_Click;
                if (scrapedData[2] == "1")
                {
                    labelIsDone.Content = "Yes";
                    button.Content = "Mark as Not Done";
                    button.Style = (Style)Application.Current.FindResource("Client_Vehicle_DeleteBtn");
                    button.Click -= ManageVehicle_ActionButton_MarkAsDone_Click;
                    button.Click += ManageVehicle_ActionButton_MarkAsNotDone_Click;
                }
                

                Grid.SetColumn(labelName, 0);
                Grid.SetColumn(labelPrice, 1);
                Grid.SetColumn(labelIsDone, 2);
                Grid.SetColumn(button, 3);
                Grid.SetColumn(buttonEdit, 4);
                Grid.SetColumn(buttonDelete, 5);

                Grid.SetRow(labelName, rowIdx);
                Grid.SetRow(labelPrice, rowIdx);
                Grid.SetRow(labelIsDone, rowIdx);
                Grid.SetRow(button, rowIdx);
                Grid.SetRow(buttonEdit, rowIdx);
                Grid.SetRow(buttonDelete, rowIdx);

                rowIdx++;

                grid.Children.Add(labelName);
                grid.Children.Add(labelPrice);
                grid.Children.Add(labelIsDone);
                grid.Children.Add(button);
                grid.Children.Add(buttonEdit);
                grid.Children.Add(buttonDelete);
            }
        }

        private void ManageVehicle_ActionButton_MarkAsDone_Click(object sender, RoutedEventArgs args)
        {
            int rowIdx = Convert.ToInt32((sender as Button).Name.Split("_").Last());
            var grid = Grid_ManageVehicle;
            foreach (UIElement child in grid.Children)
            {
                if (child is Label label &&
                    Grid.GetRow(label) == rowIdx &&
                    Grid.GetColumn(label) == 2)
                {
                    label.Content = "Yes";
                }
                if (child is Button btn &&
                    Grid.GetRow(btn) == rowIdx &&
                    Grid.GetColumn(btn) == 3)
                {
                    btn.Content = "Mark as Not Done";
                    btn.Style = (Style)Application.Current.FindResource("Client_Vehicle_DeleteBtn");
                    btn.Click -= ManageVehicle_ActionButton_MarkAsDone_Click;
                    btn.Click += ManageVehicle_ActionButton_MarkAsNotDone_Click;
                    break;
                }
            }
        }

        private void ManageVehicle_ActionButton_MarkAsNotDone_Click(object sender, RoutedEventArgs args)
        {
            int rowIdx = Convert.ToInt32((sender as Button).Name.Split("_").Last());
            var grid = Grid_ManageVehicle;
            foreach (UIElement child in grid.Children)
            {
                if (child is Label label &&
                    Grid.GetRow(label) == rowIdx &&
                    Grid.GetColumn(label) == 2)
                {
                    label.Content = "No";
                }
                if (child is Button btn &&
                    Grid.GetRow(btn) == rowIdx &&
                    Grid.GetColumn(btn) == 3)
                {
                    btn.Content = "Mark as Done";
                    btn.Style = (Style)Application.Current.FindResource("Client_Vehicle_ModifyBtn");
                    btn.Click += ManageVehicle_ActionButton_MarkAsDone_Click;
                    btn.Click -= ManageVehicle_ActionButton_MarkAsNotDone_Click;
                    break;
                }
            }
        }
        private void ManageVehicle_ActionButton_EditItem_Click(object sender, RoutedEventArgs args)
        {
            int rowIdx = Convert.ToInt32((sender as Button).Name.Split("_").Last());
            ManageVehicleWindow_AddNewItem mvw_EditItem = new(this, _vehicle, rowIdx);
            mvw_EditItem.ShowDialog();
        }
        private void ManageVehicle_ActionButton_DeleteItem_Click(object sender, RoutedEventArgs args)
        {
            int rowIdx = Convert.ToInt32((sender as Button).Name.Split("_").Last());
            var grid = Grid_ManageVehicle;
            var childrenToRemove = grid.Children
                .OfType<UIElement>()
                .Where(x => Grid.GetRow(x) == rowIdx)
                .ToList();

            foreach (var child in childrenToRemove)
            {
                grid.Children.Remove(child);
            }

            foreach (UIElement element in grid.Children)
            {
                int currentRow = Grid.GetRow(element);
                if (currentRow > rowIdx)
                {
                    Grid.SetRow(element, currentRow - 1);
                }
            }
        }

        private void ManageVehicle_AddNewItemButton_Click(object sender, RoutedEventArgs args)
        {
            ManageVehicleWindow_AddNewItem mvw_EditItem = new(this, _vehicle);
            mvw_EditItem.ShowDialog();
        }

        private void ManageVehicle_MarkVehicleAsDoneButton_Click(object sender, RoutedEventArgs args)
        {
            string[] scrapedWorkData = _vehicle.WorkOn.Split(";", StringSplitOptions.RemoveEmptyEntries);
            foreach(string data in scrapedWorkData)
            {
                string checkIsDone = data.Split(":", StringSplitOptions.RemoveEmptyEntries)[2];
                if(checkIsDone == "0")
                {
                    MessageBox.Show("All work must be done!");
                    return;
                }
            }

            
            using(WorkshopDbContext context = new WorkshopDbContext())
            {
                ClientVehicles clientVehicle = context.Client_Vehicles.FirstOrDefault(x => x.Id == _vehicle.ClientVehicle_Id);
                if(clientVehicle != null)
                {
                    _vehicle.IsDone = true;
                    clientVehicle.IsMaintenanced = false;
                    context.EmployeeWorkOnVehicles.Update(_vehicle);
                    context.Client_Vehicles.Update(clientVehicle);
                    context.SaveChanges();
                    MessageBox.Show("marked as done!");
                }
            }

            this.Close();
            _mainWindow.Worker_RefreshClientVehiclesManageList();
        }

        private void ManageVehicle_SaveVehicleButton_Click(object sender, RoutedEventArgs args)
        {
            var grid = Grid_ManageVehicle;
            string WorkOnFixed = "";
            for(int i = 1; i < grid.Children.Count; i++)
            {
                string name = "", price = "", isDone = "";
                foreach(UIElement element in grid.Children)
                {
                    if(Grid.GetRow(element) == i)
                    {
                        if(element is Label label)
                        {
                            switch (Grid.GetColumn(element))
                            {
                                case 0:
                                    name = label.Content.ToString();
                                    break;
                                case 1:
                                    price = label.Content.ToString();
                                    break;
                                case 2:
                                    isDone = (label.Content.ToString() == "Yes" ? "1" : "0");
                                    break;
                            }
                        }
                    }
                }
                if(!String.IsNullOrEmpty(name))
                    WorkOnFixed += $"{name}:{price}:{isDone};";
            }
            //MessageBox.Show(WorkOnFixed);
            _vehicle.WorkOn = WorkOnFixed;
            using(WorkshopDbContext context = new WorkshopDbContext())
            {
                context.EmployeeWorkOnVehicles.Update(_vehicle);
                context.SaveChanges();
            }

            MessageBox.Show("Successfully saved changes!");
        }
    }
}
