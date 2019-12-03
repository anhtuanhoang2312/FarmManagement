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
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public string C_Name { get; set; }
        public string C_Telephone { get; set; }
        public string C_Address { get; set; }

        public AddCustomerWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && !string.IsNullOrWhiteSpace(AddressTextBox.Text) && !string.IsNullOrWhiteSpace(TelephoneTextBox.Text))
            {
                C_Name = NameTextBox.Text;
                C_Telephone = TelephoneTextBox.Text;
                C_Address = AddressTextBox.Text;
                this.DialogResult = true;
            }
        }
    }
}
