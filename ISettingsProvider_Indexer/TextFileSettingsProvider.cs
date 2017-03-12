using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ISettingsProvider_Indexer
{
    class TextFileSettingsProvider : ISettingProvider
    {
        private string filename = "myconfig.txt";
        Dictionary<string, string> dict = new Dictionary<string, string>();
        public TextFileSettingsProvider()
        {
            string[] arr = File.ReadAllLines(filename);
            foreach (var item in arr)
            {
                string[] arr2 = item.Split('=');
                dict.Add(arr2[0], arr2[1]);
            }
        }
        public string this[string name]
        {
            get
            {

                return dict[name];
             //   string[] arr = File.ReadAllLines(filename);
                //foreach (var item in arr)
                //{
                //    string[] arr2 = item.Split('=');
                //    if (arr2[0] == name)
                //    {
                //        return arr2[1];
                //    }
                //}
                ;
               // throw new KeyNotFoundException("can't find");
            }

            set
            {
                string[] arr = File.ReadAllLines(filename);
                foreach (var item in arr)
                {
                    string[] arr2 = item.Split('=');
                    if (arr2[0] == name)
                    {
                        arr2[1] = value;
                        File.WriteAllLines(filename, arr);
                        return;
                    }

                }
                File.AppendAllText(filename, "\r\n" + name + "=" + value);
            }
        }

        public string[] Names
        {
            get
            {
                return dict.Keys.ToArray();
            }
        }

        public bool NameExists(string name)
        {


            if (dict.ContainsKey(name))
            {
                return true;
            }
            else
            {
                return false;
            }
            //string[] arr = File.ReadAllLines(filename);
            //foreach (var item in arr)
            //{
            //    string[] arr2 = item.Split('=');
            //    if (arr2[0] == name)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
    }
}
