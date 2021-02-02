using System;
using System.Collections.Generic;
using System.Text;
using SQLite;


namespace AgendaTelefonica.Models
{
   public class HistoryElem
    {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }

        public int id_Contact { set; get; }

        public DateTime date { set; get; }

        public string phoneNumber { set; get; }



        public SQLiteConnection getConnection()
        {
            return new SQLiteConnection(App.DataBaseLocation);
        }
    }
}
