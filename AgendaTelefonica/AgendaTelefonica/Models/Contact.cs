using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AgendaTelefonica.Models
{
   public class Contact
   {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }

        [MaxLength(250), Unique]
        public string firstName { get; set; }

        [MaxLength(250), Unique]
        public string secondName { get; set; }

        [MaxLength(250), Unique]
        public string phoneNumber { get; set; }

        [MaxLength(250), Unique]
        public string email { get; set; }

        public bool favorite { get; set; }

        public byte[] profilPicture { get; set; }
    

        public  SQLiteConnection getConnection()
        {
            return new SQLiteConnection(App.DataBaseLocation);
        }

    }


}
