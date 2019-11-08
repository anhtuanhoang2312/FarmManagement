using Aspose.Cells;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        FarmEntities db = new FarmEntities();

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            ThemeEffectsHelper.IsAcrylicEnabled = true;
            FluentPalette.LoadPreset(FluentPalette.ColorVariation.Light);
            InitializeComponent();
            productDataGrid.ItemsSource = db.Products.ToList();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Excel files | *.xls;*.xlsx";

            if (screen.ShowDialog() == true)
            {
                var workbook = new Workbook(screen.FileName);

                var sheet = workbook.Worksheets[0];

                var col = "A";
                var row = 2;

                var cell = sheet.Cells[$"{col}{row}"];

                while (cell.Value != null)
                {
                    var id = sheet.Cells[$"A{row}"].StringValue;
                    var name = sheet.Cells[$"B{row}"].StringValue;
                    var price = sheet.Cells[$"C{row}"].DoubleValue;
                    var weight = sheet.Cells[$"D{row}"].DoubleValue;
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
                        isDeleted = false
                        //Picture = uniqueName,
                    };

                    db.Products.Add(newProduct);
                    db.SaveChanges();

                    row++;
                    cell = sheet.Cells[$"{col}{row}"];
                }

                MessageBox.Show("Import successfully!", "Message");

                productDataGrid.ItemsSource = db.Products.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new AddProductWindow();

            if (newProduct.ShowDialog() == true)
            {
                var product = new Product()
                {
                    ID = "SP004",
                    Name = newProduct.P_Name,
                    Price = newProduct.P_Price,
                    Weight = newProduct.P_Weight,
                    isDeleted = false
                    //Picture = uniqueName,
                };

                db.Products.Add(product);
                db.SaveChanges();

                productDataGrid.ItemsSource = db.Products.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)productDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var newProduct = new EditProductWindow();
                newProduct.P_Name = selectedItem.Name;
                newProduct.P_Price = selectedItem.Price ?? 0; // 0 se la gia tri mac dinh neu Price mang gia tri NULL
                newProduct.P_Weight = selectedItem.Weight ?? 0;

                if (newProduct.ShowDialog() == true)
                {
                    var update = (from product in db.Products where product.ID == selectedItem.ID select product).Single();
                    update.Name = newProduct.P_Name;
                    update.Price = newProduct.P_Price;
                    update.Weight = newProduct.P_Weight;
                    db.SaveChanges();

                    productDataGrid.ItemsSource = db.Products.ToList();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)productDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var delete = (from product in db.Products where product.ID == selectedItem.ID select product).Single();
                db.Products.Remove(delete);
                db.SaveChanges();

                productDataGrid.ItemsSource = db.Products.ToList();
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from product in db.Products.ToList()
                       where product.Name.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved()) 
                       select product;
            productDataGrid.ItemsSource = data;
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

