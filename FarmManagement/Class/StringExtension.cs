using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmManagement
{
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
