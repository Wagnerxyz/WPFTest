using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfTest
{
    public class TextLengthRule : ValidationRuleBase
    {
       // public int Length { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string aa = value.ToString();
            if (value == String.Empty)
            {
                return new ValidationResult(false, "请输入值");
            }

            return aa.Length < Length
                       ? new ValidationResult(true, null)
                       : new ValidationResult(false, string.Format("输入长度{0}超出最大长度{1}", aa.Length, Length));
        }
    }
}
