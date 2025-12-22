using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    public class Client
    {
        private int id;
        private string lastName;
        private string firstName;
        private string secondName;
        private string documentType;
        private string documentNumber;
        private string phoneNumber;
        private DateTime registrationDate;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        //Фамилия
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

        //Отчество
        public string SecondName
        {
            get => secondName;
            set { secondName = value; OnPropertyChanged(); }
        }

        //Полное ФИО
        public string FullName => $"{LastName} {FirstName} {SecondName}";

        //Тип документа: Passport, InternationalPassport, DriverLicense

        public string DocumentType
        {
            get => documentType;
            set { documentType = value; OnPropertyChanged(); }
        }

        //Номер документа
        public string DocumentNumber
        {
            get => documentNumber;
            set { documentNumber = value; OnPropertyChanged(); }
        }

        // Полные данные документа
        public string DocumentInfo => $"{DocumentType}: {DocumentNumber}";

        //Номер телефона
        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; OnPropertyChanged(); }
        }

        //Дата регистрации клиента в системе
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set { registrationDate = value; OnPropertyChanged(); }
        }

        //Количество активных аренд у клиента
        public int ActiveRentalsCount { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Валидация данных клиента
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                return (false, "Фамилия обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(FirstName))
                return (false, "Имя обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(SecondName))
                return (false, "Отчество обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(DocumentType))
                return (false, "Тип документа обязателен для заполнения");

            if (string.IsNullOrWhiteSpace(DocumentNumber))
                return (false, "Номер документа обязателен для заполнения");

            if (DocumentNumber.Length < 3)
                return (false, "Номер документа слишком короткий");

            // Валидация номера телефона (опционально)
            if (!string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length < 10)
                return (false, "Номер телефона должен содержать минимум 10 цифр");

            return (true, string.Empty);
        }

        //Создание клиента из DataRow
        public static Client FromDataRow(System.Data.DataRow row)
        {
            return new Client
            {
                Id = Convert.ToInt32(row["Id"]),
                LastName = row["LastName"].ToString(),
                FirstName = row["FirstName"].ToString(),
                SecondName = row["SecondName"].ToString(),
                DocumentNumber = row["Document"].ToString(),
                PhoneNumber = row["PhoneNumber"]?.ToString() ?? string.Empty,
                RegistrationDate = DateTime.Now
            };
        }

        //Преобразование в строку для отладки
        public override string ToString()
        {
            return $"{FullName} | {DocumentInfo} | {PhoneNumber}";
        }
    }

    //Типы документов, удостоверяющих личность
    public static class DocumentTypes
    {
        public const string Passport = "Паспорт";
        public const string InternationalPassport = "Загранпаспорт";
        public const string DriverLicense = "Водительское удостоверение";

        public static string[] GetAll() => new[] { Passport, InternationalPassport, DriverLicense };
    }
}
