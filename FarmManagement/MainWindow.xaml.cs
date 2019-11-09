﻿using Aspose.Cells;
using FarmManagement.Class;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.MaterialControls;

namespace FarmManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static FarmEntities db = new FarmEntities();
        public static ProductControl productpg = new ProductControl();
        public static CategoryControl categorypg = new CategoryControl();

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new FluentTheme();
            ThemeEffectsHelper.IsAcrylicEnabled = true;
            FluentPalette.LoadPreset(FluentPalette.ColorVariation.Light);
            InitializeComponent();
        }

        private void navigationView_ItemClick(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as RadNavigationViewItem;
            if (item != null)
            {
                Debug.WriteLine(item.Name);

                if (item.Name == "Product")
                {
                    Control.Show(MainContent, productpg);
                }
                else if (item.Name == "Category")
                {
                    Control.Show(MainContent, categorypg);
                }
            }
        }
    }
}

