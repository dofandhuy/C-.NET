using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoStaticConstructor
{
    class Student
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public bool Gender { get; set; }    

        public Student() { }

    }
     class Program
    {
        public static void Main(String[] args)
        {
            Student s2 = new Student() { Id = 1, Name = "Nguyen Van A", Gender = true };
        }
    }
}
