using System;
using System.Windows;

namespace Ski_equipment_rental_accounting_system
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Глобальный обработчик необработанных исключений
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"Критическая ошибка: {ex.Message}\n{ex.StackTrace}",
                              "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Сохраняем в лог файл
                try
                {
                    System.IO.File.AppendAllText("error_log.txt",
                        $"[{DateTime.Now}] {ex.Message}\n{ex.StackTrace}\n\n");
                }
                catch { }
            };

            this.DispatcherUnhandledException += (sender, args) =>
            {
                args.Handled = true;
                MessageBox.Show($"Ошибка в интерфейсе: {args.Exception.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            // Инициализация БД при старте
            try
            {
                DataBase.CreateAllTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}",
                              "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}