#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApplication1;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyExercise;

#endregion



public class ProductModel
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public ProductModel(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
    ProductModel() { }//无参构造很重要
    static public List<ProductModel> GetSampleProducts()
    {
        return new List<ProductModel>{
            new ProductModel("asd",12){Name="asd",Price=124},
             new ProductModel{Name="asd",Price=124},
              new ProductModel{Name="asd",Price=124}
        };
    }
}

class PollingLoopExample
{
    private bool _loop = true;
    public static void Run()
    {
        PollingLoopExample test1 = new PollingLoopExample();
        // Set _loop to false on another thread
        // new Thread(() => { test1._loop = false; }).Start();
        // Poll the _loop field until it is set to false
        while (test1._loop) ;
        // The previous loop may never terminate
    }
}
class Program
{

    static Stopwatch watch = new Stopwatch();
    private Mutex mtx = new Mutex(true);
    static int[] arr = new int[64 * 1024 * 1024];
    //  static int[] arr = new int[64 * 1024 * 1024];
    const int total = int.MaxValue;
    static int _Count = 0;
    string called = "";
    static void Main(string[] args)
    {
        CallWebApi.Run();//test git 0:53

        //ClientGetAsync.Run(new []{ "http://localhost:45719/" });

        //Task task = Task.Factory.StartNew(Decrement);
        //for (int i = 0; i < total; i++)
        //{
        //    _Count++;
        //}
        //task.Wait();
        //Console.WriteLine("Count={0}", _Count);
        MyExercise.Eventtest.Eventtest.Run();
        //  PollingLoopExample.Run();
        // FalseSharing.Run();
        //   ThreadTest.Run();
        //int[] numbers = { 3, 8, 7, 5, 2, 1, 9, 6, 4 };
        //int len = 9;

        //Console.WriteLine("QuickSort By Recursive Method");
        //QuickSort.QuickSort_Recursive(numbers, 0, len - 1);
        //for (int i = 0; i < 9; i++)
        //    Console.WriteLine(numbers[i]);

        // PixelProcess.Run();
        // ShellSortTest st=new ShellSortTest();
        //st.sortArray();
        // Example2();
        // Example3();

        //  Encrypt();
        //Program p = new Program();
        //Thread.CurrentThread.Name = "Main  ";
        //Thread worker = new Thread(p.ThreadEntry);
        //worker.Name = "worker";
        //worker.Start();
        //Thread.Sleep(TimeSpan.FromSeconds(1));
        //p.ThreadEntry();

    }

    private void ThreadEntry()
    {
        mtx.WaitOne();
        string name = Thread.CurrentThread.Name;
        called += String.Format("{0}:  {1}", name, DateTime.Now.Millisecond);
        Console.WriteLine(called);
        this.mtx.ReleaseMutex();
    }

    static private void Decrement()
    {
        for (int i = 0; i < total; i++)
        {
            _Count--;
        }
    }
    void Example2()
    {

        Console.WriteLine("Example 2:");

        for (int k = 1; k <= 1024; k *= 2)
        {

            watch.Restart();
            for (int i = 0; i < arr.Length; i += k) arr[i] *= 3;
            watch.Stop();
            Console.WriteLine("     K = " + k + ": " + watch.ElapsedMilliseconds.ToString() + " ms");

        }
        Console.WriteLine();
    }

    static void Example3()
    {

        Console.WriteLine("Example 3:");

        for (int k = 1; k <= 1024 * 1024; k *= 2)
        {

            //256* 4bytes per 32 bit int * k = k Kilobytes
            arr = new int[256 * k];

            int steps = 64 * 1024 * 1024; // Arbitrary number of steps
            int lengthMod = arr.Length - 1;

            watch.Restart();
            for (int i = 0; i < steps; i++)
            {
                arr[(i * 16) & lengthMod]++; // (x & lengthMod) is equal to (x % arr.Length)
            }

            watch.Stop();
            Console.WriteLine("     Array size = " + arr.Length * 4 + " bytes: " + (int)(watch.Elapsed.TotalMilliseconds * 1000000.0 / arr.Length) + " nanoseconds per element");

        }
        Console.WriteLine();
    }

    //unsafe static void Main()
    //{
    //    //  new WeakRefernceTest().Test();
    //    // new StopWatchtest().Run();
    //    int asd = sizeof(DateTime);
    //    Console.WriteLine(asd);
    //    Console.ReadKey();
    //}



    private static void DateTimeOutPut()
    {
        DateTime dateValue = new DateTime(2008, 6, 15, 21, 15, 07);
        // Create an array of standard format strings.
        string[] standardFmts =
        {
            "d", "D", "f", "F", "g", "G", "m", "o",
            "R", "s", "t", "T", "u", "U", "y"
        };
        // Output date and time using each standard format string.
        foreach (string standardFmt in standardFmts)
            Console.WriteLine("{0}: {1}", standardFmt,
                              dateValue.ToString(standardFmt));
        Console.WriteLine(dateValue.ToString("h:mmm:ss"));
    }

    static void DisplayMemory()
    {
        Console.WriteLine("Total memory: {0:###,###,###,##0} bytes", GC.GetTotalMemory(true));
        Console.WriteLine();
    }

    private static void TrySerialize()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fs = new FileStream("aaa", FileMode.Create))
        {
            Product product = new Product("Data Source=.;Initial Catalog=DB;User ID=sa;Password=123");

            bf.Serialize(fs, product);
        }

        FileStream stream = File.OpenRead("aaa");
        Product deserialized = new Product();
        deserialized = (Product)bf.Deserialize(stream);
        Console.WriteLine(deserialized);
        //using (FileStream fs = new FileStream("bbb", FileMode.Create))
        //{
        //    Product product = new Product("Data Source=.;Initial Catalog=DB;User ID=sa;Password=123");
        //    XmlSerializer bf = new XmlSerializer(typeof(Product));
        //    bf.Serialize(fs, product);
        //}
    }

    private static void CustomCollection()
    {
        var collection = Artist.GetSample();

        for (int i = 0; i < collection.Count(); i++)
        {
            Console.WriteLine(collection[i].ArtistId + collection[i].Name);
        }
        foreach (var item in collection)
        {
            Console.WriteLine(item.ArtistId + item.Name);
        }

    }
    private static void Encrypt()
    {
        string s = "hello";
        string miwen = SymmetricCryptoHelper.SymmetricEncrypt(s);
        Console.WriteLine(miwen);

        string jiemi = SymmetricCryptoHelper.SymmetricDecrypt(miwen);
        Console.WriteLine(jiemi);
    }
}


class DisplaySettingsListener : IDisposable
{
    byte[] m_ExtraMemory = new byte[1000000];

    public DisplaySettingsListener()
    {
        SystemEvents.DisplaySettingsChanged += new EventHandler(ehDisplaySettingsChanged);
    }

    public void CleanUp()
    {

    }

    void ehDisplaySettingsChanged(object sender, EventArgs e)
    {
    }

    public void Dispose()
    {
        SystemEvents.DisplaySettingsChanged -= ehDisplaySettingsChanged;
    }


}


