using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{
    class PRogram
    {
        public int Add(int x, int y) => x + y;

        public string Name => FirstName + " " + LastName;


        public class Location
        {
            private string locationName;

            public Location(string name) => Name = name;

            public string Name
            {
                get => locationName;
                set => locationName = value;
            }
        }
    }
}
