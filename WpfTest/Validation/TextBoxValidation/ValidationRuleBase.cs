using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfTest
{
    public abstract class ValidationRuleBase : ValidationRule
    {
        public int Length { get; set; }
        public int Number { get; set; }
        public double Min { get; set; }

        public double Max { get; set; }
    }
}
