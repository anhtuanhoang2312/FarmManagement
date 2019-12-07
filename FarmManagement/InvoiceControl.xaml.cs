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

                //Invoice sheet
                var sheet = workbook.Worksheets[0];

                var col = "A";
                var row = 2;

                var cell = sheet.Cells[$"{col}{row}"];

                while (cell.Value != null)
                {
                    var customerid = sheet.Cells[$"B{row}"].StringValue;

                    bool exists = MainWindow.db.Customers.ToList().Any(cus => cus.ID == customerid);
                    if (exists == true)
                    {
                        var id = sheet.Cells[$"B{row}"].StringValue;
                        var date = sheet.Cells[$"C{row}"].DateTimeValue;
                        var total = sheet.Cells[$"D{row}"].DoubleValue;
                        var status = sheet.Cells[$"E{row}"].StringValue;

                        var newInvoice = new Invoice()
                        {
                            ID = IDGenerator.createID("I"),
                            CustomerID = customerid,
                            Date = date,
                            Total = total,
                            Status = status,
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
                            var newCustomer = new AddCustomerWindow();

                            if (newCustomer.ShowDialog() == true)
                            {
                                if (!string.IsNullOrWhiteSpace(newCustomer.C_Name) && !string.IsNullOrWhiteSpace(newCustomer.C_Telephone) && !string.IsNullOrWhiteSpace(newCustomer.C_Address))
                                {
                                    var customer = new Customer()
                                    {
                                        ID = customerid,
                                        Name = newCustomer.C_Name,
                                        Telephone = newCustomer.C_Telephone,
                                        Address = newCustomer.C_Address,
                                        isDeleted = false,
                                    };

                                    MainWindow.db.Customers.Add(customer);
                                    MainWindow.db.SaveChanges();
                                    CustomerControl.customer_notification.CustomerChange = true;

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
                                }
                            }

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

                //Invoice detail sheet
                var sheetd = workbook.Worksheets[1];

                var cold = "A";
                var rowd = 2;

                var celld = sheetd.Cells[$"{cold}{rowd}"];

                while (celld.Value != null)
                {
                    var invoiceid = sheetd.Cells[$"A{rowd}"].StringValue;
                    var productid = sheetd.Cells[$"B{rowd}"].StringValue;

                    bool existsinvoice = MainWindow.db.Invoices.ToList().Any(cus => cus.ID == invoiceid);
                    bool existsproduct = MainWindow.db.Products.ToList().Any(cus => cus.ID == productid);

                    if (existsinvoice != true)
                    {
                        string message = "The invoice with ID '" + invoiceid + "' does not exist. Do you want to add new invoice?";

                        if (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            var newInvoice = new AddInvoiceWindow(invoiceid);

                            if (newInvoice.ShowDialog() == true)
                            {
                                if (!string.IsNullOrWhiteSpace(newInvoice.I_CustomerID) && !string.IsNullOrWhiteSpace(newInvoice.I_Date.ToString()) && newInvoice.I_Total > 0 && !string.IsNullOrWhiteSpace(newInvoice.I_Status))
                                {
                                    var invoice = new Invoice()
                                    {
                                        ID = invoiceid,
                                        CustomerID = newInvoice.I_CustomerID,
                                        Date = newInvoice.I_Date,
                                        Total = newInvoice.I_Total,
                                        Status = newInvoice.I_Status,
                                        isDeleted = false,
                                    };

                                    MainWindow.db.Invoices.Add(invoice);
                                    MainWindow.db.SaveChanges();

                                    invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
                                }
                            }
                        }
                    }

                    if (existsproduct != true)
                    {
                        string message = "The product with ID '" + productid + "' in Invoice #'" + invoiceid + "' does not exist. Do you want to add new product?";

                        if (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            var newProduct = new AddProductWindow();

                            if (newProduct.ShowDialog() == true)
                            {
                                if (!string.IsNullOrWhiteSpace(newProduct.P_Name) && !string.IsNullOrWhiteSpace(newProduct.P_CategoryID) && newProduct.P_Price > 0 && newProduct.P_Weight > 0)
                                {
                                    var product = new Product()
                                    {
                                        ID = productid,
                                        Name = newProduct.P_Name,
                                        CategoryID = newProduct.P_CategoryID,
                                        Price = newProduct.P_Price,
                                        Weight = newProduct.P_Weight,
                                        isDeleted = false,
                                        Picture = newProduct.P_Picture,
                                    };

                                    MainWindow.db.Products.Add(product);
                                    MainWindow.db.SaveChanges();

                                    //notify changed here!!
                                }
                            }
                        }
                    }

                    var weight = sheetd.Cells[$"C{rowd}"].DoubleValue;
                    var unitprice = sheetd.Cells[$"D{rowd}"].DoubleValue;
                    var amount = sheetd.Cells[$"E{rowd}"].DoubleValue;

                    var newInvoiceDetail = new InvoiceDetail()
                    {
                        InvoiceID = invoiceid,
                        ProductID = productid,
                        Weight = weight,
                        UnitPrice = unitprice,
                        Amount = amount,
                        isDeleted = false,
                    };

                    MainWindow.db.InvoiceDetails.Add(newInvoiceDetail);
                    MainWindow.db.SaveChanges();

                    rowd++;
                    celld = sheetd.Cells[$"{cold}{rowd}"];
                }

                MessageBox.Show("Import successfully!", "Message");

                invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newInvoice = new AddInvoiceWindow(IDGenerator.createID("I"));

            if (newInvoice.ShowDialog() == true)
            {
                invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
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
            Invoice selectedItem = (Invoice)invoiceDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var deletedetails = from detail in MainWindow.db.InvoiceDetails
                                    where detail.InvoiceID == selectedItem.ID
                                    select detail;
                foreach (var detail in deletedetails)
                {
                    MainWindow.db.InvoiceDetails.Remove(detail);
                }
                MainWindow.db.SaveChanges();

                var delete = (from invoice in MainWindow.db.Invoices where invoice.ID == selectedItem.ID select invoice).Single();
                MainWindow.db.Invoices.Remove(delete);
                MainWindow.db.SaveChanges();

                invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from invoice in MainWindow.db.Invoices.ToList()
                       where invoice.ID.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved())
                       select invoice;
            invoiceDataGrid.ItemsSource = data;
        }

        private void InvoiceNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            invoiceDataGrid.ItemsSource = MainWindow.db.Invoices.ToList();
        }

        private void invoiceDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                Invoice selectedItem = (Invoice)row.Item;

                var newProduct = new InvoiceDetailWindow(selectedItem);

                if (newProduct.ShowDialog() == true)
                {
                    //something...
                }
            }
        }
    }
}
