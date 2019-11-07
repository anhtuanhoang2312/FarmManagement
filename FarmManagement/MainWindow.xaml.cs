using Aspose.Cells;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.MaterialControls;

namespace FarmManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Product> temp = new List<Product>();

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            ThemeEffectsHelper.IsAcrylicEnabled = true;
            FluentPalette.LoadPreset(FluentPalette.ColorVariation.Light);
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Excel files | *.xls;*.xlsx";

            if (screen.ShowDialog() == true)
            {
                var db = new FarmEntities();

                var workbook = new Workbook(screen.FileName);

                var sheet = workbook.Worksheets[0];

                var col = "A";
                var row = 2;

                var cell = sheet.Cells[$"{col}{row}"];

                while (cell.Value != null)
                {
                    var id = sheet.Cells[$"A{row}"].StringValue;
                    var name = sheet.Cells[$"B{row}"].StringValue;
                    var price = sheet.Cells[$"C{row}"].FloatValue;
                    var weight = sheet.Cells[$"D{row}"].FloatValue;
                    //var imageName = sheet.Cells[$"E{row}"].StringValue;

                    //var imageSourceInfo = new FileInfo(screen.FileName);
                    //var imageSourceFullPath = $"{imageSourceInfo.DirectoryName}\\images\\{imageName}";
                    //var imageSourceFileInfo = new FileInfo(imageSourceFullPath);

                    //var uniqueName = $"{Guid.NewGuid()}.{imageSourceFileInfo.Extension}";

                    //var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    //var destinationPath = $"{baseDirectory}image\\{uniqueName}";

                    //File.Copy(imageSourceFullPath, destinationPath);

                    var newProduct = new Product()
                    {
                        ID = id,
                        Name = name,
                        Price = price,
                        Weight = weight,
                        //Picture = uniqueName,
                    };

                    db.Products.Add(newProduct);
                    db.SaveChanges();

                    row++;
                    cell = sheet.Cells[$"{col}{row}"];
                }

                MessageBox.Show("Import successfully!", "Message");

                temp = db.Products.ToList();

                productList.ItemsSource = db.Products.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new AddProductWindow();

            if (newProduct.ShowDialog() == true)
            {
                //...
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;
            // LINQ Language integrated query
            var data = from product in temp
                       where product.Name.ToLower().AccentRemoved()
                                .Contains(keyword.ToLower().AccentRemoved())
                       select product;
            productList.ItemsSource = data;
        }

		private string createID(string prefix)
		{
			var db = new FarmEntities();
			var count = 0;

			if (prefix == "P")
			{
				count = db.Products.Count();
			}
			else if (prefix == "CT")
			{
				count = db.Categories.Count();
			}

			return prefix + (count + 1).ToString("000");
		}
    }

    public static class StringExtension
    {
        public static string AccentRemoved(this string value)
        {
            byte[] temp;
            temp = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(value);
            value = System.Text.Encoding.UTF8.GetString(temp);
            return value;
        }
    }
}

