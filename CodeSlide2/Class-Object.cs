using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
  public class Car
    {
        public string Make;
        public string Model;
        public void Starting()
        {
            Console.WriteLine($"{Model} is starting");
        }

        public void Accelerating()
        {
            Console.WriteLine($"{Model} is accelerating");
        }
        public void Braking()
        {
            Console.WriteLine($"{Model} is braking");

        }
        public override string ToString()
        {
            return $"Make={Make}, Model= {Model}";
        } 
    }
    class Progarm
    {
        static void Main(string[] args)
        {
            Car wwPolo= new Car();
            wwPolo.Make = "2050";
            wwPolo.Model = "Volkswagen Polo";
            wwPolo.Accelerating();
            wwPolo.Braking();
            wwPolo.Starting();
            Console.WriteLine($"Car : {wwPolo}");
            Console.ReadLine();
        }
    }
}
