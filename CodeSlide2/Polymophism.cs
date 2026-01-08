using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdfs
{
   public abstract class Shape
    {
        public int Height {  get; set; }
        public int Width { get; set; }

        public virtual void Draw()
        {
            Console.WriteLine("Performing base class Drawing tasks");
        }
        public void Print()
        {
            Console.WriteLine("Performing base class Printing tasks");
        }
        public abstract void Display();
    }
    public class Circle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing a circle");
        }
        public override void Display()
        {
            Console.WriteLine("Display circle");
        }
    }
    public class Rectangle : Shape
    {
        public override void Draw()
        {
            Console.WriteLine("Drawing a rectangle");
        }
        public override void Display()
        {
            Console.WriteLine("Display rectangle");
        }
        public new void Print()
        {
            Console.WriteLine("Printing rectangle ");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Shape p= new Circle();
            p.Display();
            p = new Rectangle();
            p.Display();
            p.Draw();
            p.Print();
            Console.ReadLine();

        }
    }
}
