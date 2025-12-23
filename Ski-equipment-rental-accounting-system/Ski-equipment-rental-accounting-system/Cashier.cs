using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    public class Cashier : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string password;
        private DateTime hireDate;
        private bool isActive;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        // Имя кассира
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        // Пароль (в реальном приложении должен быть хеширован)
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        // Дата приема на работу
        public DateTime HireDate
        {
            get => hireDate;
            set { hireDate = value; OnPropertyChanged(); }
        }

        // Активен ли кассир
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(); }
        }

        // Аутентификация
        public bool Authenticate(string inputPassword)
        {
            return Password == inputPassword && IsActive;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Валидация кассира
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

        // Создание из DataRow
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