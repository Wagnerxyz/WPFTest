using System;

class MyClass
{
    static void Go()
    {
      //  LazyTest.Go();
      //  BeforeFieldInit.Program.Go(null);
        eventProgram.Go();
        Console.ReadLine();
    }
}

#region BeforeFieldInit
namespace BeforeFieldInit
{

    //beforefieldinit tiqian
    internal class Foo
    {

        public static string Field = GetString("初始化 Foo 静态成员变量!");
        //  public static string Field2 = GetString("初始化 FooStatic 静态成员变量222!");

        public static string GetString(string s)
        {
            Console.WriteLine(s);
            return s;
        }
    }

    //precise lazy dengganghao

    internal class FooStatic
    {
        static FooStatic() { Console.WriteLine("FooStatic 类构造函数"); }

        public static string Field = GetString("初始化 FooStatic 静态成员变量!");

        //  public static string Field2 = GetString("初始化 FooStatic 静态成员变量222!");
        public static string GetString(string s)
        {
            Console.WriteLine(s);
            return s;
        }
    }

    class Program
    {
        public static void Go(string[] args)
        {
            Console.WriteLine("Main 开始 ...");

            Foo.GetString("手动调用 Foo.GetString() 方法!");
            string info = Foo.Field;

            FooStatic.GetString("手动调用 FooStatic.GetString() 方法!");
            //  string infoStatic = FooStatic.Field2;
            string infoStatic = FooStatic.Field;

            Console.ReadLine();
        }
    }
}
#endregion

class Lazy
{
    private static int x = Log();
    private static int y = 0;

    private static int Log()
    {
        Console.WriteLine("Type initialized");
        return 0;
    }

    public static void StaticMethod()
    {
        Console.WriteLine("In static method");
    }

    public static void StaticMethodUsingField()
    {
        Console.WriteLine("In static method using field");
        Console.WriteLine("y = {0}", y);
    }

    public void InstanceMethod()
    {
        Console.WriteLine("In instance method");
    }
}

static class LazyTest
{
    public static void Go()
    {
        Console.WriteLine("Before static method");
        Lazy.StaticMethod();
        Console.WriteLine("Before construction");
        Lazy lazy = new Lazy();
        Console.WriteLine("Before instance method");
        lazy.InstanceMethod();
        Console.WriteLine("Before static method using field");
        Lazy.StaticMethodUsingField();
        Console.WriteLine("End");
    }
}

#region event
class eventProgram
{
    public static void Go()
    {
        Publishser pub = new Publishser();
        Subscriber sub = new Subscriber();

        pub.NumberChanged += new NumberChangedEventHandler(sub.OnNumberChanged);
        pub.DoSomething();          // 应该通过DoSomething()来触发事件
     //   pub.NumberChanged(100);     // 但可以被这样直接调用，对委托变量的不恰当使用
    }
}

// 定义委托
public delegate void NumberChangedEventHandler(int count);

// 定义事件发布者
public class Publishser
{
    private int count;
  //  public NumberChangedEventHandler NumberChanged;         // 声明委托变量
    public event NumberChangedEventHandler NumberChanged; // 声明一个事件

    public void DoSomething()
    {
        // 在这里完成一些工作 ...

        if (NumberChanged != null)
        {    // 触发事件
            count++;
            NumberChanged(count);
        }
    }
}

// 定义事件订阅者
public class Subscriber
{
    public void OnNumberChanged(int count)
    {
        Console.WriteLine("Subscriber notified: count = {0}", count);
    }
}

#endregion