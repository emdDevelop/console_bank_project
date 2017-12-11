using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BankAccounts
{
    class Program
    {
    
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Login.LoginInput();
            Console.ReadKey();
        }
    }
}
