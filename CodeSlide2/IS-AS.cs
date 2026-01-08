using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
  interface ICaculate
    {
        double Area();
    }
    class Rectangle: ICaculate
    {
        float length;
        float width;
        public Rectangle(float x, float y)
        {
            length = x;
            width = y;
        }
        public double Area()
        {
            return length * width;
        }
    }
    class Programm
    {
        static void Main(String[] args)
        {
            Rectangle objRectangle = new Rectangle(10.2F, 20.3F);
            ICaculate caculate;
            if(objRectangle is ICaculate)
            {
                caculate = objRectangle as ICaculate;
                Console.WriteLine("Area: {0:F2}", caculate.Area());
            }else
            {
                Console.WriteLine("Interface method not implemented");
            }
        }
    }
}
