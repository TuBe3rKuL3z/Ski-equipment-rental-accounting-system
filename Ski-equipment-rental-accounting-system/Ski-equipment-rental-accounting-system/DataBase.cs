using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Класс Для управления базами данных
    /// </summary>

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
        public const string stringInsertClient      = "INSERT INTO Client (FirstName, LastName, SecondName, DocumentType, DocumentNumber, PhoneNumber) VALUES (@firstName, @lastName, @secondName, @documentType, @documentNumber, @phoneNumber)";
        public const string stringInsertRental = "INSERT INTO Rental (ClientId, EquipmentId, StartDate, EndDate, TotalPrice, Status, PaymentStatus, PaymentMethod, DiscountCode, DiscountAmount) VALUES (@clientId, @equipmentId, @startDate, @endDate, @totalPrice, @status, @paymentStatus, @paymentMethod, @discountCode, @discountAmount)";
        public const string stringInsertEquipment   = "INSERT INTO Equipment (Inv_numbery, Type, Size, Brand, Model, Image, Status) VALUES (@inv_numbery, @type, @size, @brand, @model, @image, @status)";
        //stringDelete
        public const string stringDeleteCashier     = "DELETE FROM Cashier WHERE Id = @id";
        public const string stringDeleteClient      = "DELETE FROM Client WHERE Id = @id";
        public const string stringDeleteRental      = "DELETE FROM Rental WHERE Id = @id";
        public const string stringDeleteEquipment   = "DELETE FROM Equipment WHERE Id = @id";
        //stringEdit
        public const string stringEditCashier       = "UPDATE Cashier SET Name = @name, Password = @password WHERE Id = @id";
        public const string stringEditClient        = "UPDATE Client SET FirstName = @firstName, LastName = @lastName, SecondName = @secondName, DocumentType = @documentType, DocumentNumber = @documentNumber, PhoneNumber = @phoneNumber WHERE Id = @id";
        public const string stringEditRental        = "UPDATE Rental SET DateStart = @dateStart, DateEnd = @dateEnd, TotalPrice = @totalPrice, Status = @status WHERE Id = @id";
        public const string stringEditEquipment     = "UPDATE Equipment SET Inv_numbery = @inv_numbery, Type = @type, Size = @size, Brand = @brand, Model = @model, Image = @image, Status = @status WHERE Id = @id";

        // ДОБАВИТЬ В КОНСТАНТЫ:
        public const string stringSelectDiscount = "SELECT * FROM Discount";
        public const string stringInsertDiscount = "INSERT INTO Discount (Code, Type, Value, ValidFrom, ValidTo, IsActive, Description) VALUES (@code, @type, @value, @validFrom, @validTo, @isActive, @description)";
        public const string stringDeleteDiscount = "DELETE FROM Discount WHERE Id = @id";
        public const string stringEditDiscount = "UPDATE Discount SET Code = @code, Type = @type, Value = @value, ValidFrom = @validFrom, ValidTo = @validTo, IsActive = @isActive, Description = @description WHERE Id = @id";

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

                // ИСПРАВЛЕННАЯ КОМАНДА СОЗДАНИЯ ТАБЛИЦЫ
                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Client (
                            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                            LastName TEXT NOT NULL,
                            FirstName TEXT NOT NULL,
                            SecondName TEXT NOT NULL,
                            DocumentType INTEGER NOT NULL DEFAULT 1,  -- <-- ДОБАВЛЕНО!
                            DocumentNumber TEXT NOT NULL,
                            PhoneNumber TEXT,
                            RegistrationDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            ActiveRentalsCount INTEGER DEFAULT 0 );", connection);

                command.ExecuteNonQuery();

                // Проверяем и обновляем схему если нужно
                UpdateClientTableSchema();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Клиента ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose();
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
                            ClientId INTEGER NOT NULL,
                            EquipmentId INTEGER NOT NULL,
                            StartDate TEXT NOT NULL,
                            EndDate TEXT NOT NULL,
                            TotalPrice REAL NOT NULL,
                            Status INTEGER NOT NULL, 
                            PaymentStatus INTEGER NOT NULL,
                            PaymentMethod INTEGER NOT NULL,
                            DiscountCode TEXT,
                            DiscountAmount REAL DEFAULT 0,
                            ActualReturnDate TEXT,
                            FOREIGN KEY (ClientId) REFERENCES Client(Id),
                            FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id)
                        );", connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Аренды ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose();
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
                                    InventoryNumber TEXT NOT NULL,
                                    Type INTEGER NOT NULL, 
                                    Size TEXT NOT NULL,
                                    Brand TEXT NOT NULL,
                                    Model TEXT NOT NULL,
                                    DailyRentalPrice REAL NOT NULL,
                                    Status INTEGER NOT NULL,
                                    Image BLOB );", connection);


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

        // ДОБАВИТЬ МЕТОД СОЗДАНИЯ ТАБЛИЦЫ:
        public static void CreateTableDiscount()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"CREATE TABLE IF NOT EXISTS Discount (
            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
            Code TEXT NOT NULL UNIQUE,
            Type INTEGER NOT NULL,
            Value REAL NOT NULL,
            ValidFrom TEXT NOT NULL,
            ValidTo TEXT NOT NULL,
            IsActive INTEGER NOT NULL DEFAULT 1,
            Description TEXT,
            UsageCount INTEGER DEFAULT 0
        );", connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Таблица Скидок ошибка: {ex}");
            }
            finally
            {
                if (command != null) command.Dispose();
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

        public static void InsertTableClient(string lastName, string firstName, string secondName,
                                   DocumentType documentType, string documentNumber, string phoneNumber)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                // ИСПОЛЬЗУЕМ ИСПРАВЛЕННУЮ КОНСТАНТУ
                command = new SqliteCommand(stringInsertClient, connection);

                // Правильный порядок параметров!
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@secondName", secondName);
                command.Parameters.AddWithValue("@documentType", (int)documentType);  // Enum -> int
                command.Parameters.AddWithValue("@documentNumber", documentNumber);

                // Для телефонного номера: если пустой - ставим NULL
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    command.Parameters.AddWithValue("@phoneNumber", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        public static void InsertTableRental(int clientId, int equipmentId,
                                   DateTime startDate, DateTime endDate,
                                   decimal totalPrice, RentalStatus status,
                                   PaymentStatus paymentStatus, PaymentMethod paymentMethod,
                                   string discountCode = null, decimal discountAmount = 0)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(@"INSERT INTO Rental 
                                    (ClientId, EquipmentId, StartDate, EndDate, TotalPrice, 
                                     Status, PaymentStatus, PaymentMethod, DiscountCode, DiscountAmount) 
                                    VALUES (@clientId, @equipmentId, @startDate, @endDate, @totalPrice, 
                                            @status, @paymentStatus, @paymentMethod, @discountCode, @discountAmount)",
                                            connection);

                command.Parameters.AddWithValue("@clientId", clientId);
                command.Parameters.AddWithValue("@equipmentId", equipmentId);
                command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@totalPrice", totalPrice);
                command.Parameters.AddWithValue("@status", (int)status);
                command.Parameters.AddWithValue("@paymentStatus", (int)paymentStatus);
                command.Parameters.AddWithValue("@paymentMethod", (int)paymentMethod);
                command.Parameters.AddWithValue("@discountCode", discountCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@discountAmount", discountAmount);

                int number = command.ExecuteNonQuery();
                MessageBox.Show($"Добавлено объектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
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
                                 DocumentType documentType, string documentNumber, string phoneNumber)
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

                command.Parameters.AddWithValue("@documentType", (int)documentType);

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

        // ОБНОВИТЬ МЕТОД CreateAllTables():
        public static void CreateAllTables()
        {
            CreateTableCashier();
            CreateTableClient();
            CreateTableRental();
            CreateTableEquipment();
            CreateTableDiscount(); // ДОБАВИТЬ ЭТУ СТРОКУ
        }

        #region НОВЫЕ МЕТОДЫ ДЛЯ РАБОТЫ С ОБЪЕКТАМИ
        // Метод 1: Добавление клиента через объект Client
        public static void InsertClient(Client client)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                // Валидация клиента
                var validation = client.Validate();
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage);
                    return;
                }

                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                // SQL команда для нового формата (с DocumentType)
                string sql = "INSERT INTO Client (FirstName, LastName, SecondName, DocumentType, DocumentNumber, PhoneNumber) " +
                             "VALUES (@firstName, @lastName, @secondName, @documentType, @documentNumber, @phoneNumber)";

                command = new SqliteCommand(sql, connection);

                // Добавляем параметры
                command.Parameters.AddWithValue("@firstName", client.FirstName);
                command.Parameters.AddWithValue("@lastName", client.LastName);
                command.Parameters.AddWithValue("@secondName", client.SecondName);
                command.Parameters.AddWithValue("@documentType", (int)client.DocumentType); // Enum → число
                command.Parameters.AddWithValue("@documentNumber", client.DocumentNumber);
                command.Parameters.AddWithValue("@phoneNumber",
                    string.IsNullOrEmpty(client.PhoneNumber) ? (object)DBNull.Value : client.PhoneNumber);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Клиент добавлен: {client.FullName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        // Метод 2: Обновление клиента через объект Client
        public static void UpdateClient(Client client)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                // Валидация клиента
                var validation = client.Validate();
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage);
                    return;
                }

                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                // SQL команда для нового формата (с DocumentType)
                string sql = "UPDATE Client SET " +
                             "FirstName = @firstName, " +
                             "LastName = @lastName, " +
                             "SecondName = @secondName, " +
                             "DocumentType = @documentType, " +
                             "DocumentNumber = @documentNumber, " +
                             "PhoneNumber = @phoneNumber " +
                             "WHERE Id = @id";

                command = new SqliteCommand(sql, connection);

                // Добавляем параметры
                command.Parameters.AddWithValue("@id", client.Id);
                command.Parameters.AddWithValue("@firstName", client.FirstName);
                command.Parameters.AddWithValue("@lastName", client.LastName);
                command.Parameters.AddWithValue("@secondName", client.SecondName);
                command.Parameters.AddWithValue("@documentType", (int)client.DocumentType); // Enum → число
                command.Parameters.AddWithValue("@documentNumber", client.DocumentNumber);
                command.Parameters.AddWithValue("@phoneNumber",
                    string.IsNullOrEmpty(client.PhoneNumber) ? (object)DBNull.Value : client.PhoneNumber);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Клиент обновлен: {client.FullName} (ID: {client.Id})");
                }
                else
                {
                    MessageBox.Show($"Клиент с ID {client.Id} не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления клиента: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        // Метод 3: Получение всех клиентов как List<Client>
        public static List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            SqliteConnection connection = null;
            SqliteCommand command = null;
            SqliteDataReader reader = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand("SELECT * FROM Client ORDER BY LastName, FirstName", connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        Client client = new Client
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                            DocumentType = (DocumentType)reader.GetInt32(reader.GetOrdinal("DocumentType")),
                            DocumentNumber = reader.GetString(reader.GetOrdinal("DocumentNumber")),
                            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ?
                                          string.Empty : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ?
                                               DateTime.Now : DateTime.Parse(reader.GetString(reader.GetOrdinal("RegistrationDate")))
                        };

                        clients.Add(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения клиента: {ex.Message}");
                        // Пропускаем некорректные записи
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
                // Возвращаем пустой список вместо null
            }
            finally
            {
                try
                {
                    if (reader != null && !reader.IsClosed) reader.Close();
                }
                catch { }

                try
                {
                    if (command != null) command.Dispose();
                }
                catch { }

                try
                {
                    if (connection != null && connection.State != ConnectionState.Closed)
                        connection.Close();
                }
                catch { }
            }

            return clients;
        }


        // Метод 4: Получение клиента по ID
        public static Client GetClientById(int clientId)
        {
            Client client = null;

            SqliteConnection connection = null;
            SqliteCommand command = null;
            SqliteDataReader reader = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand("SELECT * FROM Client WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@id", clientId);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    client = new Client
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        SecondName = reader.GetString(reader.GetOrdinal("SecondName")),
                        DocumentType = (DocumentType)reader.GetInt32(reader.GetOrdinal("DocumentType")),
                        DocumentNumber = reader.GetString(reader.GetOrdinal("DocumentNumber")),
                        PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ?
                                      string.Empty : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                        RegistrationDate = reader.IsDBNull(reader.GetOrdinal("RegistrationDate")) ?
                                           DateTime.Now : DateTime.Parse(reader.GetString(reader.GetOrdinal("RegistrationDate")))
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиента: {ex.Message}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return client;
        }

        public static void UpdateClientTableSchema()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                // Проверяем, существует ли столбец DocumentType
                command = new SqliteCommand(
                    "SELECT COUNT(*) FROM pragma_table_info('Client') WHERE name='DocumentType'",
                    connection);

                var hasDocumentType = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasDocumentType)
                {
                    // Добавляем столбец DocumentType
                    command = new SqliteCommand(
                        "ALTER TABLE Client ADD COLUMN DocumentType INTEGER NOT NULL DEFAULT 1",
                        connection);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Таблица Client обновлена: добавлен столбец DocumentType",
                                  "Обновление БД",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления таблицы: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }




        #endregion


        // ДОБАВИТЬ CRUD ДЛЯ DISCOUNT:
        public static DataTable SelectTableDiscount()
        {
            DataTable table = new DataTable();
            SqliteConnection connection = null;
            SqliteDataReader reader = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringSelectDiscount, connection);
                reader = command.ExecuteReader();
                table.Load(reader);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения скидок: {ex}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return table;
        }

        public static void InsertTableDiscount(Discount discount)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand(stringInsertDiscount, connection);

                command.Parameters.AddWithValue("@code", discount.Code);
                command.Parameters.AddWithValue("@type", (int)discount.Type);
                command.Parameters.AddWithValue("@value", discount.Value);
                command.Parameters.AddWithValue("@validFrom", discount.ValidFrom.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@validTo", discount.ValidTo.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@isActive", discount.IsActive ? 1 : 0);
                command.Parameters.AddWithValue("@description", discount.Description ?? string.Empty);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления скидки: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        // Метод для проверки скидки по коду
        public static Discount ValidateDiscountCode(string code)
        {
            Discount discount = null;
            SqliteConnection connection = null;
            SqliteCommand command = null;
            SqliteDataReader reader = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand("SELECT * FROM Discount WHERE Code = @code AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@code", code);

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    discount = new Discount
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Code = reader.GetString(reader.GetOrdinal("Code")),
                        Type = (DiscountType)reader.GetInt32(reader.GetOrdinal("Type")),
                        Value = reader.GetDecimal(reader.GetOrdinal("Value")),
                        ValidFrom = DateTime.Parse(reader.GetString(reader.GetOrdinal("ValidFrom"))),
                        ValidTo = DateTime.Parse(reader.GetString(reader.GetOrdinal("ValidTo"))),
                        IsActive = reader.GetInt32(reader.GetOrdinal("IsActive")) == 1,
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ?
                                      string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                        UsageCount = reader.GetInt32(reader.GetOrdinal("UsageCount"))
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки скидки: {ex.Message}");
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return discount;
        }

        // Метод для обновления счетчика использований
        public static void UpdateDiscountUsage(int discountId, int newUsageCount)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={connString}");
                connection.Open();

                command = new SqliteCommand("UPDATE Discount SET UsageCount = @usageCount WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@usageCount", newUsageCount);
                command.Parameters.AddWithValue("@id", discountId);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления скидки: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
}
