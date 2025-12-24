using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Класс, представляющий клиента системы аренды
    /// </summary>
    public class Client : INotifyPropertyChanged
    {
        private int id;
        private string lastName;
        private string firstName;
        private string secondName;
        private string documentNumber;
        private string phoneNumber;
        private DocumentType documentType;
        private DateTime registrationDate;

        /// <summary>
        /// Уникальный идентификатор клиента
        /// </summary>
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Имя клиента
        /// </summary>
        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Отчество клиента
        /// </summary>
        public string SecondName
        {
            get => secondName;
            set { secondName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Полное ФИО клиента
        /// </summary>
        public string FullName => $"{LastName} {FirstName} {SecondName}";

        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public DocumentType DocumentType
        {
            get => documentType;
            set { documentType = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber
        {
            get => documentNumber;
            set { documentNumber = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Полная информация о документе (тип и номер)
        /// </summary>
        public string DocumentInfo => $"{DocumentType}: {DocumentNumber}";

        /// <summary>
        /// Номер телефона клиента
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата регистрации клиента в системе
        /// </summary>
        public DateTime RegistrationDate
        {
            get => registrationDate;
            set { registrationDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Количество активных аренд у клиента
        /// </summary>
        public int ActiveRentalsCount { get; set; }

        /// <summary>
        /// Событие, возникающее при изменении свойства
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает событие PropertyChanged
        /// </summary>
        /// <param name="propertyName">Имя изменившегося свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Проверяет корректность данных клиента
        /// </summary>
        /// <returns>Кортеж с результатом валидации и сообщением об ошибке</returns>
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

        /// <summary>
        /// Возвращает строковое представление объекта Client
        /// </summary>
        /// <returns>Строка с информацией о клиенте</returns>
        public override string ToString()
        {
            return $"{FullName} | {DocumentInfo} | {PhoneNumber}";
        }

        /// <summary>
        /// Создает объект Client из строки данных DataRow
        /// </summary>
        /// <param name="row">Строка данных из базы данных</param>
        /// <returns>Объект Client</returns>
        public static Client FromDataRow(System.Data.DataRow row)
        {
            return new Client
            {
                Id = Convert.ToInt32(row["Id"]),
                LastName = row["LastName"].ToString(),
                FirstName = row["FirstName"].ToString(),
                SecondName = row["SecondName"].ToString(),
                DocumentType = (DocumentType)Convert.ToInt32(row["DocumentType"]),
                DocumentNumber = row["DocumentNumber"].ToString(),
                PhoneNumber = row["PhoneNumber"]?.ToString() ?? string.Empty,
                RegistrationDate = DateTime.Parse(row["RegistrationDate"].ToString())
            };
        }
    }
}