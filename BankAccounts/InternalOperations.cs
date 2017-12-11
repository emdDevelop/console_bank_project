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
            while(flag==1)
            {
                int counter = 11;//for cursor position
                using (Db ctx = new Db())
                {
                    //Show all users in bank
                    var usersList = ctx.Users.ToList();
                    foreach (var user in usersList)
                    {
                        Console.SetCursorPosition(30, counter);
                        Console.Write($"{user.Id}.{user.Username}");
                        counter++;//on every loop cursor position Y goes to next line
                    }
                    //the admin is informed to select the user
                    counter++;
                    Console.SetCursorPosition(30, counter++);
                    Console.Write("Select User : ");
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
                        Console.Clear();
                        Console.WriteLine("Please enter only numbers");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        //method for both Admin and simple user to see his balance
        public static void DisplayBalance(User user)
        {
            using (Db ctx = new Db())
            {
               // var usersList = ctx.Users.ToList();
                var userAccount = ctx.Accounts.Single(i => i.User_id.Id == user.Id);
                Console.SetCursorPosition(30, 10);
                Console.Write($"Account Balance is: {userAccount.Amount}");
            }
        }

        //Both for Admin and Simple users deposit to another member account
        public static void DepositToAccount(User currentUser)
        {
            int flag = 1;
            while (flag == 1)
            {
                using (Db ctx = new Db())
                {
                    //Show all users in bank
                    var usersList = ctx.Users.ToList();
                    foreach (var user in usersList)
                    {
                        Console.WriteLine($"{user.Id}.{user.Username}");
                    }
                    //from the above menu the user must select a user 
                    Console.Write("\nSelect User : ");
                    try
                    {
                        //User to make the deposit
                        var fromUser = ctx.Accounts.Single(i => i.User_id.Id == currentUser.Id);
                        int userId = int.Parse(Console.ReadLine());
                        //User who accept the deposit
                        var userAcount = ctx.Accounts.Single(i => i.User_id.Id == userId);
                        //inform user to enter the deposit amount 
                        Console.Write("\nEnter Amount to deposit : ");
                        int depositAmount = int.Parse(Console.ReadLine());
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
                        flag = 0;
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter only numbers");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        //Both for Admin and Simple users
        public static void DepositToMyAccount(User user)
        {
            int flag = 1;
            using (Db ctx = new Db())
            {
                var userAccount = ctx.Accounts.Single(i => i.User_id.Id == user.Id);
                while (flag == 1)
                {
                    Console.Write($"Your Account has {userAccount.Amount}\n");
                    Console.Write("Enter Amount to deposit : ");
                    try
                    {
                        int depositAmount = int.Parse(Console.ReadLine());
                        userAccount.Amount += depositAmount;
                        userAccount.Transaction_date = DateTime.Now;
                        //Update Database
                        ctx.Accounts.Update(userAccount);
                        ctx.SaveChanges();//save to database
                        //Add deposit to simple user today transaction list
                        userAccount.Amount = depositAmount;
                        //userAccount.User_id = user;
                        statementList.Add("Deposit " + userAccount.ToString());
                        flag = 0;
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter only numbers");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        //This method is only for Admin
        public static void Withdraw()
        {
            int flag = 1;
            while (flag == 1)
            {
                //Instantiating DbContext:
                using (Db ctx = new Db())
                {
                    var usersList = ctx.Users.ToList();
                    foreach (var user in usersList)
                    {
                        Console.WriteLine($"{user.Id}.{user.Username}");
                    }
                    Console.Write("\nSelect User : ");
                    try
                    {
                        int userId = int.Parse(Console.ReadLine());
                        var userAcount = ctx.Accounts.Single(i => i.User_id.Id == userId);
                        //show Info (name and current account balance) about the user from 
                        //whom to withdraw money
                        Console.WriteLine($"User to withdraw is {usersList[userId - 1].Username} Balance {userAcount.Amount}");
                        //Inform the user to enter amount for withdraw
                        Console.Write("Enter Amount to Withdraw : ");
                        //CHECK if string is enter and continue accordingly
                        int withdrawAmount = int.Parse(Console.ReadLine());
                        userAcount.Amount -= withdrawAmount;
                        userAcount.Transaction_date = DateTime.Now;
                        //Update database
                        ctx.Accounts.Update(userAcount);
                        //save to database
                        ctx.SaveChanges();
                        //Add deposit to admin today transaction list
                        //Note change the list to add object Account and from Account to print 
                        //the message to file with ToString() method
                        userAcount.Amount = withdrawAmount;
                        statementList.Add("Withdraw " + userAcount.ToString() +"From User: "+userAcount.User_id.Username);
                        flag = 0;
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("Please enter only numbers");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
        }

        public static void SendStatement(User user)
        {
            //Create file with Today Transactions list
            FileAccess f = new FileAccess(user);
            f.CreateUserFile(statementList);
        }
    }
}
