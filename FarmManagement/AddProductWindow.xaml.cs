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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public string P_Name { get; set; }
        public string P_CategoryID { get; set; }
        public float P_Price { get; set; }
        public float  P_Weight { get; set; }

        public AddProductWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
