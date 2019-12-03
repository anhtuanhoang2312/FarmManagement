using Aspose.Cells;
using FarmManagement.Class;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for InvoiceControl.xaml
    /// </summary>
    public partial class InvoiceControl : UserControl
    {
        public static Notify invoice_notification = new Notify();

        public InvoiceControl()
        {
            InitializeComponent();
            invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
            invoice_notification.PropertyChanged += InvoiceNotification_PropertyChanged;
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
                    var customerid = sheet.Cells[$"B{row}"].StringValue;

                    bool has = MainWindow.db.Customers.ToList().Any(cus => cus.ID == customerid);
                    if (has == true)
                    {
                        var id = sheet.Cells[$"B{row}"].StringValue;
                        var date = sheet.Cells[$"C{row}"].DateTimeValue;
                        var total = sheet.Cells[$"D{row}"].DoubleValue;

                        var newInvoice = new Invoice()
                        {
                            ID = IDGenerator.createID("I"),
                            CustomerID = customerid,
                            Date = date,
                            Total = total,
                            isDeleted = false,
                        };

                        MainWindow.db.Invoices.Add(newInvoice);
                        MainWindow.db.SaveChanges();

                        row++;
                        cell = sheet.Cells[$"{col}{row}"];
                    }
                    else
                    {

                        string message = "The customer with ID '" + customerid + "' does not exist. Do you want to add new customer?";

                        if (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            //var customer = new Customer()
                            //{
                            //    ID = IDGenerator.createID("C"),
                            //    Name = customerid,
                            //    isDeleted = false,
                            //};

                            //MainWindow.db.Customers.Add(customer);
                            //MainWindow.db.SaveChanges();
                            //CategoryControl.category_notification.CategoryChange = true;

                            var id = sheet.Cells[$"B{row}"].StringValue;
                            var date = sheet.Cells[$"C{row}"].DateTimeValue;
                            var total = sheet.Cells[$"D{row}"].DoubleValue;

                            var newInvoice = new Invoice()
                            {
                                ID = IDGenerator.createID("I"),
                                CustomerID = customerid,
                                Date = date,
                                Total = total,
                                isDeleted = false,
                            };

                            MainWindow.db.Invoices.Add(newInvoice);
                            MainWindow.db.SaveChanges();

                            row++;
                            cell = sheet.Cells[$"{col}{row}"];
                        }
                        else
                        {
                            row++;
                            cell = sheet.Cells[$"{col}{row}"];
                        }
                    }
                }

                MessageBox.Show("Import successfully!", "Message");

                invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new AddProductWindow();

            if (newProduct.ShowDialog() == true)
            {
                var product = new Product()
                {
                    ID = IDGenerator.createID("P"),
                    Name = newProduct.P_Name,
                    CategoryID = newProduct.P_CategoryID,
                    Price = newProduct.P_Price,
                    Weight = newProduct.P_Weight,
                    isDeleted = false,
                    Picture = newProduct.P_Picture,
                };

                MainWindow.db.Products.Add(product);
                MainWindow.db.SaveChanges();

                invoiceDataGrid.ItemsSource = MainWindow.db.Products.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)invoiceDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                //var newProduct = new EditProductWindow(selectedItem.CategoryID, selectedItem.Picture);

                //newProduct.P_Name = selectedItem.Name;
                //newProduct.P_Price = selectedItem.Price ?? 0; // 0 se la gia tri mac dinh neu Price mang gia tri NULL
                //newProduct.P_Weight = selectedItem.Weight ?? 0;

                var newProduct = new EditProductWindow(selectedItem);

                if (newProduct.ShowDialog() == true)
                {
                    var update = (from product in MainWindow.db.Products where product.ID == selectedItem.ID select product).Single();
                    update.Name = newProduct.P_Name;
                    update.CategoryID = newProduct.P_CategoryID;
                    update.Price = newProduct.P_Price;
                    update.Weight = newProduct.P_Weight;
                    update.Picture = newProduct.P_Picture;
                    MainWindow.db.SaveChanges();

                    invoiceDataGrid.ItemsSource = MainWindow.db.Products.ToList();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)invoiceDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var delete = (from product in MainWindow.db.Products where product.ID == selectedItem.ID select product).Single();
                MainWindow.db.Products.Remove(delete);
                MainWindow.db.SaveChanges();

                invoiceDataGrid.ItemsSource = MainWindow.db.Products.ToList();
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from product in MainWindow.db.Products.ToList()
                       where product.Name.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved())
                       select product;
            invoiceDataGrid.ItemsSource = data;
        }

        private void InvoiceNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            invoiceDataGrid.ItemsSource = MainWindow.db.Products.ToList();
        }

        private void invoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                Invoice selectedItem = (Invoice)row.Item;

                //var newProduct = new EditProductWindow(selectedItem);

                //if (newProduct.ShowDialog() == true)
                //{
                //    var update = (from product in MainWindow.db.Products where product.ID == selectedItem.ID select product).Single();
                //    update.Name = newProduct.P_Name;
                //    update.CategoryID = newProduct.P_CategoryID;
                //    update.Price = newProduct.P_Price;
                //    update.Weight = newProduct.P_Weight;
                //    update.Picture = newProduct.P_Picture;
                //    MainWindow.db.SaveChanges();

                //    productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
                //}
            }
        }
    }
}
