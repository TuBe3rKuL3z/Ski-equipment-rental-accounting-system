using System;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace Ski_equipment_rental_accounting_system
{
    public class DataBase
    {
        static string connStr = "Messenger.db";
     

        public static void CreateTableCahier()
        {
            SqliteConnection conn = new SqliteConnection();
            
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteConnection().CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE сahier (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Password TEXT NOT NULL);";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица Кассир создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Кассира ошибка: {ex}");
            }
        }

        public static void CreateTableClient()
        {
            SqliteConnection conn = new SqliteConnection();

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteConnection().CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE сlient (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, SecondtName TEXT NOT NULL, Document TEXT NOT NULL, PhoneNumber TEXT);";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица Клиента создана!");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Таблица Клиента ошибка: {ex}");
            }
        }
        public static void CreateTableRental()
        {
            SqliteConnection conn = new SqliteConnection();

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteConnection().CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE rental (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, DateStart TEXT NOT NULL, DateEnd TEXT NOT NULL, TotalPrice DOUBLE NOT NULL, Status TEXT NOT NULL);";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица Аренды создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Аренды ошибка: {ex}");
            }
        }

        public static void CreateTableEquipment()
        {
            SqliteConnection conn = new SqliteConnection();

            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteConnection().CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"CREATE TABLE equipment (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Inv_numbery TEXT NOT NULL, Type TEXT NOT NULL, Size TEXT NOT NULL, Brand TEXT NOT NULL, Model TEXT NOT NULL, Image BLOB, Status Text NOT NULL);";
                cmd.ExecuteNonQuery();

                MessageBox.Show("Таблица Оборудования создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Оборудования ошибка: {ex}");
            }
        }
    }

    
}
