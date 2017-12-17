using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BankAccounts
{
    class FileAccess
    {
        //properties
        private readonly string path;
        private readonly DateTime date;
        public User CurrentUser { get; set; }
        //Constructor
        public FileAccess(User currentUser)
        {
            date = DateTime.Now;
            path = $"statement_{currentUser.Username}_{date.Day}_{date.Month}_{date.Year}.txt";
        }
        //Methods
        public void CreateUserFile(List<string> transaction)
        {
            //If list with transactions is empty then no need to create empty file
            if (transaction.Count == 0)
            {
                Console.WriteLine("No need to create File.No Transactions had made");
            }
            //if file path is not exist then create the today file statment
            else if (!File.Exists(path))
            {
                //Create a file to write to
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var item in transaction)
                    {
                        //override ToString() Method 
                        sw.WriteLine(item.ToString());
                        Console.WriteLine(item.ToString());
                    }
                }
            }
            else //if exists then append to file
            {
                //Append to file to write to
                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach (var item in transaction)
                    {
                        sw.WriteLine(item.ToString());
                        Console.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}
