namespace abc
{ 
    
     static class UtilityClass
        {
            public static int Add(this int x, int y)
            {
                return x + y;
            }
        }
    class USing
    {
       
        public static void Main()
        {
            int x = 10, y = 20;
            
            int z= x.Add(y);
        }
    }
}