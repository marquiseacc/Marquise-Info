using Marquise_Web.Data;
using Marquise_Web.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageRepository me = new MessageRepository();
            me.Add(new Message()
            {
                Name = "Sadaf",
                Phonenumber = "09333801631",
                Email = "sdffbabaei@gmail.com",
                MessageText = "hello world",
                RegisterDate = DateTime.Now
            });
        }
    }
}
