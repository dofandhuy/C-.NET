using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide1
{
    class Boxing_Unboxing
    {
        //static void Main(String [] args)
        //{
        //    int i = 123;
        //    object o = i;
        //    i = 456;
        //    Console.WriteLine("THe value type value = {0}", i);
        //    Console.WriteLine("The object-type value = {0}", o);
        //    Console.ReadLine();
        //}

        static void Main(String [] args)
        {
            int i = 123;
            object o = i;
            int j = (int)o;
            Console.WriteLine("i ={0}, o= {1}, j ={2}", i, o, j);
            Console.ReadLine();
        }
    }
}
