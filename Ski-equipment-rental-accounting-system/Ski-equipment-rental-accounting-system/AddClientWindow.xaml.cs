using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Ski_equipment_rental_accounting_system
{
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
            LoadDocumentTypes();
        }

        private void LoadDocumentTypes()
        {
            var documentTypes = new Dictionary<int, string>
            {
                { (int)DocumentType.Passport, "Паспорт РФ" },
                { (int)DocumentType.InternationalPassport, "Загранпаспорт" },
                { (int)DocumentType.DriverLicense, "Водительское удостоверение" }
            };

            cmbDocumentType.ItemsSource = documentTypes;
            cmbDocumentType.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(txtSecondName.Text) ||
                    string.IsNullOrWhiteSpace(txtDocumentNumber.Text))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем выбранный тип документа
                var selectedDoc = (KeyValuePair<int, string>)cmbDocumentType.SelectedItem;
                DocumentType docType = (DocumentType)selectedDoc.Key;

                // Используем НОВЫЙ объектно-ориентированный метод
                Client newClient = new Client
                {
                    LastName = txtLastName.Text,
                    FirstName = txtFirstName.Text,
                    SecondName = txtSecondName.Text,
                    DocumentType = docType,
                    DocumentNumber = txtDocumentNumber.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    RegistrationDate = DateTime.Now
                };

                // Валидация через метод класса Client
                var validation = newClient.Validate();
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage, "Ошибка валидации",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Используем новый метод из DataBase
                DataBase.InsertClient(newClient);

                MessageBox.Show("Клиент успешно добавлен!", "Успех",
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
    }
}