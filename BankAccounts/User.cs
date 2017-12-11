using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankAccounts
{
    public class User
    {
        //Properties
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }

        //public virtual Account Account { get; set; }

        public User()
        {

        }

        public User(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
