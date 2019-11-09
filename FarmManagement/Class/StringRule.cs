using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FarmManagement
{
    class StringRule : ValidationRule
    {
        public string stringPattern { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string String = "";

            try
            {
                String = (string)value;
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "There's something wrong!");
            }

            //string phonePattern = @"^[0][1-9]\d{8}$|^[0][1-9]\d{9}$";

            Regex regex = new Regex(stringPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var isValid = regex.IsMatch(String);

            if (isValid == false)
            {
                return new ValidationResult(false, "This value is not valid");
            }

            return ValidationResult.ValidResult;
        }
    }
}
