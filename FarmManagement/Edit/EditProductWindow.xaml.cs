﻿using System;
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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public string P_Name { get; set; }
        public string P_CategoryID { get; set; }
        public double P_Price { get; set; }
        public double P_Weight { get; set; }

        public EditProductWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            P_Name = NameTextBox.Text;
            P_Price = double.Parse(PriceTextBox.Text);
            P_Weight = double.Parse(WeightTextBox.Text);
            this.DialogResult = true;
        }
    }
}
