using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FarmManagement
{
    class Control
    {
        public static void Show(Grid g, UserControl uc)
        {
            if (g.Children.Count > 0)
            {
                g.Children.Clear();
                g.Children.Add(uc);
            }
            else
            {
                g.Children.Add(uc);
            }
        }
    }
}
