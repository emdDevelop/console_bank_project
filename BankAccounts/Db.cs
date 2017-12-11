using Microsoft.EntityFrameworkCore;
//Entity framework is an Object/Relational Mapping (O/RM) framework
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankAccounts
{
    //DbContext is an important part of Entity Framework.
    //It is a bridge between our domain or entity classes and the database.
    public class Db:DbContext
    {
        //Entity set
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server= stathis\\sqlexpress; Database= afdemp_csharp_1;" +
            "Integrated Security = true;");
        }

        public static void GetInfoUser()
        {
            using (Db ctx = new Db())
            {
                var info=ctx.Users.ToArray();
            }
        }
        //public static void ExecuteQuery(string query)
        //{
          
        //}

        //public static void ShowTable(SqlDataReader data)
        //{
        //    int count = 1;
        //    while (data.Read())
        //    {
        //        //Console.Write($"{count} ");
        //        for (int i = 0; i < data.FieldCount; i++)
        //        {
        //            Console.Write($"{data[i].ToString(),-10}\t");
        //        }
        //        Console.WriteLine();
        //        count++;
        //    }
        //}
    }
}
