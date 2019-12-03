using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FarmManagement
{ 
    class IDGenerator
    {
        public static string createID(string prefix)
        {
            var count = 0;

            if (prefix == "P")
            {
                if (MainWindow.db.Products.Count() > 0)
                {
                    count = int.Parse(MainWindow.db.Products.ToList()[MainWindow.db.Products.Count() - 1].ID.ToString().Substring(1, 3));
                }
            }
            else if (prefix == "CT")
            {
                if (MainWindow.db.Categories.Count() > 0)
                {
                    count = int.Parse(MainWindow.db.Categories.ToList()[MainWindow.db.Categories.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "C")
            {
                if (MainWindow.db.Customers.Count() > 0)
                {
                    count = int.Parse(MainWindow.db.Customers.ToList()[MainWindow.db.Customers.Count() - 1].ID.ToString().Substring(1, 3));
                }
            }
            else if (prefix == "I")
            {
                if (MainWindow.db.Invoices.Count() > 0)
                {
                    count = int.Parse(MainWindow.db.Invoices.ToList()[MainWindow.db.Invoices.Count() - 1].ID.ToString().Substring(1, 3));
                }
            }

            return prefix + (count + 1).ToString("000");
        }
    }
}
