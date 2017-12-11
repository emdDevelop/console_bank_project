using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace BankAccounts
{
    public class Account
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("transaction_date")]
        public DateTime Transaction_date { get; set; }
        [Column("amount")]
        public decimal Amount { get; set; }
        [Column("user_id")]
        [ForeignKey("user_id")]
        public User User_id { get; set; }

        //public virtual User User { get; set; }

        //methods
        //Every class or struct in C# implicitly inherits the Object class.
        //Therefore,every object in C# gets the ToString  method, which returns 
        //a string representation of that object.
        public override string ToString()
        {
            var amountLocal=Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"));
            var dateFormat=Transaction_date.ToString("yyyy-MM-dd HH:mm:ss.FFF", CultureInfo.CreateSpecificCulture("el-GR"));
            return $"Transaction Date : {dateFormat} Amount: {amountLocal} ";
        }
    }
}
