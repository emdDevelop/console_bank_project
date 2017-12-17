using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankAccounts
{
    class Login
    {
        //properties
        private static int wrongCounter = 1;
        //methods
        public static void LoginInput()
        {
            string userPass = "";
            int counter = 1;
            Console.Clear();
            Console.SetCursorPosition(30, 10);
            Console.Write("Enter login name: ");
            string userName = Console.ReadLine();   //input of username
            Console.SetCursorPosition(30, 11);
            Console.Write("Enter you password: ");
            //Mask the password with * symbol
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    //get cursor position
                    var curPos = Console.CursorLeft;
                    if (curPos < 50)
                    {
                        //if after repeat click of backspace we reach the start of string
                        //then we freeze position at begining position 
                        Console.SetCursorPosition(50, 11);
                        //Set pass string to initial value with nothing inside
                        userPass = "";
                        key = Console.ReadKey();
                    }
                    else
                    {
                        Console.SetCursorPosition(curPos, 11);
                        Console.Write(" ");
                        Console.SetCursorPosition(curPos, 11);
                        counter--;
                        userPass = userPass.Substring(0, (userPass.Length - 1));
                        key = Console.ReadKey();
                    }
                }
                else
                {
                    Console.SetCursorPosition(50, 11);
                    for (int i = 0; i < counter; i++)
                    {
                        Console.Write("*");
                    }
                    userPass += (key.KeyChar);
                    counter++;
                    key = Console.ReadKey();
                }
            }
            //End of masking
            CheckUser(userName, userPass);
        }

        public static void CheckUser(string user, string pass)
        {
            Console.SetCursorPosition(30, 13);
            Console.Write("Waiting.....");
            if (wrongCounter != 3)
            {
                using (Db ctx = new Db())
                {
                    try
                    {
                        //check database if user with parameter user exists in database
                        //it throws an exception if no element NULL or more tha one element exists
                        //So we use try catch to handle the exception
                        var userExist = ctx.Users.Single(i => i.Username == user);

                        //cheking password for admin
                        if (userExist.Username == "admin" && BCrypt.Net.BCrypt.Verify(pass, userExist.Password))
                        {
                            //create the admin user
                            User admin = new User(userExist.Id, userExist.Username, userExist.Password);
                            AppMenu.SwitchSuperAdmin(admin);
                        }
                        //checking password for simple users
                        
                        else if ((userExist.Username == "user1" && BCrypt.Net.BCrypt.Verify(pass, userExist.Password)) ||
                                 (userExist.Username == "user2" && BCrypt.Net.BCrypt.Verify(pass, userExist.Password)))
                        {
                            //create a user so as to know if its user1 or user2
                            User simpleUser = new User(userExist.Id, userExist.Username, userExist.Password);
                            AppMenu.SwitchSimpleUser(simpleUser);
                        }
                        else
                        {
                            Console.Clear();
                            Console.SetCursorPosition(30, 8);
                            Console.Write("Wrong password!");
                            Console.SetCursorPosition(30, 9);
                            Console.Write($"You have {3 - wrongCounter} more times!!");
                            wrongCounter++;
                            Console.SetCursorPosition(30, 11);
                            Console.Write("Press any key to try again..");
                            Console.ReadKey();
                            Console.Clear();
                            LoginInput();
                        }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.SetCursorPosition(30, 8);
                        Console.Write("User not Found!");
                        Console.SetCursorPosition(30, 9);
                        Console.Write($"You have {3 - wrongCounter} more times!!");
                        wrongCounter++;
                        Console.SetCursorPosition(30, 11);
                        Console.Write("Press any key to try again");
                        Console.ReadKey();
                        LoginInput();
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.SetCursorPosition(30, 8);
                Console.Write("Please contact system administrator");
                Console.SetCursorPosition(30, 10);
                Console.Write("Press any key to exit");
                return;
            }
        }
    }
}
