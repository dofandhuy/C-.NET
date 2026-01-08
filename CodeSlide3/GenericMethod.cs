using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide3
{
 public class MyClass
    {
        public void Display<T,U>(T msg, U value)
        {
            Console.WriteLine($"{msg}: {value}");
        }
    }
    class Programn
    {
        static void Main(string[] args)
        {
            MyClass obj= new MyClass();
            obj.Display<string, int>("Integer", 2050);
            obj.Display<double, char>(155.9, 'A');
            obj.Display<float, double>(358.9F, 255.67);
            Console.ReadLine();
        }
    }
}
