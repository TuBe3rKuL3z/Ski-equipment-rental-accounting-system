using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    public class Equipment : INotifyPropertyChanged
    {
        private int id;
        private string inventoryNumber;
        private EquipmentType type;
        private string size;
        private string brand;
        private string model;
        private decimal dailyRentalPrice;
        private EquipmentStatus status;
        private byte[] image;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        // Инвентарный номер
        public string InventoryNumber
        {
            get => inventoryNumber;
            set { inventoryNumber = value; OnPropertyChanged(); }
        }

        // Тип снаряжения
        public EquipmentType Type
        {
            get => type;
            set { type = value; OnPropertyChanged(); }
        }

        // Размер/ростовка
        public string Size
        {
            get => size;
            set { size = value; OnPropertyChanged(); }
        }

        // Бренд
        public string Brand
        {
            get => brand;
            set { brand = value; OnPropertyChanged(); }
        }

        // Модель
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged(); }
        }

        // Стоимость аренды в сутки
        public decimal DailyRentalPrice
        {
            get => dailyRentalPrice;
            set { dailyRentalPrice = value; OnPropertyChanged(); }
        }

        // Статус оборудования
        public EquipmentStatus Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        // Изображение
        public byte[] Image
        {
            get => image;
            set { image = value; OnPropertyChanged(); }
        }

        // Проверка доступности
        public bool IsAvailable()
        {
            return Status == EquipmentStatus.Available;
        }

        // Проверка на обслуживании
        public bool IsUnderMaintenance()
        {
            return Status == EquipmentStatus.UnderMaintenance;
        }

        // Расчет стоимости аренды на N дней
        public decimal CalculateRentalPrice(int days)
        {
            return DailyRentalPrice * days;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Валидация оборудования
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrWhiteSpace(InventoryNumber))
                return (false, "Инвентарный номер обязателен");

            if (string.IsNullOrWhiteSpace(Size))
                return (false, "Размер обязателен");

            if (string.IsNullOrWhiteSpace(Brand))
                return (false, "Бренд обязателен");

            if (string.IsNullOrWhiteSpace(Model))
                return (false, "Модель обязательна");

            if (DailyRentalPrice <= 0)
                return (false, "Стоимость аренды должна быть больше 0");

            return (true, string.Empty);
        }

        // Создание из DataRow
        public static Equipment FromDataRow(System.Data.DataRow row)
        {
            return new Equipment
            {
                Id = Convert.ToInt32(row["Id"]),
                InventoryNumber = row["InventoryNumber"].ToString(),
                Type = (EquipmentType)Convert.ToInt32(row["Type"]),
                Size = row["Size"].ToString(),
                Brand = row["Brand"].ToString(),
                Model = row["Model"].ToString(),
                DailyRentalPrice = Convert.ToDecimal(row["DailyRentalPrice"]),
                Status = (EquipmentStatus)Convert.ToInt32(row["Status"]),
                Image = row["Image"] as byte[] ?? null
            };
        }
    }
}