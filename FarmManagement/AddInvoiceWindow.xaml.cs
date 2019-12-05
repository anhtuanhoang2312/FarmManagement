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

namespace FarmManagement
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class AddInvoiceWindow : Window
    {
        public string I_ID { get; set; }
        public string I_CustomerID { get; set; }
        public DateTime I_Date { get; set; }
        public double I_Total { get; set; }
        public string I_Status { get; set; }

        public AddInvoiceWindow(string id)
        {
            InitializeComponent();

            InvoiceTextBox.Text = id;

            CustomerComboBox.ItemsSource = MainWindow.db.Customers.ToList();

            DateDatePicker.Text = DateTime.Now.ToString("MM/dd/yyyy");

            var statusList = new List<string>() { "New", "Delivered", "Cancelled" };
            StatusComboBox.ItemsSource = statusList;

            ProductComboBox.ItemsSource = MainWindow.db.Products.ToList();

            LoadDetails(id);
            this.DataContext = this;
        }

        private void LoadDetails(string id)
        {
            var data = from invoice in MainWindow.db.InvoiceDetails.ToList()
                       where invoice.InvoiceID.Contains(id)
                       select invoice;
            invoiceDataGrid.ItemsSource = data;
        }
    }
}
