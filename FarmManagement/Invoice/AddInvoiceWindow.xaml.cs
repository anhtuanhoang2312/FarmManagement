using FarmManagement.Class;
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
using System.Windows.Shapes;

namespace FarmManagement
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class AddInvoiceWindow : Window
    {
        public static Notify invoiceDetail_notification = new Notify();

        private BindingList<InvoiceDetail> tempList = new BindingList<InvoiceDetail>();

        public AddInvoiceWindow(string id)
        {
            InitializeComponent();

            InvoiceTextBox.Text = id;

            CustomerComboBox.ItemsSource = MainWindow.db.Customers.ToList();

            DateDatePicker.Text = DateTime.Now.ToString("MM/dd/yyyy");

            var statusList = new BindingList<string>() { "New", "Delivered", "Cancelled" };
            StatusComboBox.ItemsSource = statusList;

            ProductComboBox.ItemsSource = MainWindow.db.Products.ToList();

            invoiceDetail_notification.PropertyChanged += InvoiceDetailNotification_PropertyChanged;

            this.DataContext = this;
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var product = ProductComboBox.SelectedItem as Product;

            var newInvoiceDetail = new InvoiceDetail()
            {
                InvoiceID = InvoiceTextBox.Text,
                ProductID = product.Name,
                Weight = 1,
                UnitPrice = product.Price,
                Amount = product.Price,
                isDeleted = false,
            };

            tempList.Add(newInvoiceDetail);
            invoiceDataGrid.ItemsSource = tempList;

            TotalTextBox.Text = "$" + TotalCalculator().ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = CustomerComboBox.SelectedItem as Customer;

            if (CustomerComboBox.SelectedItem != null && DateDatePicker.Text != null && StatusComboBox.SelectedItem != null)
            {
                var newInvoice = new Invoice()
                {
                    ID = InvoiceTextBox.Text,
                    CustomerID = customer.ID,
                    Date = DateTime.Now,
                    Total = TotalCalculator(),
                    Status = StatusComboBox.SelectedItem.ToString(),
                    isDeleted = false,
                };

                MainWindow.db.Invoices.Add(newInvoice);
                MainWindow.db.SaveChanges();

                ProductNametoIDConverter(tempList);

                foreach (var item in tempList)
                {
                    MainWindow.db.InvoiceDetails.Add(item);
                    MainWindow.db.SaveChanges();
                }

                this.DialogResult = true;
            }               
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            InvoiceDetail selectedItem = (InvoiceDetail)invoiceDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                foreach (var detail in tempList)
                {
                    if (detail.InvoiceID == selectedItem.InvoiceID)
                    {
                        tempList.Remove(detail);
                        break;
                    }
                }

                invoiceDataGrid.ItemsSource = tempList;
                TotalTextBox.Text = "$" + TotalCalculator().ToString();
            }
        }

        private double TotalCalculator()
        {
            double sum = 0;

            foreach (var detail in tempList)
            {
                sum = sum + (detail.UnitPrice ?? 0) * (detail.Weight ?? 0);
            }

            return sum;
        }

        private void ProductNametoIDConverter(BindingList<InvoiceDetail> list)
        {
            foreach(var item in list)
            {
                item.ProductID = (from product in MainWindow.db.Products where product.Name == item.ProductID select product).Single().ID;
            }
        }

        private void InvoiceDetailNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            invoiceDataGrid.ItemsSource = tempList;
        }

        private void NumericUpDown_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            InvoiceDetail selectedItem = (InvoiceDetail)invoiceDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                foreach (var detail in tempList)
                {
                    if (detail.InvoiceID == selectedItem.InvoiceID)
                    {
                        detail.Amount = (detail.UnitPrice ?? 0) * (detail.Weight ?? 0);
                    }
                }
            }

            invoiceDetail_notification.InvoiceDetailChange = true;
            TotalTextBox.Text = "$" + TotalCalculator().ToString();
        }
    }
}
