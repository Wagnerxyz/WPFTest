using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfTest
{
    public class IsNumberRule : ValidationRuleBase
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double aa;
            bool success = double.TryParse(value.ToString(),out aa);
            if (success)
            {
                return new ValidationResult(true,null);
            }
            return new ValidationResult(false, "请输入数字");
        }
    }
}
