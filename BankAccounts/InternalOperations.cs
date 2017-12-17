using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BankAccounts
{
    class InternalOperations
    {
        //Properties 
        //List with Today Transactions
        private static List<string> statementList;
        private static bool checkFileSend = false;

        //Static constructor to initiate List
        static InternalOperations()
        {
            statementList = new List<string>();
        }

        //Methods
        //This method is only for admin .The programm creates a list with all users and the 
        //admin select the user to show his account
        public static void DisplayUserBalance()
        {
            int flag = 1;
            while (flag == 1)
            {
                int counter = 11;//for cursor position
                using (Db ctx = new Db())
                {
                    //Show all users in bank
                    Console.SetCursorPosition(30, counter++);
                    Console.Write($"Id\tUser");
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("--------------------------------");
                    var usersList = ctx.Users.ToList();
                    foreach (var user in usersList)
                    {
                        if (user.Username == "admin")
                        {
                            continue;
                        }
                        Console.SetCursorPosition(30, counter);
                        Console.Write($"{user.Id}\t\t{user.Username}");
                        counter++;//on every loop cursor position Y goes to next line
                    }
                    //the admin is informed to select the user
                    counter++;
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("Select User By Id Number: ");
                    //Pass only numbers
                    try
                    {
                        int userId = int.Parse(Console.ReadLine());
                        var userAcount = ctx.Accounts.Single(i => i.User_id.Id == userId);
                        Console.SetCursorPosition(30, counter++);
                        Console.Write($"User {userAcount.User_id.Username} Account Balance is: {userAcount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");

                        counter++;
                        Console.SetCursorPosition(30, counter++);
                        Console.Write("Press any key to go back to menu...");
                        flag = 0;
                    }
                    catch
                    {
                        counter = 8;
                        CatchErrorNotNumber(counter);
                    }
                }
            }
        }

        //method for both Admin and simple user to see his balance
        public static void DisplayBalance(User user)
        {
            using (Db ctx = new Db())
            {
                //var usersList = ctx.Users.ToList();
                var userAccount = ctx.Accounts.Single(i => i.User_id.Id == user.Id);
                Console.SetCursorPosition(30, 10);
                Console.Write($"Account Balance is: {userAccount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
            }
        }

        //Both for Admin and Simple users deposit to another member account
        public static void DepositToAccount(User currentUser)
        {
            int flag = 1;
            int counter = 10;
            while (flag == 1)
            {
                using (Db ctx = new Db())
                {
                    //Show all users in bank except current user
                    var usersList = ctx.Users.ToList();
                    Console.SetCursorPosition(30, counter++);
                    Console.Write($"Id\tUser");
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("--------------------------------");
                    foreach (var user in usersList)
                    {
                        if (user.Id != currentUser.Id && user.Username!="admin")
                        {
                            Console.SetCursorPosition(30, counter);
                            Console.Write($"{user.Id}\t\t{user.Username}");
                            counter++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //from the above menu the user must select a user 
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("Select User By Id Number: ");
                    try
                    {
                        //User to make the deposit
                        var fromUser = ctx.Accounts.Single(i => i.User_id.Id == currentUser.Id);
                        int userId = int.Parse(Console.ReadLine());
                        //User who accepts the deposit
                        var userAcount = ctx.Accounts.Single(i => i.User_id.Id == userId);
                        Console.SetCursorPosition(30, counter++);
                        Console.Write($"Your Account has {fromUser.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}\n");
                        //inform user to enter the deposit amount 
                        Console.SetCursorPosition(30, counter++);
                        Console.Write($"Enter Amount to deposit to {userAcount.User_id.Username}: ");
                        decimal depositAmount = 0;
                        if(Decimal.TryParse(Console.ReadLine().Replace('.', ','), out depositAmount))
                        {
                            //check deposit amount not to be zero and if its smaller or equal
                            //from current account balance
                            if (depositAmount > 0 && depositAmount <= fromUser.Amount)
                            {
                                fromUser.Amount -= depositAmount;
                                userAcount.Amount += depositAmount;
                                userAcount.Transaction_date = DateTime.Now;
                                //update database
                                ctx.Accounts.Update(fromUser);
                                ctx.Accounts.Update(userAcount);
                                //save to database
                                ctx.SaveChanges();
                                //Add deposit to admin today transaction list
                                userAcount.Amount = depositAmount;
                                statementList.Add("Deposit " + userAcount.ToString() + "To User: " + userAcount.User_id.Username);
                                //Stop while loop
                                flag = 0;
                                Console.SetCursorPosition(10, counter++);
                                //Only admin can make multiple deposits
                                if (fromUser.User_id.Username == "admin")
                                {
                                    Console.Write("Make another Deposit? Press Y for yes or any key to return to menu ");
                                }
                                else
                                {
                                    Console.SetCursorPosition(30, counter++);
                                    Console.Write("Press any key to continue..");
                                }
                            }
                            else
                            {
                                //less than zero bigger from current account balance
                                flag = 1;
                                counter = 8;
                                Console.Clear();
                                Console.SetCursorPosition(20, counter++);
                                Console.Write($"Your deposit must be between 0 and {fromUser.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
                                Console.SetCursorPosition(30, counter++);
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        else
                        {
                            //Not decimal
                            flag = 1;
                            counter = 8;
                            CatchErrorNotNumber(counter);
                        }
                    }
                    catch
                    {
                        //not number
                        counter = 8;
                        CatchErrorNotNumber(counter);
                    }
                }
            }
        }

        //For Simple users
        public static void DepositToAdminAccount(User user)
        {
            int flag = 1;//This flag stops the while loop
            int counter = 10;
            using (Db ctx = new Db())
            {
                var adminAccount = ctx.Accounts.Single(i => i.User_id.Username == "admin");
                var userAccount = ctx.Accounts.Single(i => i.User_id.Id == user.Id);
                while (flag == 1)
                {
                    Console.SetCursorPosition(30, counter++);
                    Console.Write($"Your Account has {userAccount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("Enter Amount to deposit : ");
                    try
                    {
                        decimal depositAmount = 0;
                        //Prompt the user to enter amount for deposit
                        if (decimal.TryParse(Console.ReadLine().Replace('.', ','), out depositAmount))
                        {
                            //Check if deposit amount is bigger than zero and smaller than user balance
                            if (depositAmount > 0 && depositAmount <= userAccount.Amount)
                            {
                                //Add this amount to admin account
                                adminAccount.Amount += depositAmount;
                                adminAccount.Transaction_date = DateTime.Now;
                                //Substract this amount from user account
                                userAccount.Amount -= depositAmount;
                                userAccount.Transaction_date = DateTime.Now;
                                //Update Database
                                ctx.Accounts.Update(adminAccount);
                                ctx.Accounts.Update(userAccount);
                                //save to database
                                ctx.SaveChanges();
                                //Add deposit to simple user today transaction list
                                adminAccount.Amount = depositAmount;
                                //userAccount.User_id = user;
                                statementList.Add("Deposit to Admin " + adminAccount.ToString());
                                flag = 0;
                                Console.SetCursorPosition(30, counter++);
                                Console.WriteLine("Press any key to return to menu..");
                            }
                            else
                            {
                                //less than zero bigger from current account balance
                                flag = 1;
                                counter = 8;
                                Console.Clear();
                                Console.SetCursorPosition(20, counter++);
                                Console.Write($"Your deposit must be between 0 and {userAccount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
                                Console.SetCursorPosition(30, counter++);
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        }
                        else
                        {
                            //Not decimal
                            flag = 1;
                            counter = 8;
                            CatchErrorNotNumber(counter);
                        }
                    }
                    catch
                    {
                        //Not Number
                        counter = 8;
                        CatchErrorNotNumber(counter);
                    }
                }
            }
        }

        //This method is only for Admin
        public static void Withdraw()
        {
            int flag = 1;
            int counter = 10;
            while (flag == 1)
            {
                //Instantiating DbContext:
                using (Db ctx = new Db())
                {
                    var usersList = ctx.Users.ToList();
                    Console.SetCursorPosition(30, counter++);
                    Console.Write($"Id\tUser");
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("--------------------------------");
                    foreach (var user in usersList)
                    {
                        if (user.Username == "admin")
                        {
                            continue;
                        }
                        Console.SetCursorPosition(30, counter);
                        Console.WriteLine($"{user.Id}\t\t{user.Username}");
                        counter++;
                    }
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("Select User by Id Number: ");
                    try
                    {
                        var adminAccount = ctx.Accounts.Single(i => i.User_id.Username == "admin");
                        int userId = int.Parse(Console.ReadLine());
                        var userAcount = ctx.Accounts.Single(i => i.User_id.Id == userId);
                        //show Info (name and current account balance) about the user from 
                        //whom to withdraw money
                        Console.SetCursorPosition(30, counter++);
                        Console.Write($"User to withdraw is {userAcount.User_id.Username} with account Balance {userAcount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
                        //Inform the user to enter amount for withdraw
                        Console.SetCursorPosition(30, counter++);
                        Console.Write("Enter Amount to Withdraw : ");
                        //CHECK if string is enter and continue accordingly
                        decimal withdrawAmount = 0;
                        if (Decimal.TryParse(Console.ReadLine().Replace('.', ','), out withdrawAmount))
                        {
                            if(withdrawAmount>0 && withdrawAmount <= userAcount.Amount)
                            {
                                //Withdraw amount is between valid amounts
                                userAcount.Amount -= withdrawAmount;
                                userAcount.Transaction_date = DateTime.Now;
                                adminAccount.Amount += withdrawAmount;
                                adminAccount.Transaction_date = DateTime.Now;
                                //Update database
                                ctx.Accounts.Update(userAcount);
                                ctx.Accounts.Update(adminAccount);
                                //save to database
                                ctx.SaveChanges();
                                //Add deposit to admin today transaction list
                                //Note change the list to add object Account and from Account to print 
                                //the message to file with ToString() method
                                userAcount.Amount = withdrawAmount;
                                statementList.Add("Withdraw " + userAcount.ToString() + "From User: " + userAcount.User_id.Username);
                                flag = 0;
                                Console.SetCursorPosition(10, counter++);
                                Console.Write("Make another Withdraw? Press Y for yes or any key to return to menu ");
                            }
                            else
                            {
                                //Withdraw amount is zero or bigger from current balance
                                flag = 1;
                                counter = 8;
                                Console.Clear();
                                Console.SetCursorPosition(20, counter++);
                                Console.Write($"Your Withdraw amount must be between 0 and {userAcount.Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))}");
                                Console.SetCursorPosition(30, counter++);
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                            } 
                        }
                        else
                        {
                            //Not decimal
                            flag = 1;
                            counter = 8;
                            CatchErrorNotNumber(counter);
                        }
                    }
                    catch
                    {
                        //Not number
                        counter = 8;
                        CatchErrorNotNumber(counter);
                    }
                }
            }
        }

        public static void SendStatement(User user)
        {
            //Create file with Today Transactions list
            FileAccess f = new FileAccess(user);
            f.CreateUserFile(statementList);
            checkFileSend = true;
        }

        public static void CheckFileSendOnExit(User user)
        {
            //if filestatement exist then no need to create one
            if (checkFileSend)
            {
                Console.WriteLine("Check if File statement send...  OK!");
            }
            else  //if filestatement dont exist then send statement
            {
                Console.Write("Sending File Statement\n");
                SendStatement(user);
            }
        }

        public static void CatchErrorNotNumber(int counter)
        {
            Console.Clear();
            Console.SetCursorPosition(30, counter++);
            Console.WriteLine("Please enter only numbers");
            Console.SetCursorPosition(30, counter++);
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
