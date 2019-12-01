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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public string P_Name { get; set; }
        public string P_CategoryID { get; set; }
        public double P_Price { get; set; }
        public double P_Weight { get; set; }
        public string P_Picture { get; set; }

        public EditProductWindow(Product item)
        {
            InitializeComponent();

            NameTextBox.Text = item.Name;

            CategoryComboBox.ItemsSource = MainWindow.db.Categories.ToList();
            CategoryComboBox.SelectedIndex = FindIndex(MainWindow.db.Categories.ToList(), item.CategoryID);

            PriceTextBox.Text = item.Price.ToString();
            WeightTextBox.Text = item.Weight.ToString();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var destinationPath = $"{baseDirectory}image\\{item.Picture}";
            Uri fileUri = new Uri(destinationPath);

            productImage.Source = new BitmapImage(fileUri);
            P_Picture = item.Picture;

            this.DataContext = this;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text) && !string.IsNullOrWhiteSpace(PriceTextBox.Text) && !string.IsNullOrWhiteSpace(WeightTextBox.Text))
            {
                P_Name = NameTextBox.Text;
                var temp = CategoryComboBox.SelectedItem as Category;
                P_CategoryID = temp.ID;
                P_Price = double.Parse(PriceTextBox.Text);
                P_Weight = double.Parse(WeightTextBox.Text);
                this.DialogResult = true;
            }
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Image files|*.jpg;*.jpeg;*.png";

            if (screen.ShowDialog() == true)
            {
                Uri fileUri = new Uri(screen.FileName);
                productImage.Source = new BitmapImage(fileUri);

                var imageSourceFileInfo = new FileInfo(screen.FileName);

                var uniqueName = $"{Guid.NewGuid()}.{imageSourceFileInfo.Extension}";
                P_Picture = uniqueName;

                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var destinationPath = $"{baseDirectory}image\\{uniqueName}";

                File.Copy(screen.FileName, destinationPath);
            }
        }

        private int FindIndex(List<Category> list, string id)
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
    }
}
