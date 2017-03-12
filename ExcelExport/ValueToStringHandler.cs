using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelTest
{
    /// <summary>
    /// 值转字符串处理器
    /// </summary>
    public class ValueToStringHandler
    {
        /// <summary>
        /// 验证该处理器是否针对指定值的验证器
        /// </summary>
        public Func<object, bool> ValueValidator { get; private set; }
        /// <summary>
        /// 转换器，如果验证通过，则使用此转换器将值转换为字符串
        /// </summary>
        public Func<object, string> Converter { get; private set; }

        public ValueToStringHandler(Func<object, bool> validator, Func<object, string> converter)
        {
            this.ValueValidator = validator;
            this.Converter = converter;
        }
    }
}
