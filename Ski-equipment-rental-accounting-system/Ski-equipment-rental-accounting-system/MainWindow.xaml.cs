using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Код Главной формы
    /// </summary>

        public partial class MainWindow : Window
        {
            private string currentTable = "";

            public MainWindow()
            {
                InitializeComponent();

                // Обновляем БД перед использованием
                UpdateDatabaseSchema();

<<<<<<< HEAD
                // Обновляем схему Rental
                DatabaseUpdater.UpdateRentalTableSchema();

                // Проверяем, есть ли данные
                var clients = DataBase.SelectTableClient();
                if (clients.Rows.Count == 0)
=======
                // Устанавливаем начальное состояние
                SetActiveButton(btnClients);  // Начнем с клиентов
                ShowTable("Client");
            }

            private void UpdateDatabaseSchema()
            {
                try
>>>>>>> d57a948fa8f43d42efa8eba40f660900595a991a
                {
                    // Сначала создаем таблицы
                    DataBase.CreateAllTables();

                    // Явно обновляем схему Client
                    DataBase.UpdateClientTableSchema();

                    // Проверяем, есть ли данные
                    var clients = DataBase.SelectTableClient();
                    if (clients.Rows.Count == 0)
                    {
                        AddDemoData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка инициализации БД: {ex.Message}");
                }
            }


            private void InitializeDatabase()
            {
<<<<<<< HEAD
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}");
            }
        }



        private void InitializeDatabase()
        {
=======
>>>>>>> d57a948fa8f43d42efa8eba40f660900595a991a
            try
            {
                DataBase.CreateAllTables();

                // Для теста - если таблицы пустые, добавляем демо-данные
                var clients = DataBase.SelectTableClient();
                if (clients.Rows.Count == 0)
                {
                    AddDemoData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}");
            }
        }

        private void AddDemoData()
        {
            // Добавляем демо-клиентов
            DataBase.InsertTableClient("Иванов", "Иван", "Иванович",
                                      DocumentType.Passport, "4501 123456", "+7 (999) 123-45-67");
            DataBase.InsertTableClient("Петрова", "Мария", "Сергеевна",
                                      DocumentType.InternationalPassport, "75 1234567", "+7 (911) 765-43-21");

            // Добавляем демо-оборудование
            DataBase.InsertTableEquipment("INV-001", "Лыжи", "180cm", "Atomic", "Redster", null, "Доступно");
            DataBase.InsertTableEquipment("INV-002", "Ботинки", "42", "Salomon", "S/Pro", null, "Доступно");
        }

        // ========== ОБРАБОТЧИКИ КНОПОК НАВИГАЦИИ ==========

        private void btnRentals_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(btnRentals);
            ShowTable("Rental");
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(btnClients);
            ShowTable("Client");
        }

        private void btnEquipment_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(btnEquipment);
            ShowTable("Equipment");
        }

        private void btnBookings_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(btnBookings);
            MessageBox.Show("Раздел 'Бронирования' в разработке", "Информация");
        }

        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            SetActiveButton(btnMaintenance);
            MessageBox.Show("Раздел 'Обслуживание' в разработке", "Информация");
        }

        // ========== ОСНОВНЫЕ ФУНКЦИИ ==========

        private void ShowTable(string tableName)
        {
            try
            {
                currentTable = tableName;
                DataTable table = null;

                switch (tableName)
                {
                    case "Rental":
                        table = DataBase.SelectTableRental();
                        txtCurrentSection.Text = "Аренды";
                        break;
                    case "Client":
                        table = DataBase.SelectTableClient();
                        txtCurrentSection.Text = "Клиенты";
                        break;
                    case "Equipment":
                        table = DataBase.SelectTableEquipment();
                        txtCurrentSection.Text = "Оборудование";
                        break;
                }

                if (table != null)
                {
                    dataGrid.ItemsSource = table.DefaultView;
                    ConfigureDataGridColumns(table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки таблицы: {ex.Message}");
            }
        }

        private void ConfigureDataGridColumns(DataTable table)
        {
            dataGrid.AutoGenerateColumns = false;
            dataGrid.Columns.Clear();

            foreach (DataColumn column in table.Columns)
            {
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = GetRussianColumnName(column.ColumnName),
                    Binding = new System.Windows.Data.Binding(column.ColumnName),
                    Width = DataGridLength.Auto
                });
            }
        }

        private string GetRussianColumnName(string englishName)
        {
            switch (englishName)
            {
                case "Id": return "ID";
                case "FirstName": return "Имя";
                case "LastName": return "Фамилия";
                case "SecondName": return "Отчество";
                case "DocumentType": return "Тип документа";
                case "DocumentNumber": return "Номер документа";
                case "PhoneNumber": return "Телефон";
                case "InventoryNumber": return "Инв. номер";
                case "Type": return "Тип";
                case "Size": return "Размер";
                case "Brand": return "Бренд";
                case "Model": return "Модель";
                case "Status": return "Статус";
                default: return englishName;
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            // Сбрасываем все кнопки к обычному стилю
            btnRentals.Style = (Style)FindResource("NavButtonStyle");
            btnClients.Style = (Style)FindResource("NavButtonStyle");
            btnEquipment.Style = (Style)FindResource("NavButtonStyle");
            btnBookings.Style = (Style)FindResource("NavButtonStyle");
            btnMaintenance.Style = (Style)FindResource("NavButtonStyle");

            // Устанавливаем активный стиль для выбранной кнопки
            activeButton.Style = (Style)FindResource("NavButtonActiveStyle");
        }

        // ========== БЫСТРЫЕ ДЕЙСТВИЯ ==========

        private void btnQuickNewRental_Click_1(object sender, RoutedEventArgs e)
        {
            // В зависимости от текущей таблицы открываем соответствующее окно
            switch (currentTable)
            {
                case "Client":
                    ShowAddClientWindow();
                    break;
                case "Equipment":
                    ShowAddEquipmentWindow();
                    break;
                case "Rental":
                    ShowAddRentalWindow(); // НОВЫЙ МЕТОД
                    break;
                default:
                    ShowAddClientWindow();
                    break;
            }
        }
        private void ShowAddRentalWindow()
        {
            AddRentalWindow addRentalWindow = new AddRentalWindow();
            addRentalWindow.Owner = this;
            if (addRentalWindow.ShowDialog() == true)
            {
                // Обновляем таблицу аренд
                btnRentals_Click(null, null);
            }
        }

        // ДОБАВИТЬ МЕТОД ДЛЯ ОБОРУДОВАНИЯ:
        private void ShowAddEquipmentWindow()
        {
            AddEquipmentWindow addWindow = new AddEquipmentWindow();
            if (addWindow.ShowDialog() == true)
            {
                btnEquipment_Click(null, null);
            }
        }

        // ДОБАВИТЬ НОВЫЙ МЕТОД:
        private void ShowAddRentalWindow()
        {
            AddRentalWindow addRentalWindow = new AddRentalWindow();
            addRentalWindow.Owner = this;
            if (addRentalWindow.ShowDialog() == true)
            {
                // Обновляем таблицу аренд
                btnRentals_Click(null, null);
            }
        }

        // ДОБАВИТЬ МЕТОД ДЛЯ ОБОРУДОВАНИЯ:
        private void ShowAddEquipmentWindow()
        {
            AddEquipmentWindow addWindow = new AddEquipmentWindow();
            if (addWindow.ShowDialog() == true)
            {
                btnEquipment_Click(null, null);
            }
        }

        private void ShowAddClientWindow()
        {
            AddClientWindow addWindow = new AddClientWindow();
            if (addWindow.ShowDialog() == true)
            {
                // Обновляем таблицу клиентов
                btnClients_Click(null, null);
            }
        }

        private void btnQuickReturn_Click_1(object sender, RoutedEventArgs e)
        {
            // Эта кнопка теперь для возврата оборудования
            MessageBox.Show("Функция быстрого возврата - в разработке", "Информация");
        }

        // ========== НОВЫЕ КНОПКИ ==========

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настройки системы - в разработке", "Информация");
        }

        private void btnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления!", "Предупреждение",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                int id = Convert.ToInt32(selectedRow["Id"]);

                var result = MessageBox.Show($"Удалить выбранную запись (ID: {id})?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo,
                                           MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    switch (currentTable)
                    {
                        case "Client":
                            DataBase.DeleteTableClient(id);
                            break;
                        case "Equipment":
                            DataBase.DeleteTableEquipment(id);
                            break;
                        case "Rental":
                            DataBase.DeleteTableRental(id);
                            break;
                    }

                    MessageBox.Show("Запись успешно удалена!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    // Обновляем таблицу
                    ShowTable(currentTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить логику при выборе строки
        }
    }
}