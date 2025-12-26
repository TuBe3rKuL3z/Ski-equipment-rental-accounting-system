using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace Ski_equipment_rental_accounting_system
{
    public static class DatabaseUpdater
    {
        public static void UpdateRentalTableSchema()
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={DataBase.connString}");
                connection.Open();

                // Проверяем существование столбца DiscountCode
                command = new SqliteCommand(
                    "SELECT COUNT(*) FROM pragma_table_info('Rental') WHERE name='DiscountCode'",
                    connection);

                var hasDiscountCode = Convert.ToInt32(command.ExecuteScalar()) > 0;

                if (!hasDiscountCode)
                {
                    // Добавляем столбец DiscountCode
                    command = new SqliteCommand(
                        "ALTER TABLE Rental ADD COLUMN DiscountCode TEXT",
                        connection);
                    command.ExecuteNonQuery();

                    // Добавляем столбец DiscountAmount
                    command = new SqliteCommand(
                        "ALTER TABLE Rental ADD COLUMN DiscountAmount REAL DEFAULT 0",
                        connection);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Таблица Rental обновлена: добавлены столбцы DiscountCode и DiscountAmount",
                                  "Обновление БД",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления таблицы Rental: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
}