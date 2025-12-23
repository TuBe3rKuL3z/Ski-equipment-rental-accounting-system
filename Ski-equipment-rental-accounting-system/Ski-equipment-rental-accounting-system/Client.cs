using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    public class Client : INotifyPropertyChanged
    {
        private int id;
        private string lastName;
        private string firstName;
        private string secondName;
        private string documentNumber;
        private string phoneNumber;
        private DocumentType documentType; // Изменяем на Enum
        private DateTime registrationDate;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        // Фамилия
        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        // Имя
        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        // Отчество
        public string SecondName
        {
            get => secondName;
            set { secondName = value; OnPropertyChanged(); }
        }

        // Полное ФИО
        public string FullName => $"{LastName} {FirstName} {SecondName}";

        // Тип документа (используем Enum)
        public DocumentType DocumentType
        {
            get => documentType;
            set { documentType = value; OnPropertyChanged(); }
        }

        // Номер документа
        public string DocumentNumber
        {
            get => documentNumber;
            set { documentNumber = value; OnPropertyChanged(); }
        }

        // Полные данные документа
        public string DocumentInfo => $"{DocumentType}: {DocumentNumber}";

        // Номер телефона
        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; OnPropertyChanged(); }
        }

        // Дата регистрации
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set { registrationDate = value; OnPropertyChanged(); }
        }

        // Количество активных аренд
        public int ActiveRentalsCount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Валидация данных клиента
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                return (false, "Фамилия обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(FirstName))
                return (false, "Имя обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(SecondName))
                return (false, "Отчество обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(DocumentNumber))
                return (false, "Номер документа обязателен для заполнения");

            if (DocumentNumber.Length < 3)
                return (false, "Номер документа слишком короткий");

            if (!string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length < 10)
                return (false, "Номер телефона должен содержать минимум 10 цифр");

            return (true, string.Empty);
        }

        // Конвертация в строку для отладки
        public override string ToString()
        {
            return $"{FullName} | {DocumentInfo} | {PhoneNumber}";
        }

        // Создание клиента из DataRow
        public static Client FromDataRow(System.Data.DataRow row)
        {
            return new Client
            {
                Id = Convert.ToInt32(row["Id"]),
                LastName = row["LastName"].ToString(),
                FirstName = row["FirstName"].ToString(),
                SecondName = row["SecondName"].ToString(),
                DocumentType = (DocumentType)Convert.ToInt32(row["DocumentType"]), // Получаем Enum из БД
                DocumentNumber = row["DocumentNumber"].ToString(),
                PhoneNumber = row["PhoneNumber"]?.ToString() ?? string.Empty,
                RegistrationDate = DateTime.Parse(row["RegistrationDate"].ToString())
            };
        }
    }
}