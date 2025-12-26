using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Data.Sqlite;

namespace Ski_equipment_rental_accounting_system
{
    public partial class AddRentalWindow : Window
    {
        private Client selectedClient = null;
        private List<Equipment> selectedEquipment = new List<Equipment>();
        private Discount appliedDiscount = null;
        private List<Client> allClients = new List<Client>();
        private List<Equipment> allEquipment = new List<Equipment>();
        private decimal totalPrice = 0;
        private int rentalDays = 1;
        private PaymentMethod selectedPaymentMethod = PaymentMethod.Cash;

        public AddRentalWindow()
        {
            try
            {
                InitializeComponent();

                // Инициализация DatePickers
                dpStartDate.SelectedDate = DateTime.Now;
                dpEndDate.SelectedDate = DateTime.Now.AddDays(1);

                // Загрузка данных
                LoadClients();
                LoadEquipment();

                // Таймер для обновления времени
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();

                UpdateRentalDays();
                UpdatePreview();

                // Устанавливаем начальный способ оплаты
                rbCash.IsChecked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании окна: {ex.Message}\n{ex.StackTrace}",
                              "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.DialogResult = false;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем обработчики для сенсорного взаимодействия
            SetupTouchHandlers();
        }

        private void SetupTouchHandlers()
        {
            try
            {
                // Увеличиваем область нажатия для всех кнопок
                foreach (var button in FindVisualChildren<Button>(this))
                {
                    button.TouchDown += (s, e) =>
                    {
                        var btn = s as Button;
                        AnimateButtonPress(btn);
                    };
                }
            }
            catch (Exception ex)
            {
                // Игнорируем ошибки в обработчиках касаний
                Console.WriteLine($"Ошибка в SetupTouchHandlers: {ex.Message}");
            }
        }

        private void AnimateButtonPress(Button button)
        {
            try
            {
                // Простая анимация нажатия
                var scaleTransform = new ScaleTransform(1, 1);
                button.RenderTransformOrigin = new Point(0.5, 0.5);
                button.RenderTransform = scaleTransform;

                var animation = new System.Windows.Media.Animation.DoubleAnimation
                {
                    To = 0.95,
                    Duration = TimeSpan.FromMilliseconds(100),
                    AutoReverse = true
                };

                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
            }
            catch
            {
                // Игнорируем ошибки анимации
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                txtDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            }
            catch
            {
                // Игнорируем ошибки таймера
            }
        }

        private void LoadClients()
        {
            try
            {
                allClients = DataBase.GetAllClients();
                if (allClients == null)
                {
                    allClients = new List<Client>();
                    MessageBox.Show("Не удалось загрузить клиентов. База данных может быть недоступна.",
                                  "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                listClients.ItemsSource = allClients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                allClients = new List<Client>();
                listClients.ItemsSource = allClients;
            }
        }

        private void LoadEquipment()
        {
            try
            {
                SqliteConnection connection = null;
                SqliteCommand command = null;
                SqliteDataReader reader = null;

                connection = new SqliteConnection($"Data Source={DataBase.connString}");
                connection.Open();

                command = new SqliteCommand("SELECT * FROM Equipment", connection);
                reader = command.ExecuteReader();

                allEquipment.Clear();
                while (reader.Read())
                {
                    try
                    {
                        Equipment equipment = new Equipment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            InventoryNumber = reader.GetString(reader.GetOrdinal("InventoryNumber")),
                            Type = (EquipmentType)reader.GetInt32(reader.GetOrdinal("Type")),
                            Size = reader.GetString(reader.GetOrdinal("Size")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            DailyRentalPrice = reader.GetDecimal(reader.GetOrdinal("DailyRentalPrice")),
                            Status = (EquipmentStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                        };
                        allEquipment.Add(equipment);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка чтения оборудования: {ex.Message}");
                    }
                }

                if (reader != null && !reader.IsClosed) reader.Close();
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();

                // Показываем только свободное оборудование по умолчанию
                FilterEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки оборудования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                allEquipment = new List<Equipment>();
                FilterEquipment();
            }
        }

        private void FilterEquipment()
        {
            try
            {
                var filtered = allEquipment.Where(e =>
                    (cmbStatusFilter.SelectedIndex == 0 ? e.Status == EquipmentStatus.Available : true) &&
                    (cmbEquipmentFilter.SelectedIndex <= 0 ||
                     (int)e.Type == cmbEquipmentFilter.SelectedIndex)
                ).ToList();

                listEquipment.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации оборудования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearchClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = txtSearchClient.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    listClients.ItemsSource = allClients;
                }
                else
                {
                    var filtered = allClients.Where(c =>
                        (c.FullName?.ToLower() ?? "").Contains(searchText) ||
                        (c.PhoneNumber?.ToLower() ?? "").Contains(searchText) ||
                        (c.DocumentNumber?.ToLower() ?? "").Contains(searchText)
                    ).ToList();

                    listClients.ItemsSource = filtered;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска клиентов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedClient = listClients.SelectedItem as Client;
                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора клиента: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedEquipment.Clear();
                foreach (Equipment item in listEquipment.SelectedItems)
                {
                    selectedEquipment.Add(item);
                }

                CalculateTotalPrice();
                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора оборудования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dpStartDate.SelectedDate.HasValue && dpEndDate.SelectedDate.HasValue)
                {
                    if (dpStartDate.SelectedDate.Value > dpEndDate.SelectedDate.Value)
                    {
                        dpEndDate.SelectedDate = dpStartDate.SelectedDate.Value;
                    }
                    UpdateRentalDays();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора даты начала: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dpStartDate.SelectedDate.HasValue && dpEndDate.SelectedDate.HasValue)
                {
                    if (dpEndDate.SelectedDate.Value < dpStartDate.SelectedDate.Value)
                    {
                        dpStartDate.SelectedDate = dpEndDate.SelectedDate.Value;
                    }
                    UpdateRentalDays();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора даты окончания: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateRentalDays()
        {
            try
            {
                if (dpStartDate.SelectedDate.HasValue && dpEndDate.SelectedDate.HasValue)
                {
                    rentalDays = (int)(dpEndDate.SelectedDate.Value - dpStartDate.SelectedDate.Value).TotalDays + 1;
                    if (rentalDays < 1) rentalDays = 1;

                    txtRentalDays.Text = $"{rentalDays} {GetDayWord(rentalDays)}";
                    CalculateTotalPrice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета дней аренды: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetDayWord(int days)
        {
            try
            {
                if (days % 10 == 1 && days % 100 != 11) return "день";
                if (days % 10 >= 2 && days % 10 <= 4 && (days % 100 < 10 || days % 100 >= 20)) return "дня";
                return "дней";
            }
            catch
            {
                return "дней";
            }
        }

        private void CalculateTotalPrice()
        {
            try
            {
                totalPrice = 0;

                foreach (var equipment in selectedEquipment)
                {
                    totalPrice += equipment.CalculateRentalPrice(rentalDays);
                }

                txtTotalPrice.Text = $"{totalPrice:N0} ₽";
                UpdateFinalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета стоимости: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateFinalPrice()
        {
            try
            {
                decimal discountAmount = appliedDiscount?.CalculateDiscount(totalPrice) ?? 0;
                decimal finalPrice = totalPrice - discountAmount;

                txtFinalPrice.Text = $"{finalPrice:N0} ₽";

                if (appliedDiscount != null)
                {
                    string discountType = appliedDiscount.Type == DiscountType.Percentage ?
                        $"{appliedDiscount.Value}%" : $"{appliedDiscount.Value:C}";
                    txtDiscountInfo.Text = $"Применена скидка: {appliedDiscount.Code}\n" +
                                         $"Тип: {discountType}\n" +
                                         $"Сумма скидки: {discountAmount:N0} ₽";
                    borderDiscountInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    borderDiscountInfo.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления итоговой стоимости: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtDiscountCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                btnApplyDiscount.IsEnabled = !string.IsNullOrWhiteSpace(txtDiscountCode.Text);
            }
            catch
            {
                // Игнорируем
            }
        }

        private void btnApplyDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = txtDiscountCode.Text.Trim();

                if (string.IsNullOrWhiteSpace(code))
                {
                    MessageBox.Show("Введите код промокода", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var discount = DataBase.ValidateDiscountCode(code);
                if (discount == null || !discount.IsValidNow())
                {
                    MessageBox.Show("Промокод недействителен или истек срок действия", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                appliedDiscount = discount;
                UpdateFinalPrice();
                txtDiscountCode.Text = "";

                MessageBox.Show($"Промокод успешно применен!\nСкидка: {discount.Value}" +
                              (discount.Type == DiscountType.Percentage ? "%" : " ₽"),
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка применения промокода: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemoveDiscount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                appliedDiscount = null;
                UpdateFinalPrice();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления скидки: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PaymentMethod_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbCash.IsChecked == true) selectedPaymentMethod = PaymentMethod.Cash;
                else if (rbCard.IsChecked == true) selectedPaymentMethod = PaymentMethod.Card;
                else if (rbOnline.IsChecked == true) selectedPaymentMethod = PaymentMethod.Online;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка выбора способа оплаты: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePreview()
        {
            try
            {
                var border = spPreview as Border;

                if (selectedClient == null && selectedEquipment.Count == 0)
                {
                    border.Child = txtPreviewEmpty;
                    return;
                }

                var stack = new StackPanel();

                if (selectedClient != null)
                {
                    stack.Children.Add(CreatePreviewItem("👤 Клиент:", selectedClient.FullName));
                    stack.Children.Add(CreatePreviewItem("📄 Документ:", selectedClient.DocumentInfo));
                    stack.Children.Add(CreatePreviewItem("📱 Телефон:", selectedClient.PhoneNumber));
                }

                if (selectedEquipment.Count > 0)
                {
                    stack.Children.Add(new Border
                    {
                        Height = 2,
                        Background = Brushes.LightGray,
                        Margin = new Thickness(0, 10, 0, 10)
                    });

                    stack.Children.Add(CreatePreviewItem("🎿 Оборудование:", $"{selectedEquipment.Count} шт."));

                    foreach (var equipment in selectedEquipment)
                    {
                        stack.Children.Add(CreatePreviewItem(
                            $"  • {equipment.InventoryNumber}:",
                            $"{equipment.Brand} {equipment.Model} ({equipment.Size}) - {equipment.DailyRentalPrice:C}/день"
                        ));
                    }
                }

                if (rentalDays > 0)
                {
                    stack.Children.Add(new Border
                    {
                        Height = 2,
                        Background = Brushes.LightGray,
                        Margin = new Thickness(0, 10, 0, 10)
                    });

                    stack.Children.Add(CreatePreviewItem("📅 Период:",
                        $"{dpStartDate.SelectedDate:dd.MM.yyyy} - {dpEndDate.SelectedDate:dd.MM.yyyy} ({rentalDays} {GetDayWord(rentalDays)})"));

                    decimal discountAmount = appliedDiscount?.CalculateDiscount(totalPrice) ?? 0;

                    stack.Children.Add(CreatePreviewItem("💰 Стоимость:", $"{totalPrice:N0} ₽"));

                    if (discountAmount > 0)
                    {
                        stack.Children.Add(CreatePreviewItem("🎫 Скидка:", $"-{discountAmount:N0} ₽"));
                        stack.Children.Add(CreatePreviewItem("💳 Итого:", $"{totalPrice - discountAmount:N0} ₽"));
                    }
                }

                border.Child = stack;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления предварительного просмотра: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private StackPanel CreatePreviewItem(string title, string value)
        {
            try
            {
                var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                panel.Children.Add(new TextBlock
                {
                    Text = title,
                    FontWeight = FontWeights.Bold,
                    Width = 200,
                    FontSize = 22
                });
                panel.Children.Add(new TextBlock
                {
                    Text = value ?? "",
                    FontSize = 22,
                    TextWrapping = TextWrapping.Wrap
                });
                return panel;
            }
            catch
            {
                return new StackPanel();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnNewClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var addClientWindow = new AddClientWindow();
                if (addClientWindow.ShowDialog() == true)
                {
                    LoadClients(); // Перезагружаем список клиентов
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания нового клиента: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbEquipmentFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FilterEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации оборудования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FilterEquipment();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации по статусу: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            await SaveRental(false);
        }

        private async void btnSaveAndPrint_Click(object sender, RoutedEventArgs e)
        {
            await SaveRental(true);
        }

        private async System.Threading.Tasks.Task SaveRental(bool printAfterSave)
        {
            // Валидация
            if (selectedClient == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedEquipment.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одно оборудование", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Укажите даты аренды", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (rentalDays < 1)
            {
                MessageBox.Show("Период аренды должен быть не менее 1 дня", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Показываем индикатор загрузки
            loadingOverlay.Visibility = Visibility.Visible;

            try
            {
                // Создаем аренду для каждого оборудования
                bool success = true;
                foreach (var equipment in selectedEquipment)
                {
                    var rental = new Rental
                    {
                        ClientId = selectedClient.Id,
                        EquipmentId = equipment.Id,
                        StartDate = dpStartDate.SelectedDate.Value,
                        EndDate = dpEndDate.SelectedDate.Value,
                        TotalPrice = equipment.CalculateRentalPrice(rentalDays),
                        Status = RentalStatus.Active,
                        PaymentStatus = PaymentStatus.Pending,
                        PaymentMethod = selectedPaymentMethod
                    };

                    // Применяем скидку (если есть)
                    if (appliedDiscount != null)
                    {
                        rental.ApplyDiscount(appliedDiscount);
                    }

                    // Сохраняем в БД
                    var result = await System.Threading.Tasks.Task.Run(() => SaveRentalToDatabase(rental));
                    if (!result)
                    {
                        success = false;
                        break;
                    }

                    // Обновляем статус оборудования
                    result = await System.Threading.Tasks.Task.Run(() => UpdateEquipmentStatus(equipment.Id, EquipmentStatus.Rented));
                    if (!result)
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    MessageBox.Show($"Аренда успешно создана!\n" +
                                  $"Клиент: {selectedClient.FullName}\n" +
                                  $"Оборудование: {selectedEquipment.Count} шт.\n" +
                                  $"Сумма: {totalPrice:N0} ₽",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (printAfterSave)
                    {
                        // TODO: Реализовать печать PDF
                        MessageBox.Show("Печать будет реализована в следующем обновлении", "Информация",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось сохранить аренду. Проверьте подключение к базе данных.",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения аренды: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                loadingOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private bool SaveRentalToDatabase(Rental rental)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={DataBase.connString}");
                connection.Open();

                // Проверяем, есть ли уже аренда на это оборудование
                string checkSql = "SELECT COUNT(*) FROM Rental WHERE EquipmentId = @equipmentId AND Status = 1";
                var checkCommand = new SqliteCommand(checkSql, connection);
                checkCommand.Parameters.AddWithValue("@equipmentId", rental.EquipmentId);
                var count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show($"Оборудование ID {rental.EquipmentId} уже арендовано!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                string sql = @"INSERT INTO Rental 
                             (ClientId, EquipmentId, StartDate, EndDate, TotalPrice, Status, 
                              PaymentStatus, PaymentMethod, DiscountCode, DiscountAmount) 
                             VALUES (@clientId, @equipmentId, @startDate, @endDate, @totalPrice, @status,
                                     @paymentStatus, @paymentMethod, @discountCode, @discountAmount)";

                command = new SqliteCommand(sql, connection);

                command.Parameters.AddWithValue("@clientId", rental.ClientId);
                command.Parameters.AddWithValue("@equipmentId", rental.EquipmentId);
                command.Parameters.AddWithValue("@startDate", rental.StartDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@endDate", rental.EndDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@totalPrice", rental.TotalPrice);
                command.Parameters.AddWithValue("@status", (int)rental.Status);
                command.Parameters.AddWithValue("@paymentStatus", (int)rental.PaymentStatus);
                command.Parameters.AddWithValue("@paymentMethod", (int)rental.PaymentMethod);
                command.Parameters.AddWithValue("@discountCode", rental.DiscountCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@discountAmount", rental.DiscountAmount);

                command.ExecuteNonQuery();

                // Обновляем счетчик использований скидки
                if (!string.IsNullOrEmpty(rental.DiscountCode))
                {
                    var discount = DataBase.ValidateDiscountCode(rental.DiscountCode);
                    if (discount != null)
                    {
                        DataBase.UpdateDiscountUsage(discount.Id, discount.UsageCount + 1);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения аренды в БД: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                if (command != null) command.Dispose();
                if (connection != null && connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
        }

        private bool UpdateEquipmentStatus(int equipmentId, EquipmentStatus status)
        {
            SqliteConnection connection = null;
            SqliteCommand command = null;

            try
            {
                connection = new SqliteConnection($"Data Source={DataBase.connString}");
                connection.Open();

                command = new SqliteCommand("UPDATE Equipment SET Status = @status WHERE Id = @id", connection);
                command.Parameters.AddWithValue("@status", (int)status);
                command.Parameters.AddWithValue("@id", equipmentId);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления статуса оборудования: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
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

        // Вспомогательный метод для поиска дочерних элементов
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}