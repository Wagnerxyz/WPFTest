using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
     class AAA
    {
        /// <summary>
        /// ID
   
        public string Name { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 概要说明
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
    
       
     
        /// <summary>
        /// 实现了事实定义或事实的Provider接口的类型所在的程序集名称
        /// </summary>
        public string ProviderAssemblyName { get; set; }

        /// <summary>
        /// 实现了事实定义或事实的Provider接口的类型全名（包括命名空间）
        /// </summary>
        public string ProviderTypeName { get; set; }

        /// <summary>
        /// 配置编辑器类型名称
        /// </summary>
        public string ConfigEditorTypeName { get; set; }

        /// <summary>
        /// 配置编辑器所在程序集名称
        /// </summary>
        public string ConfigEditorAssemblyName { get; set; }

    }

    class AAACollection : List<AAA>
    {
        // Default constructor is required for usage in the WPF Designer.
        private AAACollection() { }
    }
    
}
