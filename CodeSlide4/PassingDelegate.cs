using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSle4
{
    public delegate void MyDelegate(string msg);
    class MyClass
    {
        public static void Print(string message) => Console.WriteLine($"{message.ToUpper()}");
        public static void Show(string message) => Console.WriteLine($"{message.ToLower()}");

    }
    class Program
    {
        static void InvokeDelegate(MyDelegate dele, string msg) => dele(msg);
        static void Main(string[] args)
        {
            string msg = "Passing delegate as a parameter";
            InvokeDelegate(MyClass.Print, msg);
            InvokeDelegate(MyClass.Show, msg);
            Console.ReadLine();
        }
    }
}
