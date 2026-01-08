namespace abc
{
    class MyClass
    {
        public static int x = 1;
        static MyClass()
        {
            x = 2;
            Console.WriteLine($"Static constructor: {x}");

        }
        public MyClass()
        {
            x++;
            Console.WriteLine("Normal Constructor x={0}",x);
        }
    }
    class Program
    {
        public static void Main()
        {
            MyClass m1= new MyClass();
            MyClass.x = 4;
            MyClass m2= new MyClass();
        }
    }
}
