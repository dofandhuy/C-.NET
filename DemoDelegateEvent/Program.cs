namespace DemoEvent
{
    public delegate void myEvent(int x, int y);
    class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private int _amount;

        public event myEvent AmountChanged;
        public int Amount
        {
            get => _amount;
            set
            {
                int old = _amount;
                _amount = value;
                OnChangedAmount(old, _amount);
            }
        }
        private void OnChangedAmount(int oldValue, int newValue)
        {
            if (AmountChanged != null)
            {
                AmountChanged.Invoke(oldValue, newValue);
            }
        }
    }
    class Using
    {
        public static void Main()
        {
            Account acc = new Account() { Id = 1, Name = "NGuyen Van A" };
            acc.Amount = 10;
            acc.Amount = 20;
            acc.AmountChanged += Acc_AmountChanged;
            acc.Amount = 30;
            acc.Amount = 40;
            acc.AmountChanged -= Acc_AmountChanged;
            acc.Amount = 50;
            acc.Amount = 60;
        }

        private static void Acc_AmountChanged(int x, int y)
        {
            System.Console.WriteLine("Amount changed from {0} to {1}", x, y);
        }
    }
}

//namespace DemoEvent
//{

//    class Account
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }

//        private int _amount;

//        private event EventHandler<myEventArgs> amountChanged;
//        public event EventHandler<myEventArgs> AmountChanged
//        {
//            add
//            {
//                amountChanged += value;
//            }
//            remove
//            {
//                amountChanged -= value;
//            }
//        }


//        public int Amount
//        {
//            get => _amount;
//            set
//            {
//                int old = _amount;
//                _amount = value;
//                OnChangedAmount(old, _amount);
//            }
//        }
//        private void OnChangedAmount(int oldValue, int newValue)
//        {
//            if (amountChanged != null)
//            {
//                amountChanged.Invoke(null, new myEventArgs(oldValue, newValue));
//            }
//        }
//    }
//    class myEventArgs : EventArgs
//    {
//        public int OldValue { get; set; }
//        public int NewValue { get; set; }
//        public myEventArgs()
//        {
//        }
//        public myEventArgs( int oldValue, int newValue)
//        {
//            OldValue = oldValue;
//            NewValue = newValue;
//        }
//    }
//    class Using
//    {
//        public static void Main()
//        {
//            Account acc = new Account() { Id = 1, Name = "NGuyen Van A" };
//            acc.Amount = 10;
//            acc.Amount = 20;
//           acc.AmountChanged += Acc_AmountChanged;
//            acc.Amount = 30;
//            acc.Amount = 40;
//            acc.AmountChanged -= Acc_AmountChanged;
//            acc.Amount = 50;
//            acc.Amount = 60;

//        }

//        private static void Acc_AmountChanged(object? sender, myEventArgs e)
//        {
//            Console.WriteLine($"So du thay doi tu {e.OldValue} sang {e.NewValue}"); 
//        }
//    }
//}