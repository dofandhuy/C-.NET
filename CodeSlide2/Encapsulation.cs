using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSide2
{
   class Customer
    {
        private int Id;
        public int CustomerId
        {
            get { return Id; }
            set { Id = value; }
        }
        public string CustomerName { get; set; } = "New Customer";
        public void Print()
        {
            Console.WriteLine($"ID: {CustomerId}, Name:{CustomerName}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Customer obj= new Customer();
            obj.CustomerId = 1000;
            Console.WriteLine($"ID:{obj.CustomerId}, Name:{obj.CustomerName}");
            obj.CustomerId = 2000;
            obj.CustomerName = "Jack";
            obj.Print();
            Console.ReadLine();
        }
    }
}
