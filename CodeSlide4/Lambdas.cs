using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide4
{
   class Program2
    {

        // lambdas expressions 
        //static void Main(string[] args)
        //{
        //    int n1 = 35;
        //    int n2 = 45;
        //    int result;

        //    Func<int, int, int > add = ((a, b) => a + b);
        //    result= add(n1, n2);
        //    Console.WriteLine($"{n1} + {n2} = {result}");
        //    Console.ReadLine(); 
        //}


        // lambdas with Standard Query Operators (SQO)
        //static void Main(string[] args)
        //{
        //    string[] names= { "Mary", "John", "Peter", "Sally", "Jane", "Paul" };
        //    foreach(string item in names.OrderBy(n => n))
        //    {
        //        Console.WriteLine(item);
        //    }
        //    Console.ReadLine();
        //}

        // LINQ to Object with Query Expressions 

        static void Main(string[] args)
        {
            string[] names= {"David", "Mary", "John", "Sally", "Jane", "Paul" };
        var items = from word in names
                    where word.Contains("a")
                    select word;
            foreach (var item in items)
            {
                Console.WriteLine(item);

            }
            Console.ReadLine();
        }
        }
    }

