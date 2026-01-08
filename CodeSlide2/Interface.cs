using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CodeSlide2
{   

    // OOP - interface
   //public interface IFrist
   // {
   //     void Print();
   //     void Display();

   // }
   // public interface ISecond
   // {
   //     void Print();
   // }
   // public class MyClasss : IFrist, ISecond {
   //     public void Display()
   //     {
   //         Console.WriteLine("Display method:");
   //     }
   //     void IFrist.Print()
   //     {
   //         Console.WriteLine("IFrist's Print method");
   //     }
   //     void ISecond.Print()
   //     {
   //         Console.WriteLine("ISecond's Print method");
   //     }
   // }
   // class Programs
   // {
   //     static void Main(string[] args)
   //     {
   //         MyClasss obj= new MyClasss();
   //         obj.Display();
   //         IFrist first = obj;
   //         first.Print();
   //         ISecond second = obj;
   //         second.Print();
   //         Console.ReadLine();
   //     }
   // }

    // interface inheritance
    //public interface ICar
    //{
    //    void Drive();
    //}

    //public interface IUnderwaterCar
    //{
    //    void Dive();
    //}

    //public interface IJamebondCar: ICar, IUnderwaterCar
    //{
    //    void TurboBoost();
    //}
    //public class MyClassss: IJamebondCar
    //{
    //    public void TurboBoost()
    //    {

    //    }
    //    public void Dive()
    //    {
    //    }
    //    public void Drive() 
    //    { 
    //    }
    //}

    // default interface methods
     public interface ISample
    {
        static void Print()
        {
            Console.WriteLine("Static method");
        }
        string GetString(string s)
        {
            return "Hello " + s;
        }

    }
    interface ISample01
    {
        void MyMethod()
        {
            Console.WriteLine("ISample01.MyMethod");
        }

    }
    interface ISample02: ISample01
    {
        void ISample01.MyMethod()
        {
            Console.WriteLine("ISample02.MyMethod");
        }
    }
}
