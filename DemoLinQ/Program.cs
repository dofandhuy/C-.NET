namespace DemoLinQ
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] name = {"DAvid", "Alex", "Bob", "Charlie", "Eve" };
            foreach(string item in name.OrderBy(s => s))
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
        
    }
}