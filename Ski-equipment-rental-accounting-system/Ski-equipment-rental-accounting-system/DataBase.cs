using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Ski_equipment_rental_accounting_system
{
    public class DataBase
    {
        public const string connString              = "RentalDB.db";

        //stringSelect
        public const string stringSelectCashier     = "SELECT * FROM Cashier";
        public const string stringSelectClient      = "SELECT * FROM Client";
        public const string stringSelectRental      = "SELECT * FROM Rental";
        public const string stringSelectEquipment   = "SELECT * FROM Equipment";
        //stringInsert
        public const string stringInsertCashier     = "INSERT INTO Cashier (Name, Password) VALUES (@name, @password)";
        public const string stringInsertClient      = "INSERT INTO Client (FirstName, LastName, SecondName, Document, PhoneNumber) VALUES (@firstName, @lastName, @secondName, @document, @phoneNumber)";
        public const string stringInsertRental      = "INSERT INTO Rental (DateStart, DateEnd, TotalPrice, Status) VALUES (@dateStart, @dateEnd, @totalPrice, @status)";
        public const string stringInsertEquipment   = "INSERT INTO Equipment (Inv_numbery, Type, Size, Brand, Model, Image, Status) VALUES (@inv_numbery, @type, @size, @brand, @model, @image, @status)";
        //stringDelete
        public const string stringDeleteCashier     = "DELETE FROM Cashier WHERE Id = @id";
        public const string stringDeleteClient      = "DELETE FROM Client WHERE Id = @id";
        public const string stringDeleteRental      = "DELETE FROM Rental WHERE Id = @id";
        public const string stringDeleteEquipment   = "DELETE FROM Equipment WHERE Id = @id";
        //stringEdit
        public const string stringEditCashier       = "UPDATE Cashier SET Name = @name, Password = @password WHERE Id = @id";
        public const string stringEditClient        = "UPDATE Client SET FirstName = @firstName, LastName = @lastName, SecondName = @secondName, Document = @document, PhoneNumber = @phoneNumber WHERE Id = @id";
        public const string stringEditRental        = "UPDATE Rental SET DateStart = @dateStart, DateEnd = @dateEnd, TotalPrice = @totalPrice, Status = @status WHERE Id = @id";
        public const string stringEditEquipment     = "UPDATE Equipment SET Inv_numbery = @inv_numbery, Type = @type, Size = @size, Brand = @brand, Model = @model, Image = @image, Status = @status WHERE Id = @id";

        //+CREATE TABLES
        #region CREATE
        public static void CreateTableCashier()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Cashier (
                                                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                Name TEXT NOT NULL,
                                                Password TEXT NOT NULL);",
                                                connection);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Кассира ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose(); // ← ДОБАВЬТЕ ЭТУ СТРОКУ!
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
        public static void CreateTableClient()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Client (
                                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                            FirstName TEXT NOT NULL,
                                            LastName TEXT NOT NULL,
                                            SecondName TEXT NOT NULL,
                                            Document TEXT NOT NULL, 
                                            PhoneNumber TEXT);",
                                                    connection);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Клиента ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose(); // ← ДОБАВЬТЕ ЭТУ СТРОКУ!
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
        public static void CreateTableRental()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Rental (
                                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                            DateStart TEXT NOT NULL,
                                            DateEnd TEXT NOT NULL,
                                            TotalPrice REAL NOT NULL,
                                            Status TEXT NOT NULL);",
                                                    connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Аренды ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose(); // ← ДОБАВЬТЕ ЭТУ СТРОКУ!
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
        public static void CreateTableEquipment()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Equipment (
                                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                            Inv_numbery TEXT NOT NULL,
                                            Type TEXT NOT NULL,
                                            Size TEXT NOT NULL,
                                            Brand TEXT NOT NULL,
                                            Model TEXT NOT NULL,
                                            Image BLOB,
                                            Status TEXT NOT NULL);",
                                                    connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Оборудования ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose(); // ← ДОБАВЬТЕ ЭТУ СТРОКУ!
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
        #endregion

        //+SELECT 
        #region SELECT
        public static DataTable SelectTableCashier()
        {
            DataTable table = new DataTable();
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;
            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringSelectCashier, connection);
                reader = command.ExecuteReader();

                /*StringBuilder results = new StringBuilder();

                if (reader.HasRows) // если есть данные
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

                table.Load(reader);

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
                return table;
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
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringSelectClient, connection);
                reader = command.ExecuteReader();

                /*StringBuilder results = new StringBuilder();

                if (reader.HasRows) // если есть данные
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

                table.Load(reader);

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
                return table;
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
        public static DataTable SelectTableRental()
        {
            DataTable table = new DataTable();
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringSelectRental, connection);
                reader = command.ExecuteReader();

                /*StringBuilder results = new StringBuilder();

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
                }*/
                table.Load(reader) ;

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
                return table;
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
        public static DataTable SelectTableEquipment() 
        {
            DataTable table = new DataTable();
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringSelectEquipment, connection);
                reader = command.ExecuteReader();

                /*StringBuilder results = new StringBuilder();

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
                }*/
                table.Load(reader);

                return table;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения {ex}");
                return table;
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
        public static void InsertTableCashier(string name, string password)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringInsertCashier, connection);

                // создаем параметр для имени и добавляем его
                SqliteParameter nameParam = new SqliteParameter("@name", name);
                command.Parameters.Add(nameParam);

                // создаем параметр для пароля и добавляем его
                SqliteParameter passwordParam = new SqliteParameter("@password", password);
                command.Parameters.Add(passwordParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public static void InsertTableClient(string firstName, string lastName, string secondName,
                                           string document, string phoneNumber)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringInsertClient, connection);

                // создаем параметр для имени и добавляем его
                SqliteParameter firstNameParam = new SqliteParameter("@firstName", firstName);
                command.Parameters.Add(firstNameParam);

                // создаем параметр для фамилии и добавляем его
                SqliteParameter lastNameParam = new SqliteParameter("@lastName", lastName);
                command.Parameters.Add(lastNameParam);

                SqliteParameter secondNameParam = new SqliteParameter("@secondName", secondName);
                command.Parameters.Add(secondNameParam);

                SqliteParameter documentParam = new SqliteParameter("@document", document);
                command.Parameters.Add(documentParam);

                SqliteParameter phoneNumberParam = new SqliteParameter("@phoneNumber", phoneNumber);
                command.Parameters.Add(phoneNumberParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public static void InsertTableRental(DateTime dateStart, DateTime dateEnd,
                                           decimal totalPrice, string status)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringInsertRental, connection);

                // создаем параметр для даты начала и добавляем его
                SqliteParameter dateStartParam = new SqliteParameter("@dateStart", dateStart.ToString("yyyy-MM-dd"));
                command.Parameters.Add(dateStartParam);

                // создаем параметр для даты окончания и добавляем его
                SqliteParameter dateEndParam = new SqliteParameter("@dateEnd", dateEnd.ToString("yyyy-MM-dd"));
                command.Parameters.Add(dateEndParam);

                SqliteParameter totalPriceParam = new SqliteParameter("@totalPrice", totalPrice);
                command.Parameters.Add(totalPriceParam);

                SqliteParameter statusParam = new SqliteParameter("@status", status);
                command.Parameters.Add(statusParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public static void InsertTableEquipment(string invNumber, string type, string size,
                                              string brand, string model, byte[] image, string status)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringInsertEquipment, connection);

                // создаем параметр для инвентарного номера и добавляем его
                SqliteParameter invNumberParam = new SqliteParameter("@inv_numbery", invNumber);
                command.Parameters.Add(invNumberParam);

                // создаем параметр для типа и добавляем его
                SqliteParameter typeParam = new SqliteParameter("@type", type);
                command.Parameters.Add(typeParam);

                SqliteParameter sizeParam = new SqliteParameter("@size", size);
                command.Parameters.Add(sizeParam);

                SqliteParameter brandParam = new SqliteParameter("@brand", brand);
                command.Parameters.Add(brandParam);

                SqliteParameter modelParam = new SqliteParameter("@model", model);
                command.Parameters.Add(modelParam);

                SqliteParameter imageParam = new SqliteParameter("@image", image ?? (object)DBNull.Value);
                command.Parameters.Add(imageParam);

                SqliteParameter statusParam = new SqliteParameter("@status", status);
                command.Parameters.Add(statusParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        #endregion


        //DELETE
        #region DELETE
        public static void DeleteTableCashier(int id)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringDeleteCashier, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Удалено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления кассира: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        public static void DeleteTableClient(int id)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringDeleteClient, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Удалено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления клиента: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        public static void DeleteTableRental(int id)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringDeleteRental, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Удалено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления аренды: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        public static void DeleteTableEquipment(int id)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringDeleteEquipment, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Удалено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления оборудования: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }
        #endregion

        //EDIT
        #region EDIT
        public static void EditTableCashier(int id, string name, string password)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringEditCashier, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                SqliteParameter nameParam = new SqliteParameter("@name", name);
                command.Parameters.Add(nameParam);

                SqliteParameter passwordParam = new SqliteParameter("@password", password);
                command.Parameters.Add(passwordParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Обновлено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления кассира: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }

        }
        public static void EditTableClient(int id, string firstName, string lastName, string secondName,
                                  string document, string phoneNumber)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringEditClient, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                SqliteParameter firstNameParam = new SqliteParameter("@firstName", firstName);
                command.Parameters.Add(firstNameParam);

                SqliteParameter lastNameParam = new SqliteParameter("@lastName", lastName);
                command.Parameters.Add(lastNameParam);

                SqliteParameter secondNameParam = new SqliteParameter("@secondName", secondName);
                command.Parameters.Add(secondNameParam);

                SqliteParameter documentParam = new SqliteParameter("@document", document);
                command.Parameters.Add(documentParam);

                SqliteParameter phoneNumberParam = new SqliteParameter("@phoneNumber", phoneNumber);
                command.Parameters.Add(phoneNumberParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Обновлено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления клиента: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }

        }
        public static void EditTableRental(int id, DateTime dateStart, DateTime dateEnd,
                                  decimal totalPrice, string status)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringEditRental, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                SqliteParameter dateStartParam = new SqliteParameter("@dateStart", dateStart.ToString("yyyy-MM-dd"));
                command.Parameters.Add(dateStartParam);

                SqliteParameter dateEndParam = new SqliteParameter("@dateEnd", dateEnd.ToString("yyyy-MM-dd"));
                command.Parameters.Add(dateEndParam);

                SqliteParameter totalPriceParam = new SqliteParameter("@totalPrice", totalPrice);
                command.Parameters.Add(totalPriceParam);

                SqliteParameter statusParam = new SqliteParameter("@status", status);
                command.Parameters.Add(statusParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Обновлено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления аренды: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }

        }
        public static void EditTableEquipment(int id, string invNumber, string type, string size,
                                     string brand, string model, byte[] image, string status)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringEditEquipment, connection);

                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);

                SqliteParameter invNumberParam = new SqliteParameter("@inv_numbery", invNumber);
                command.Parameters.Add(invNumberParam);

                SqliteParameter typeParam = new SqliteParameter("@type", type);
                command.Parameters.Add(typeParam);

                SqliteParameter sizeParam = new SqliteParameter("@size", size);
                command.Parameters.Add(sizeParam);

                SqliteParameter brandParam = new SqliteParameter("@brand", brand);
                command.Parameters.Add(brandParam);

                SqliteParameter modelParam = new SqliteParameter("@model", model);
                command.Parameters.Add(modelParam);

                SqliteParameter imageParam = new SqliteParameter("@image", image ?? (object)DBNull.Value);
                command.Parameters.Add(imageParam);

                SqliteParameter statusParam = new SqliteParameter("@status", status);
                command.Parameters.Add(statusParam);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Обновлено записей: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления оборудования: {ex.Message}");
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                if (connection != null)
                {
                    connection.Dispose();
                }
            }

        }
        #endregion

        public static void CreateAllTables()
        {
            CreateTableCashier();
            CreateTableClient();
            CreateTableRental();
            CreateTableEquipment();
        }
    }
}
