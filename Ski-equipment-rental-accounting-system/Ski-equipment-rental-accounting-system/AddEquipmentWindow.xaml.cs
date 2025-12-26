using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Ski_equipment_rental_accounting_system
{
    public partial class AddEquipmentWindow : Window
    {
        private byte[] imageData = null;

        public AddEquipmentWindow()
        {
            InitializeComponent();
            LoadEquipmentTypes();
            LoadStatuses();
        }

        private void LoadEquipmentTypes()
        {
            var equipmentTypes = new Dictionary<int, string>
            {
                { (int)EquipmentType.Skis, "Лыжи" },
                { (int)EquipmentType.BootsSkis, "Ботинки лыжные" },
                { (int)EquipmentType.Snowboard, "Сноуборд" },
                { (int)EquipmentType.BootsSnowboard, "Ботинки для сноуборда" },
                { (int)EquipmentType.Helmet, "Шлем" },
                { (int)EquipmentType.Suit, "Костюм" },
                { (int)EquipmentType.SkiPoles, "Палки лыжные" },
                { (int)EquipmentType.Goggles, "Очки" }
            };

            cmbEquipmentType.ItemsSource = equipmentTypes;
            cmbEquipmentType.SelectedIndex = 0;
        }

        private void LoadStatuses()
        {
            var statuses = new Dictionary<int, string>
            {
                { (int)EquipmentStatus.Available, "Свободно для аренды" },
                { (int)EquipmentStatus.Rented, "Арендовано клиентом" },
                { (int)EquipmentStatus.UnderMaintenance, "На обслуживании/ремонте" },
                { (int)EquipmentStatus.OutOfService, "Списано/неисправно" }
            };

            cmbStatus.ItemsSource = statuses;
            cmbStatus.SelectedIndex = 0;
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите изображение оборудования"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Читаем файл как массив байтов
                    imageData = File.ReadAllBytes(openFileDialog.FileName);

                    // Обновляем информацию
                    FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                    txtImageInfo.Text = $"Файл: {fileInfo.Name}\n" +
                                      $"Размер: {fileInfo.Length / 1024} KB\n" +
                                      $"Разрешение: {GetImageDimensions(openFileDialog.FileName)}";

                    // Показываем кнопку предпросмотра
                    btnPreviewImage.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GetImageDimensions(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var bitmapFrame = BitmapFrame.Create(stream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    return $"{bitmapFrame.PixelWidth}x{bitmapFrame.PixelHeight}";
                }
            }
            catch
            {
                return "Неизвестно";
            }
        }

        private void btnPreviewImage_Click(object sender, RoutedEventArgs e)
        {
            if (imageData != null)
            {
                // Создаем окно предпросмотра
                var previewWindow = new Window
                {
                    Title = "Предпросмотр изображения",
                    Width = 600,
                    Height = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                var image = new System.Windows.Controls.Image();
                using (var ms = new MemoryStream(imageData))
                {
                    var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    image.Source = bitmap;
                }

                previewWindow.Content = new ScrollViewer
                {
                    Content = image,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };

                previewWindow.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(txtInventoryNumber.Text))
                {
                    MessageBox.Show("Введите инвентарный номер!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSize.Text))
                {
                    MessageBox.Show("Введите размер/ростовку!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtBrand.Text))
                {
                    MessageBox.Show("Введите бренд!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtModel.Text))
                {
                    MessageBox.Show("Введите модель!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtDailyPrice.Text, out decimal dailyPrice) || dailyPrice <= 0)
                {
                    MessageBox.Show("Введите корректную стоимость аренды (больше 0)!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем выбранные значения
                var selectedType = (KeyValuePair<int, string>)cmbEquipmentType.SelectedItem;
                var selectedStatus = (KeyValuePair<int, string>)cmbStatus.SelectedItem;

                // Используем объектно-ориентированный подход
                Equipment newEquipment = new Equipment
                {
                    InventoryNumber = txtInventoryNumber.Text,
                    Type = (EquipmentType)selectedType.Key,
                    Size = txtSize.Text,
                    Brand = txtBrand.Text,
                    Model = txtModel.Text,
                    DailyRentalPrice = dailyPrice,
                    Status = (EquipmentStatus)selectedStatus.Key,
                    Image = imageData
                };

                // Валидация через метод класса Equipment
                var validation = newEquipment.Validate();
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage, "Ошибка валидации",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Сохраняем в БД
                SaveEquipmentToDatabase(newEquipment);

                MessageBox.Show("Оборудование успешно добавлено!", "Успех",
                              MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveEquipmentToDatabase(Equipment equipment)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={DataBase.connString}");
                connection.Open();

                string sql = @"INSERT INTO Equipment 
                             (InventoryNumber, Type, Size, Brand, Model, DailyRentalPrice, Status, Image) 
                             VALUES (@inventoryNumber, @type, @size, @brand, @model, @dailyPrice, @status, @image)";

                command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("@inventoryNumber", equipment.InventoryNumber);
                command.Parameters.AddWithValue("@type", (int)equipment.Type);
                command.Parameters.AddWithValue("@size", equipment.Size);
                command.Parameters.AddWithValue("@brand", equipment.Brand);
                command.Parameters.AddWithValue("@model", equipment.Model);
                command.Parameters.AddWithValue("@dailyPrice", equipment.DailyRentalPrice);
                command.Parameters.AddWithValue("@status", (int)equipment.Status);
                command.Parameters.AddWithValue("@image", equipment.Image ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения оборудования: {ex.Message}");
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void txtDailyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешаем только цифры и запятую/точку
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c) && c != ',' && c != '.')
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
