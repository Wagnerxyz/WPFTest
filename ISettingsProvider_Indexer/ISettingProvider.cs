using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISettingsProvider_Indexer
{
    interface ISettingProvider
    {
        string this[string name] { get; set; }//以字符串类型读取、设置配置项的值
        string[] Names { get; }//所有配置名
        bool NameExists(string name);//判断指定名字的配置是否存在
    }
}
