using System;
using Microsoft.Data.Sqlite;

namespace Ski_equipment_rental_accounting_system
{
    private class DataBase
    {
        static string connStr = "Messenger.db";
     

        private static void CreateDb()
        {
            SqliteConnection conn = new SqliteConnection();
            

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteConnection().CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE user ();"
            }
        }

    }

    
}
