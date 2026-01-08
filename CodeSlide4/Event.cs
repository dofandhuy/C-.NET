using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide4
{
    public delegate void PrintDetail(string msg);
    class Programv
    {
        event PrintDetail Print;
        void Show(string msg) => Console.WriteLine(msg.ToUpper());
        static void Main(string[] args)
        {
            Programv p = new Programv();
            p.Print += new PrintDetail(p.Show);
            p.Print("Hello World"); 
            Console.ReadLine();
        }
}
