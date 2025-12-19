using SQLitePCL;
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

            DataBase.CreateTableClient();
        }
        private void CreateAllTables()
        {
            
        }

        private void btnQuickNewRental_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Создание нового комплекта Аренды, для Андрея");
        }

        private void btnQuickReturn_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Возврат комплекта оборудования, от Андрея");
        }

        private void btnRentals_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("База данных Арендованных комплектов");
        }

        private void btnBookings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("База данных Забронированных комплектов");

        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("База данных Зарегестрированных клиентов");
            DataTable clientsTable = DataBase.SelectTableClient();

            // Показываем в DataGrid (добавь DataGrid в XAML)
            dataGrid.ItemsSource = clientsTable.DefaultView;

        }

        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("База данных Поломанных снарежений");

        }
    }
}
