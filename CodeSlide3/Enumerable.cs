using System;
using System.Collections; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace odeSlide3
{
    public class Person
    {
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Person() { }

        public override string ToString() => $"Name: {FirstName} {LastName}, Age :{Age}";
    }

    public class MyCollection<T> : IEnumerable where T : class, new()
    {
        private List<T> list = new List<T>();

       
        public void AddItem(params T[] item) => list.AddRange(item);

        
        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        
        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    }

    class Program
    {

        static void Main(string[] args)
        {

            MyCollection<Person> collection = new MyCollection<Person>();

            var p1 = new Person { FirstName = "David", LastName = "Simpson", Age = 50 };
           
            var p2 = new Person { FirstName = "Alice", LastName = "Smith", Age = 25 };
            var p3 = new Person { FirstName = "Bob", LastName = "Johnson", Age = 30 };
            var p4 = new Person { FirstName = "Charlie", LastName = "Brown", Age = 45 };
            collection.AddItem(p1, p2, p3, p4);
            foreach (var p in collection)
            {
                Console.WriteLine(p);
            }
            Console.ReadKey();
        }
    }
}