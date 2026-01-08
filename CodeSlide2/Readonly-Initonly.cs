using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeSlide2
{
   public class MyClass
    {
        public int x {  get; set; }
        public int y { get;  }

        public MyClass() {
            x = 10;
            y = 20;
        }
        public MyClass(int a, int b) 
        {
            x = a;
            y = b;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj1 = new MyClass { x = 1 };
            Console.WriteLine($"x: {obj1.x}, y:{obj1.y}");
            MyClass obj2 = new MyClass();
            Console.WriteLine($"x:{obj2.x}, y:{obj2.y}");
            MyClass obj3 = new MyClass(30,50);
            Console.WriteLine($"x:{obj3.x}, y: {obj3.y}");
            Console.ReadLine();
        }
    }
}
