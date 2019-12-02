using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public double P_Price { get; set; }
        public double P_Weight { get; set; }
        public string P_Picture { get; set; }

        public AddProductWindow()
        {
            InitializeComponent();
            CategoryComboBox.ItemsSource = MainWindow.db.Categories.ToList();
            this.DataContext = this;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && PriceTextBox.Value > 0 && WeightTextBox.Value > 0 && CategoryComboBox.SelectedItem != null)
            {
                P_Name = NameTextBox.Text;
                var cat = CategoryComboBox.SelectedItem as Category;
                P_CategoryID = cat.ID;
                P_Price = PriceTextBox.Value;
                P_Weight = WeightTextBox.Value;
                this.DialogResult = true;
            }
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Image files|*.jpg;*.jpeg;*.png";

            if (screen.ShowDialog() == true)
            {
                Uri fileUri = new Uri(screen.FileName);
                productImage.Source = new BitmapImage(fileUri);
                BrowseBtn.Content = "Change";

                var imageSourceFileInfo = new FileInfo(screen.FileName);

                var uniqueName = $"{Guid.NewGuid()}.{imageSourceFileInfo.Extension}";
                P_Picture = uniqueName;

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var destinationPath = $"{baseDirectory}image\\{uniqueName}";

                File.Copy(screen.FileName, destinationPath);
            }
        }
    }
}
