using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdeSlide4
{
    public delegate void MyDelegate(string msg);
    class MyClass
    {
        public static void Print(string message) => Console.WriteLine($"{message.ToUpper()}");
        public static void Show(string message) => Console.WriteLine($"{message.ToLower()}");
        public static void Display(string message) => Console.WriteLine($"{message}");

    }
    class Program
    {
        static void Main(string[] args)
        {
            string msg = "Multicast delegate";
            MyDelegate MyDele01 = MyClass.Print;
            MyDelegate MyDele02 = MyClass.Show;
            Console.WriteLine("****** Combines MyDele01 + MyDele02*****");
            MyDelegate MyDele = MyDele01 + MyDele02;
            MyDele(msg);
            Console.WriteLine("****Combines MyDele1+ MyDelee02 + MyDele****");
            MyDelegate MyDele03 = MyClass.Display;
            MyDele += MyDele03;
            MyDele(msg);
            Console.WriteLine("********************");
            Console.WriteLine("******Remove My Delegate*************");
            MyDele -= MyDele02;
            MyDele("Hello world !");
            Console.ReadLine();

        }
    }

}
