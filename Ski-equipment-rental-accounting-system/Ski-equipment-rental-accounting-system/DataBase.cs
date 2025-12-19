using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Text;
using System.Windows;

namespace Ski_equipment_rental_accounting_system
{
    public class DataBase
    {
        public const string connStr = "RentalDB.db";

        //stringSelect
        public const string stringSelectCashier = "SELECT * FROM Cashier";
        public const string stringSelectClient = "SELECT * FROM Client";
        public const string stringSelectRental = "SELECT * FROM Rental";
        public const string stringSelectEquipment = "SELECT * FROM Equipment";
        //stringInsert
        //stringDelete
        //stringEdit

        public static bool TableExists(string tableName)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={connStr}"))
                {
                    connection.Open();
                    var command = new SqliteCommand(
                        $"SELECT count(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'",
                        connection);
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
            }
            catch
            {
                return false;
            }
        }


        //+CREATE TABLES
        #region CREATE
        public static void CreateTableCashier()
        {


            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection.Open();
                command = new SqliteConnection().CreateCommand();
                command.Connection = connection;
                command.CommandText = @"CREATE TABLE Cashier (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Password TEXT NOT NULL);";
                command.ExecuteNonQuery();

                MessageBox.Show("Таблица Кассир создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Кассира ошибка: {ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        public static void CreateTableClient()
        {
            if (TableExists("Client"))
            {
                MessageBox.Show("Таблица Кассир уже существует!");
                return;
            }

            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                /*connection.Open();
                command = new SqliteConnection().CreateCommand();
                command.Connection = connection;
                command.CommandText = @"CREATE TABLE Client (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, FirstName TEXT NOT NULL, LastName TEXT NOT NULL, SecondtName TEXT NOT NULL, Document TEXT NOT NULL, PhoneNumber TEXT);";
                command.ExecuteNonQuery();

                MessageBox.Show("Таблица Клиента создана!");*/

                connection = new SqliteConnection($"Data Source={connStr}");
                connection.Open();

                command = new SqliteCommand(
                    @"CREATE TABLE Cashier (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT NOT NULL, Password TEXT NOT NULL);",
                    connection);
                command.ExecuteNonQuery();

                MessageBox.Show("Таблица Кассир создана!");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Клиента ошибка: {ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        public static void CreateTableRental()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection.Open();
                command = new SqliteConnection().CreateCommand();
                command.Connection = connection;
                command.CommandText = @"CREATE TABLE Rental (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, DateStart TEXT NOT NULL, DateEnd TEXT NOT NULL, TotalPrice DOUBLE NOT NULL, Status TEXT NOT NULL);";
                command.ExecuteNonQuery();

                MessageBox.Show("Таблица Аренды создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Аренды ошибка: {ex}");
            }
            finally
            {
                connection.Close();
            }
        }
        public static void CreateTableEquipment()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection.Open();
                command = new SqliteConnection().CreateCommand();
                command.Connection = connection;
                command.CommandText = @"CREATE TABLE Equipment (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, Inv_numbery TEXT NOT NULL, Type TEXT NOT NULL, Size TEXT NOT NULL, Brand TEXT NOT NULL, Model TEXT NOT NULL, Image BLOB, Status Text NOT NULL);";
                command.ExecuteNonQuery();

                MessageBox.Show("Таблица Оборудования создана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Оборудования ошибка: {ex}");
            }
            finally
            {
                connection.Close(); 
            }
        }
        #endregion

