using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FarmManagement.Validation
{
    class NumberRule : ValidationRule
    {
        public string numberPattern { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            string Number = "";

            try
            {
                Number = (string)value;
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "There's something wrong!");
            }

            //string phonePattern = @"^[0][1-9]\d{8}$|^[0][1-9]\d{9}$";

            Regex regex = new Regex(numberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var isValid = regex.IsMatch(Number);

            if (isValid == false)
            {
                return new ValidationResult(false, "This value is not valid");
            }

            return ValidationResult.ValidResult;
        }
    }
}
