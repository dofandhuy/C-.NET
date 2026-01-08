using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CodeSlide4
{
  class Programr
    {
        static int Sum(int x, int y) => x + y;
        static void Print(string msg) => Console.WriteLine(msg.ToUpper());
        static void Main(string[] args)
        {
            int a = 15, b = 25, s;
            string strResult;
            Func<int, int, int> sumFunx = Sum;
            s = sumFunx(a, b);
            strResult = $"{a}+{b}={s}";
            Console.WriteLine("******Invoke Print method by Action delegate*****");
            Action<string> action = Print;
            Console.ReadLine();
        }

    }
}
