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
    /// Interaction logic for InvoiceDetailWindow.xaml
    /// </summary>
    public partial class InvoiceDetailWindow : Window
    {
        //public string I_ID { get; set; }
        //public string I_CustomerID { get; set; }
        //public DateTime I_Date { get; set; }
        //public double I_Total { get; set; }
        //public string I_Status { get; set; }

        private BindingList<InvoiceDetail> tempList = new BindingList<InvoiceDetail>();

        public static Notify invoiceDetail_notification = new Notify();

        public InvoiceDetailWindow(Invoice item)
        {
            InitializeComponent();

            InvoiceTextBox.Text = item.ID;

            CustomerComboBox.ItemsSource = MainWindow.db.Customers.ToList();
            CustomerComboBox.SelectedIndex = FindIndex(MainWindow.db.Customers.ToList(), item.CustomerID);

            DateDatePicker.Text = item.Date.ToString();

            var statusList = new BindingList<string>() { "New", "Delivered", "Cancelled" };
            StatusComboBox.ItemsSource = statusList;

            StatusComboBox.SelectedIndex = FindIndex(statusList, item.Status); 
            
            LoadDetails(item.ID);

            invoiceDetail_notification.PropertyChanged += InvoiceDetailNotification_PropertyChanged;

            this.DataContext = this;
        }

        private void LoadDetails(string id)
        {
            var data = from invoice in MainWindow.db.InvoiceDetails.ToList()
                       where invoice.InvoiceID.Contains(id)
                       select invoice;
            invoiceDataGrid.ItemsSource = data;
        }

        private int FindIndex(List<Customer> list, string id)
        {
            int i = 0;
            foreach (var item in list)
            {
                if (item.ID == id)
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        private int FindIndex(BindingList<string> list, string name)
        {
            int i = 0;
            foreach (var item in list)
            {
                if (item == name)
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        private void InvoiceDetailNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            invoiceDataGrid.ItemsSource = tempList;
        }
    }
}
