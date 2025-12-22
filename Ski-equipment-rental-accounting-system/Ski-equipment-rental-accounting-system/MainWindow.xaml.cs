using System;
using System.Data;
using System.Windows;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataBase.CreateAllTables();
        }

        private void btnQuickNewRental_Click_1(object sender, RoutedEventArgs e)
        {
            string firstName = "Альберт";
            string lastName = "Кубанов";
            string secondName = "Азрет-Алиевич";
            string document = "Паспорт";
            string phoneNumber = "1234567890";
            DataBase.InsertTableClient(firstName, lastName, secondName, document, phoneNumber);
            DataBase.SelectTableClient();
        }

        private void btnQuickReturn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, есть ли выбранная строка
                if (dataGrid.SelectedItem == null)
                {
                    MessageBox.Show("Выберите запись для удаления!");
                    return;
                }

                // Получаем DataRowView из выбранного элемента
                DataRowView selectedRow = (DataRowView)dataGrid.SelectedItem;
                int id = Convert.ToInt32(selectedRow["Id"]);
                string name = selectedRow["FirstName"]?.ToString() ?? "Неизвестный";

                // Подтверждение удаления
                var result = MessageBox.Show($"Удалить клиента '{name}' (ID: {id})?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DataBase.DeleteTableClient(id);
                    MessageBox.Show("Клиент успешно удален!");

                    // Обновляем таблицу
                    btnClients_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        private void btnRentals_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBookings_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            DataTable rentalsTable = DataBase.SelectTableClient();
            dataGrid.ItemsSource = rentalsTable.DefaultView;
        }

        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btnEquipment_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

    }
}
