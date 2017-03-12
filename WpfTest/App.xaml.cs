using System;
using System.Globalization;
using System.Reflection;
using System.Runtime;
using System.Windows;
using WpfTest.Helper;
using System.Linq;

namespace WpfTest
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ProfileOptimization.SetProfileRoot(@"D:\ProfileFile");

            ProfileOptimization.StartProfile("ProfileFile");
            base.OnStartup(e);
            DispatcherHelper.Initialize();
            //LoadTheme();
           // LoadLanguage();

            //把引用程序集的copy local设为false 项目内引用此程序集且生成操作设为嵌入的资源输出目录就不会单独出现这个文件，
            //本地找不着则需要用这个加载。似乎是让多个程序集合并成一个执行的方法
             AppDomain.CurrentDomain.AssemblyResolve += ResolveEventHandler;
        }
        private void LoadTheme()
        {
            ResourceDictionary resource = null;

            try
            {
                resource = Application.LoadComponent((new Uri("/PresentationFramework.Aero, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35;component/themes/Aero.normalcolor.xaml", UriKind.Relative))) as ResourceDictionary;
            }
            catch (Exception)
            {
                return;
            }

            if (resource != null)
            {
                if (Resources.MergedDictionaries.Count > 0)
                {
                    Resources.MergedDictionaries.Clear();
                }
                Resources.MergedDictionaries.Add(resource);
            }
        }
        private void LoadLanguage()
        {
            // CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo currentCultureInfo = CultureInfo.CurrentCulture;

            ResourceDictionary langRd = null;

            try
            {
                langRd =
                    LoadComponent(
                        new Uri(@"Lang\" + currentCultureInfo.Name + ".xaml", UriKind.Relative))
                        as ResourceDictionary;
            }
            catch
            {
            }

            if (langRd != null)
            {
                if (Resources.MergedDictionaries.Count > 0)
                {
                    Resources.MergedDictionaries.Clear();
                }
                Resources.MergedDictionaries.Add(langRd);
            }
        }
        private static Assembly ResolveEventHandler(Object sender, ResolveEventArgs args)
        {
            // Debugger.Break();
            String dllName = new AssemblyName(args.Name).Name + ".dll";

            var assem = Assembly.GetExecutingAssembly();
            var a = assem.GetManifestResourceNames();
            String resourceName = a.FirstOrDefault(rn => rn.EndsWith(dllName));
            if (resourceName == null) return null; // Not found, maybe another handler will find it
            using (var stream = assem.GetManifestResourceStream(resourceName))
            {
                Byte[] assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }
    }
}