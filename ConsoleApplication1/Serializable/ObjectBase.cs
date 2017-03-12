using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 假如外部父类未被标记为序列化，自定义序列化可not add Name Property
/// </summary>
    public class ObjectBase
    {
        public string Name { get; set; }
    }