        //+SELECT 
        #region SELECT
        public void SelectTableCashier()
        {
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;
            try
            {
                connection = new SqliteConnection($"Data Source={connStr}");
                connection.Open();

                command = new SqliteCommand(stringSelectCashier, connection);
                reader = command.ExecuteReader();


                /*if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        var id = reader.GetValue(0);
                        var name = reader.GetValue(1);
                        var password = reader.GetValue(2);

                        results.AppendLine($"{id} \t {name} \t {password}");

                    }
                    MessageBox.Show(results.ToString(), "Результаты");
                }
                else
                {
                    MessageBox.Show("Нет данных", "Информация");
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
            }
            finally
            {
                // Важно освобождать ресурсы в правильном порядке
                try
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch { } // Игнорируем ошибки при закрытии

                try
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
                catch { }

                try
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                catch { }

                try
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
                catch { }
            }
            
        }
        public static DataTable SelectTableClient()
        {
            DataTable table = new DataTable();
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connStr}");
                connection.Open();

                command = new SqliteCommand(stringSelectClient, connection);
                reader = command.ExecuteReader();


                StringBuilder results = new StringBuilder();

                table.Load(reader);
                /*if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        var id = reader.GetValue(0);
                        var firstName = reader.GetValue(1);
                        var lastName = reader.GetValue(2);
                        var secondName = reader.GetValue(3);
                        var document = reader.GetValue(4);
                        var phoneNumber = reader.GetValue(5);

                        results.AppendLine($"{id}\t{firstName}\t{lastName}\t{secondName}\t{document}\t{phoneNumber}");
                    }
                    MessageBox.Show(results.ToString(), "Результаты");
                }
                else
                {
                    MessageBox.Show("Нет данных", "Информация");
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
            }
            finally
            {
                // Важно освобождать ресурсы в правильном порядке
                try
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch { } // Игнорируем ошибки при закрытии

                try
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
                catch { }

                try
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                catch { }

                try
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
                catch { }
            }

            return table;
        }
        public static void SelectTableRental()
        {
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connStr}");
                connection.Open();

                command = new SqliteCommand(stringSelectRental, connection);
                reader = command.ExecuteReader();


                StringBuilder results = new StringBuilder();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        var id = reader.GetValue(0);
                        var dataStart = reader.GetValue(1);
                        var dataEnd = reader.GetValue(2);
                        var totalPrice = reader.GetValue(3);
                        var status = reader.GetValue(4);

                        results.AppendLine($"{id}\t{dataStart}\t{dataEnd}\t{totalPrice}\t{status}");
                    }
                    MessageBox.Show(results.ToString(), "Результаты");
                }
                else
                {
                    MessageBox.Show("Нет данных", "Информация");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
            }
            finally
            {
                // Важно освобождать ресурсы в правильном порядке
                try
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch { } // Игнорируем ошибки при закрытии

                try
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
                catch { }

                try
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                catch { }

                try
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
                catch { }
            }
        }
        public static void SelectTableEquipment() 
        {
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connStr}");
                connection.Open();

                command = new SqliteCommand(stringSelectEquipment, connection);
                reader = command.ExecuteReader();


                StringBuilder results = new StringBuilder();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        var id = reader.GetValue(0);
                        var type = reader.GetValue(1);
                        var size = reader.GetValue(2);
                        var brand = reader.GetValue(3);
                        var status = reader.GetValue(4);
                        var model = reader.GetValue(5);
                        var image = reader.GetValue(6);

                        results.AppendLine($"{id}\t{type}\t{size}\t{brand}\t{status}\t{model}\t{image}");
                    }
                    MessageBox.Show(results.ToString(), "Результаты");
                }
                else
                {
                    MessageBox.Show("Нет данных", "Информация");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
            }
            finally
            {
                // Важно освобождать ресурсы в правильном порядке
                try
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
                catch { } // Игнорируем ошибки при закрытии

                try
                {
                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
                catch { }

                try
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
                catch { }

                try
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                    }
                }
                catch { }
            }
        }
        #endregion

        //INSERT
        #region INSERT
        public static void InsertTableCashier()
        {
        }
        public static void InsertTableClient()
        {
        }
        public static void InsertTableRental()
        {
        }
        public static void InsertTableEquipment()
        {
        }
        #endregion

        //DELETE
        #region DELETE
        public static void DeleteTableCashier()
        {
        }
        public static void DeleteTableClient()
        {
        }
        public static void DeleteTableRental()
        {
        }
        public static void DeleteTableEquipment()
        {
        }
        #endregion

        //EDIT
        #region EDIT
        public static void EditTableCashier()
        {
        }
        public static void EditTableClient()
        {
        }
        public static void EditTableRental()
        {
        }
        public static void EditTableEquipment()
        {
        }
        #endregion
    }


}
