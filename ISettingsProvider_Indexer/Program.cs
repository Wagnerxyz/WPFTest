using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISettingsProvider_Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            string providerName = ConfigurationManager.AppSettings["SettingsProvider"];
            ISettingProvider sp;
            if (providerName == "Access")
            {
                sp = new AccessSettingsProvider();
            }
            else if (providerName == "TextFile")
            {
                sp = new TextFileSettingsProvider();
            }
            else if (providerName == "Sql")
            {
                sp = new SqlSettingsProvider();
            }
            else
            {
                throw new Exception("错误的providerName");
            }

            //ISettingsProvider sp = new AccessSettingsProvider();
            //ISettingsProvider sp = new TextFileSettingsProvider();
            if (sp.NameExists("User"))
            {
                string v1 = sp["User"];
                Console.WriteLine(v1);
            }

            sp["Password"] = "8848";
            sp["Age"] = "500";

            foreach (string name in sp.Names)
            {
                Console.WriteLine(name);
            }
            Console.ReadKey();
        }
    }
}

