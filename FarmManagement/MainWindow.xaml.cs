using Aspose.Cells;
using FarmManagement.Class;
using HandyControl.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static FarmEntities db = new FarmEntities();
        public static ProductControl productpg = new ProductControl();
        public static CategoryControl categorypg = new CategoryControl();
        public static CustomerControl customerpg = new CustomerControl();
        public static InvoiceControl invoicepg = new InvoiceControl();

        public MainWindow()
        {         
            InitializeComponent();
        }

        private void sideMenu_Selected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as SideMenuItem;
            if (item != null)
            {
                if (item.Name == "Products")
                {
                    Control.Show(MainContent, productpg);
                }
                else if (item.Name == "Categories")
                {
                    Control.Show(MainContent, categorypg);
                }
                else if (item.Name == "Customers")
                {
                    Control.Show(MainContent, customerpg);
                }
                else if (item.Name == "Invoices")
                {
                    Control.Show(MainContent, invoicepg);
                }
            }
        }
    }
}

