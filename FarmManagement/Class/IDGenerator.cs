using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagement
{ 
    class IDGenerator
    {
        public static string createID(string prefix)
        {
            var count = 0;

            if (prefix == "P")
            {
                count = MainWindow.db.Products.Count();
            }
            else if (prefix == "CT")
            {
                count = MainWindow.db.Categories.Count();
            }

            return prefix + (count + 1).ToString("000");
        }
    }
}
