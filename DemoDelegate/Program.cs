delegate int myDelegate(int x, int y);  // định nghĩa 
class Calculator
{
    public int Add(int x, int y)
    {
        Console.WriteLine("ban da  goi den phap cong");
       return x + y;
    }
    public int Sub(int x, int y)
    {
        Console.WriteLine("ban da doi den phep tru");
        return x - y;
    }

    public static void Main(string[] args)
    {
        int x = 10, y = 3;
        Calculator c = new Calculator();
        // myDelegate my = new myDelegate(c.Add); // khai báo
        myDelegate my;
        my = c.Sub;
        my += c.Add;
       
        //===================================================
        //Console.WriteLine($"tổng hai số  {my.Invoke(x, y)}");
        //my -= c.Sub;
        //my(x, y);
    }
}

//using System.Runtime.CompilerServices;

//delegate string myDelegate(string s);
//class MyString {
//    public string ToUpper(String s)
//    {
//        return s.ToUpper(); 
//    }
//    public string ToLower(String s)
//    {
//        return s.ToLower();
//    }

//}
//class Program
//{ 
//    public void CallDelegatem(myDelegate my, string msg)
//    {
//        Console.WriteLine($"{my.Invoke(msg)}");
//    }
//    public static void Main(string[] args)
//    {
//        Program p= new Program();
//        MyString my= new MyString();
//        Console.WriteLine("input string s: ");
//        string msg = Console.ReadLine();
//        Console.WriteLine("String to Upper: ");
//        p.CallDelegatem(my.ToUpper,msg);
//        Console.WriteLine("String to lower: ");
//        p.CallDelegatem(my.ToLower, msg);
//    }
//}