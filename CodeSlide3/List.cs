using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide3
{
  public class Person
    {
        public int Age { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string ToString() => $"Name : {FirstName} {LastName}, Age: {Age}";
        
        
    }
    class Programe
    {
        static void Main(string[] args)
        {
            List<Person> people = new List<Person>()
            {
                new Person {FirstName ="David", LastName ="Simpson", Age= 50},
                new Person {FirstName ="Marge", LastName ="Simpson", Age= 445},
                new Person {FirstName ="Lisa", LastName ="Simpson", Age= 19},
                new Person {FirstName ="Jack", LastName ="Simpson", Age= 16},
            };
            Console.WriteLine("Item in List : {0}", people.Count);
            foreach (Person person in people)
            {
                Console.WriteLine(person);
            }
            Console.ReadLine();
        }
    }
}
