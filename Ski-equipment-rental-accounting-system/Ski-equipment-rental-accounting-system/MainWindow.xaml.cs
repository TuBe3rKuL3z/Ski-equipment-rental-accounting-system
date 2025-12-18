using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

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
        }
        private void btnQuickNewRental_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnQuickReturn_Click(object sender, RoutedEventArgs e)
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
            MessageBox.Show("База данных Зарегестрированных клиентов");

        }

        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("База данных Поломанных снарежений");

        }
    }
    public class EmojiExtractorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // Извлекаем эмодзи (первый символ или первые 2 символа)
                var emojiMatch = Regex.Match(text, @"^[\p{So}\p{Cs}]+");
                return emojiMatch.Success ? emojiMatch.Value : "📋";
            }
            return "📋";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TextExtractorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                // Удаляем эмодзи из начала строки
                return Regex.Replace(text, @"^[\p{So}\p{Cs}]+", "").Trim();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
