using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeide4
{
    class acb
    {
        public delegate void MyDele(int value);

        public static void Main(string[] args)
        {
            MyDele printtValue = delegate (int val)
            {
                Console.WriteLine("Inside anonymous method, value: {0}", val);
            };
            printtValue += delegate
            {
                Console.WriteLine("this is Anonymous Method");
            };
            printtValue(100);
        }
    }
}
