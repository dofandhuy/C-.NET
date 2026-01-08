using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
    class abc
    {
        static void Main(string[] args)
        {
            var obj1 = new { id = 1000, name = "Jack" };
            Console.WriteLine($"Id : {obj1.id}, name: {obj1.name}");
            dynamic obj2 = new { id = 1000, name = "scott", Email = "scott@gmail.com" };
            Console.WriteLine($"id: {obj2.id}, name: {obj2.name}, email: {obj2.Email}");
            Console.ReadLine();
        }
    }
}
