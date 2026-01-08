using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
    // static class
    static class UtilityClass
    {
        public static void PrintTime()
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString());
        }
        public static void PrintDate()
        {
            Console.WriteLine(DateTime.Today.ToShortDateString());
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            UtilityClass.PrintDate();
        }
    }

    // static constructor 
    class MyClass
    {
        public static int x = 1;
        static MyClass()
        {
            x = 2;
            Console.WriteLine("Static constructor: x = {0}", x);
        }
        public MyClass()
        {
            x++;
            Console.WriteLine("Object constructor: x= {0}",x);
        }
        class PRogram
        {
            static void Main(String[] args)
            {
                MyClass m1= new MyClass();
                MyClass.x = 4;
                MyClass m2= new MyClass();
            }
        }
    }
}
