using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
  public record Customer
    {
        public string Name { get; init; } = "New Customer";
        public int Age { get; init; } = 20;

        public void Print()
        {
            Console.WriteLine($" NAme: {Name}, Age:{Age}");
        }
    }
    class Programw
    {
        static void Main(string[] args) {
            Customer customer01 = new Customer { Name = "jack", Age = 25 };
            customer01.Print();
            Customer customer02 = customer01 with { Name = "John" };
            customer02.Print();
            Customer customer03 = new();
            customer03.Print();
            Console.ReadLine();
        }
    }
}
