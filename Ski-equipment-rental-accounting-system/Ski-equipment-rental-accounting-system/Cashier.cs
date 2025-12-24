using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Класс, представляющий кассира системы
    /// </summary>
    public class Cashier : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string password;
        private DateTime hireDate;
        private bool isActive;

        /// <summary>
        /// Уникальный идентификатор кассира
        /// </summary>
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Имя кассира
        /// </summary>
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Пароль для входа в систему (в реальном приложении должен быть хеширован)
        /// </summary>
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата приема на работу
        /// </summary>
        public DateTime HireDate
        {
            get => hireDate;
            set { hireDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Флаг активности учетной записи кассира
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Аутентифицирует кассира по паролю
        /// </summary>
        /// <param name="inputPassword">Введенный пароль</param>
        /// <returns>true, если аутентификация успешна, иначе false</returns>
        public bool Authenticate(string inputPassword)
        {
            return Password == inputPassword && IsActive;
        }

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
        /// Проверяет корректность данных кассира
        /// </summary>
        /// <returns>Кортеж с результатом валидации и сообщением об ошибке</returns>
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return (false, "Имя кассира обязательно");

            if (string.IsNullOrWhiteSpace(Password))
                return (false, "Пароль обязателен");

            if (Password.Length < 4)
                return (false, "Пароль должен содержать минимум 4 символа");

            return (true, string.Empty);
        }

        /// <summary>
        /// Создает объект Cashier из строки данных DataRow
        /// </summary>
        /// <param name="row">Строка данных из базы данных</param>
        /// <returns>Объект Cashier</returns>
        public static Cashier FromDataRow(System.Data.DataRow row)
        {
            return new Cashier
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Password = row["Password"].ToString(),
                HireDate = DateTime.Parse(row["HireDate"].ToString()),
                IsActive = Convert.ToBoolean(row["IsActive"])
            };
        }
    }
}