using Aspose.Cells;
using FarmManagement.Class;
using Microsoft.Win32;
using System;
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
    /// Interaction logic for CategoryControl.xaml
    /// </summary>
    public partial class CategoryControl : UserControl
    {
        public static Notify category_notification = new Notify();

        public CategoryControl()
        {
            InitializeComponent();
            categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
            category_notification.PropertyChanged += CategoryNotification_PropertyChanged;

        }
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
                    var name = sheet.Cells[$"B{row}"].StringValue;

                    var newCategory = new Category()
                    {
                        ID = IDGenerator.createID("CT"),
                        Name = name,
                        isDeleted = false,
                    };

                    MainWindow.db.Categories.Add(newCategory);
                    MainWindow.db.SaveChanges();

                    row++;
                    cell = sheet.Cells[$"{col}{row}"];
                }

                MessageBox.Show("Import successfully!", "Message");

                categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newCategory = new AddCategoryWindow();

            if (newCategory.ShowDialog() == true)
            {
                var category = new Category()
                {
                    ID = IDGenerator.createID("CT"),
                    Name = newCategory.CT_Name,                 
                    isDeleted = false,
                };

                MainWindow.db.Categories.Add(category);
                MainWindow.db.SaveChanges();

                categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Category selectedItem = (Category)categoryDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                var newCategory = new EditCategoryWindow(selectedItem);

                if (newCategory.ShowDialog() == true)
                {
                    var updatecategory = (from category in MainWindow.db.Categories where category.ID == selectedItem.ID select category).Single();
                    updatecategory.Name = newCategory.CT_Name;
                    MainWindow.db.SaveChanges();
                    ProductControl.product_notification.CategoryChange = true;

                    categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
                }
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Category selectedItem = (Category)categoryDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                string message = "Are you sure you want to delete '" + selectedItem.Name + "'? This will also delete all products in this category.";

                if (MessageBox.Show(message, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var deleteproduct = from product in MainWindow.db.Products
                                     where product.CategoryID == selectedItem.ID
                                     select product;
                    foreach (var product in deleteproduct)
                    {
                        MainWindow.db.Products.Remove(product);
                    }
                    MainWindow.db.SaveChanges();
                    ProductControl.product_notification.CategoryChange = true;

                    var deletecategory = (from category in MainWindow.db.Categories where category.ID == selectedItem.ID select category).Single();
                    MainWindow.db.Categories.Remove(deletecategory);
                    MainWindow.db.SaveChanges();

                    categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
                }
            }
        }

        private void KeywordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = keywordTextBox.Text;

            var data = from category in MainWindow.db.Categories.ToList()
                       where category.Name.ToLower().AccentRemoved().Contains(keyword.ToLower().AccentRemoved())
                       select category;
            categoryDataGrid.ItemsSource = data;
        }

        private void CategoryNotification_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
        }

        private void categoryDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row != null)
            {
                Category selectedItem = (Category)row.Item;

                var newCategory = new EditCategoryWindow(selectedItem);

                if (newCategory.ShowDialog() == true)
                {
                    var updatecategory = (from category in MainWindow.db.Categories where category.ID == selectedItem.ID select category).Single();
                    updatecategory.Name = newCategory.CT_Name;
                    MainWindow.db.SaveChanges();
                    ProductControl.product_notification.CategoryChange = true;

                    categoryDataGrid.ItemsSource = MainWindow.db.Categories.ToList();
                }
            }
        }
    }
}