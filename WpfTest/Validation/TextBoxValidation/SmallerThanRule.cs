using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfTest
{
    public class SmallerThanRule : ValidationRuleBase
    {
        public int Number { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || Number == null)
            {
                return new ValidationResult(false, "请输入比较的值，且比较的值不能为空");
            }

            int v;
            if (!int.TryParse(value.ToString(), out v))
                return new ValidationResult(false, "请输入数字");

            return v < Number
                       ? new ValidationResult(true, null)
                       : new ValidationResult(false, string.Format("{0}必需小于{1}", v, Number));
        }
    }
}
