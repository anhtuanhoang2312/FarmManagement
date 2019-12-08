using Aspose.Cells;
using FarmManagement.Class;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FarmManagement
{
    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        public static Notify customer_notification = new Notify();

        public CustomerControl()
        {
            InitializeComponent();
            customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
            customer_notification.PropertyChanged += CustomerNotification_PropertyChanged;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Excel files|*.xls;*.xlsx";

            if (screen.ShowDialog() == true)
            {
                var workbook = new Workbook(screen.FileName);

                var sheet = workbook.Worksheets[0];

                var col = "A";
                var row = 2;

                var cell = sheet.Cells[$"{col}{row}"];

                while (cell.Value != null)
                {
                    var name = sheet.Cells[$"B{row}"].StringValue;
                    var telephone = sheet.Cells[$"C{row}"].StringValue;
                    var address = sheet.Cells[$"D{row}"].StringValue;

                    var newCustomer = new Customer()
                    {
                        ID = IDGenerator.createID("C"),
                        Name = name,
                        Telephone = telephone,
                        Address = address,
                        isDeleted = false,
                    };

                    MainWindow.db.Customers.Add(newCustomer);
                    MainWindow.db.SaveChanges();

                    row++;
                    cell = sheet.Cells[$"{col}{row}"];
                }

                MessageBox.Show("Import successfully!", "Message");

                customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new AddCustomerWindow();

            if (newCustomer.ShowDialog() == true)
            {
                var customer = new Customer()
                {
                    ID = IDGenerator.createID("C"),
                    Name = newCustomer.C_Name,
                    Telephone = newCustomer.C_Telephone,
                    Address = newCustomer.C_Address,
                    isDeleted = false,
                };

                MainWindow.db.Customers.Add(customer);
                MainWindow.db.SaveChanges();

                customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedItem = (Customer)customerDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var newCustomer = new EditCustomerWindow(selectedItem);

                if (newCustomer.ShowDialog() == true)
                {
                    var update = (from customer in MainWindow.db.Customers where customer.ID == selectedItem.ID select customer).Single();
                    update.Name = newCustomer.C_Name;
                    update.Telephone = newCustomer.C_Telephone;
                    update.Address = newCustomer.C_Address;
                    MainWindow.db.SaveChanges();

                    customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedItem = (Customer)customerDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var delete = (from customer in MainWindow.db.Customers where customer.ID == selectedItem.ID select customer).Single();
                MainWindow.db.Customers.Remove(delete);
                MainWindow.db.SaveChanges();

                customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from customer in MainWindow.db.Customers.ToList()
                       where customer.Name.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved())
                       select customer;
            customerDataGrid.ItemsSource = data;
        }

        private void CustomerNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
        }

        private void customerDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            
            if (row != null)
            {
                Customer selectedItem = (Customer)row.Item;

                var newCustomer = new EditCustomerWindow(selectedItem);

                if (newCustomer.ShowDialog() == true)
                {
                    var update = (from customer in MainWindow.db.Customers where customer.ID == selectedItem.ID select customer).Single();
                    update.Name = newCustomer.C_Name;
                    update.Telephone = newCustomer.C_Telephone;
                    update.Address = newCustomer.C_Address;
                    MainWindow.db.SaveChanges();

                    customerDataGrid.ItemsSource = MainWindow.db.Customers.ToList();
                }         
            }
        }
    }
}
