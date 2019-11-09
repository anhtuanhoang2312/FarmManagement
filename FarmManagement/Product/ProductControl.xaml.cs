using Aspose.Cells;
using FarmManagement.Class;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
            productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
        }

        //public static IEnumerable getQueryTable()
        //{
        //    var query = (from product in MainWindow.db.Products
        //                 join category in MainWindow.db.Categories on product.CategoryID equals category.ID
        //                 select new { product.Name, category.CategoryName, product.Price, product.Weight });
        //    return query;
        //}

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Excel files|*.xls;*.xlsx";

            if (screen.ShowDialog() == true)
            {
                var workbook = new Workbook(screen.FileName);

                var sheet = workbook.Worksheets[0];

                var col = "A";
                var row = 2;

                var cell = sheet.Cells[$"{col}{row}"];

                while (cell.Value != null)
                {
                    var categoryname = sheet.Cells[$"C{row}"].StringValue;

                    bool has = MainWindow.db.Categories.ToList().Any(cus => cus.Name == categoryname);
                    if (has == true)
                    {
                        var name = sheet.Cells[$"B{row}"].StringValue;
                        var price = sheet.Cells[$"D{row}"].DoubleValue;
                        var weight = sheet.Cells[$"E{row}"].DoubleValue;
                        var imageName = sheet.Cells[$"F{row}"].StringValue;

                        var imageSourceInfo = new FileInfo(screen.FileName);
                        var imageSourceFullPath = $"{imageSourceInfo.DirectoryName}\\images\\{imageName}";
                        var imageSourceFileInfo = new FileInfo(imageSourceFullPath);

                        var uniqueName = $"{Guid.NewGuid()}.{imageSourceFileInfo.Extension}";

                        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        var destinationPath = $"{baseDirectory}image\\{uniqueName}";

                        File.Copy(imageSourceFullPath, destinationPath);

                        var newProduct = new Product()
                        {
                            ID = IDGenerator.createID("P"),
                            Name = name,
                            CategoryID = (from category in MainWindow.db.Categories where category.Name == categoryname select category.ID).Single(),
                            Price = price,
                            Weight = weight,
                            isDeleted = false,
                            Picture = uniqueName,
                        };

                        MainWindow.db.Products.Add(newProduct);
                        MainWindow.db.SaveChanges();

                        row++;
                        cell = sheet.Cells[$"{col}{row}"];
                    }
                    else
                    {
                        
                        string message = "The category '" + categoryname + "' does not exist. Do you want to add new category?";

                        if (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            var category = new Category()
                            {
                                ID = IDGenerator.createID("CT"),
                                Name = categoryname,
                                isDeleted = false,
                            };

                            MainWindow.db.Categories.Add(category);
                            MainWindow.db.SaveChanges();
                            CategoryControl.notification.CategoryChange = true;

                            var name = sheet.Cells[$"B{row}"].StringValue;
                            var price = sheet.Cells[$"D{row}"].DoubleValue;
                            var weight = sheet.Cells[$"E{row}"].DoubleValue;
                            var imageName = sheet.Cells[$"F{row}"].StringValue;

                            var imageSourceInfo = new FileInfo(screen.FileName);
                            var imageSourceFullPath = $"{imageSourceInfo.DirectoryName}\\images\\{imageName}";
                            var imageSourceFileInfo = new FileInfo(imageSourceFullPath);

                            var uniqueName = $"{Guid.NewGuid()}.{imageSourceFileInfo.Extension}";

                            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            var destinationPath = $"{baseDirectory}image\\{uniqueName}";

                            File.Copy(imageSourceFullPath, destinationPath);

                            var newProduct = new Product()
                            {
                                ID = IDGenerator.createID("P"),
                                Name = name,
                                CategoryID = (from category1 in MainWindow.db.Categories where category1.Name == categoryname select category1.ID).Single(),
                                Price = price,
                                Weight = weight,
                                isDeleted = false,
                                Picture = uniqueName,
                            };

                            MainWindow.db.Products.Add(newProduct);
                            MainWindow.db.SaveChanges();

                            row++;
                            cell = sheet.Cells[$"{col}{row}"];

                        }
                        else
                        {
                            row++;
                            cell = sheet.Cells[$"{col}{row}"];
                        }
                    }
                }

                MessageBox.Show("Import successfully!", "Message");

                productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new AddProductWindow();

            if (newProduct.ShowDialog() == true)
            {
                var product = new Product()
                {
                    ID = IDGenerator.createID("P"),
                    Name = newProduct.P_Name,
                    CategoryID = newProduct.P_CategoryID,
                    Price = newProduct.P_Price,
                    Weight = newProduct.P_Weight,
                    isDeleted = false,
                    Picture = newProduct.P_Picture,
                };

                MainWindow.db.Products.Add(product);
                MainWindow.db.SaveChanges();

                productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)productDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var newProduct = new EditProductWindow(selectedItem.CategoryID, selectedItem.Picture);

                newProduct.P_Name = selectedItem.Name;
                newProduct.P_Price = selectedItem.Price ?? 0; // 0 se la gia tri mac dinh neu Price mang gia tri NULL
                newProduct.P_Weight = selectedItem.Weight ?? 0;

                if (newProduct.ShowDialog() == true)
                {
                    var update = (from product in MainWindow.db.Products where product.ID == selectedItem.ID select product).Single();
                    update.Name = newProduct.P_Name;
                    update.CategoryID = newProduct.P_CategoryID;
                    update.Price = newProduct.P_Price;
                    update.Weight = newProduct.P_Weight;
                    update.Picture = newProduct.P_Picture;
                    MainWindow.db.SaveChanges();

                    productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedItem = (Product)productDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var delete = (from product in MainWindow.db.Products where product.ID == selectedItem.ID select product).Single();
                MainWindow.db.Products.Remove(delete);
                MainWindow.db.SaveChanges();

                productDataGrid.ItemsSource = MainWindow.db.Products.ToList();
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from product in MainWindow.db.Products.ToList()
                       where product.Name.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved())
                       select product;
            productDataGrid.ItemsSource = data;
        }
    }
}