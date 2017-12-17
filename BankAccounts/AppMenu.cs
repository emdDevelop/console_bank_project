using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccounts
{
    class AppMenu
    {
        //Methods
        public static void SuperAdminAccount(User user)
        {
            int counter = 8;
            string[] superMenu =
            {
                " 1.View internal bank account\n",
                " 2.View Members bank accounts\n",
                " 3.Deposit to Members bank account\n",
                " 4.Withdraw from Members bank account\n",
                " 5.Send to the statement file todays transactions\n",
                " 6.Exit the application\n",
                "Please select a number from 1 to 6 : "
            };
            Console.Clear();
            Console.SetCursorPosition(30, 6);
            Console.Write($"Welcome {user.Username}");
            foreach (var item in superMenu)
            {
                Console.SetCursorPosition(30, counter);
                Console.Write(item);
                counter++;
            }
        }

        public static void SimpleMemberAccount(User user)
        {
            int counter = 8;
            string[] simpleMenu =
            {
                " 1.View bank account\n",
                " 2.Deposit to Super Admin account\n",
                " 3.Deposit to another Members bank account\n",
                " 4.Send to the statement file todays transactions\n",
                " 5.Exit the application\n",
                "Please select a number from 1 to 5 : "
            };
            Console.Clear();
            Console.SetCursorPosition(30, 6);
            Console.Write($"Welcome {user.Username}");
            foreach (var item in simpleMenu)
            {
                Console.SetCursorPosition(30, counter);
                Console.Write(item);
                counter++;
            }
        }

        public static void SwitchSimpleUser(User user)
        {
            int choice = 0;
            do
            {
                SimpleMemberAccount(user);

                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}");
                        Console.SetCursorPosition(30, 8);
                        Console.Write(" 1.View bank account\n");
                        InternalOperations.DisplayBalance(user);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}");
                        Console.SetCursorPosition(30, 8);
                        Console.Write(" 2.Deposit to Super Admin account");
                        InternalOperations.DepositToAdminAccount(user);
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}");
                        Console.SetCursorPosition(30, 8);
                        Console.Write(" 3.Deposit to another Members bank account\n");
                        InternalOperations.DepositToAccount(user);
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}\n\n");
                        Console.Write(" 4.Send to the statement file todays transactions\n");
                        InternalOperations.SendStatement(user);
                        Console.WriteLine("\nPress any key to return to menu");
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.Clear();
                        InternalOperations.CheckFileSendOnExit(user);
                        Console.SetCursorPosition(30, 8);
                        Console.Write("Press any key to Quit");
                        break;
                    default:
                        Console.Clear();
                        Console.SetCursorPosition(30, 8);
                        Console.WriteLine("Please enter a number between 1-5");
                        Console.SetCursorPosition(30, 9);
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                }
            } while (choice != 5);
        }

        public static void SwitchSuperAdmin(User user)
        {
            int choice = 0;
            bool continueAction= true;
            do
            {
                SuperAdminAccount(user);

                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}");
                        InternalOperations.DisplayBalance(user);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}");
                        Console.SetCursorPosition(30, 8);
                        Console.Write(" 2.View Members bank accounts");
                        Console.SetCursorPosition(30, 10);
                        Console.Write("Users List");
                        InternalOperations.DisplayUserBalance();
                        Console.ReadKey();
                        break;
                    case 3:
                        do
                        {
                            Console.Clear();
                            Console.Write($"Login User: {user.Username}\n\n");
                            Console.SetCursorPosition(30, 8);
                            Console.Write(" 3.Deposit to Members bank account\n");
                            InternalOperations.DepositToAccount(user);
                            ConsoleKeyInfo key = Console.ReadKey();
                            if (key.Key == ConsoleKey.Y)
                            {
                                Console.Clear();
                                continueAction = true;
                            }
                            else
                            {
                                continueAction = false;
                            }
                        } while (continueAction);
                        break;
                    case 4:
                        do
                        {
                            Console.Clear();
                            Console.Write($"Login User: {user.Username}\n\n");
                            Console.SetCursorPosition(30, 8);
                            Console.Write(" 4.Withdraw from Members bank account\n");
                            InternalOperations.Withdraw();
                            ConsoleKeyInfo key = Console.ReadKey();
                            if (key.Key == ConsoleKey.Y)
                            {
                                Console.Clear();
                                continueAction = true;
                            }
                            else
                            {
                                continueAction = false;
                            }
                        } while (continueAction);
                        break;
                    case 5:
                        Console.Clear();
                        Console.Write($"Login User: {user.Username}\n\n");
                        Console.Write(" 5.Send to the statement file todays transactions\n");
                        InternalOperations.SendStatement(user);
                        Console.WriteLine("\nPress any key to return to menu");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.Clear();
                        InternalOperations.CheckFileSendOnExit(user);
                        Console.SetCursorPosition(30, 8);
                        Console.Write("Press any key to Quit");
                        break;
                    default:
                        Console.Clear();
                        Console.SetCursorPosition(30, 8);
                        Console.WriteLine("Please enter a number between 1-6");
                        Console.SetCursorPosition(30, 9);
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                }
            } while (choice != 6);
        }
    }
}
