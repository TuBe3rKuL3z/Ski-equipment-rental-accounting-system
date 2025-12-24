using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Класс, представляющий оборудование для аренды
    /// </summary>
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

        /// <summary>
        /// Уникальный идентификатор оборудования
        /// </summary>
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Инвентарный номер оборудования
        /// </summary>
        public string InventoryNumber
        {
            get => inventoryNumber;
            set { inventoryNumber = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Тип снаряжения
        /// </summary>
        public EquipmentType Type
        {
            get => type;
            set { type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Размер/ростовка оборудования
        /// </summary>
        public string Size
        {
            get => size;
            set { size = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Бренд производителя
        /// </summary>
        public string Brand
        {
            get => brand;
            set { brand = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Модель оборудования
        /// </summary>
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Стоимость аренды в сутки
        /// </summary>
        public decimal DailyRentalPrice
        {
            get => dailyRentalPrice;
            set { dailyRentalPrice = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Статус оборудования
        /// </summary>
        public EquipmentStatus Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Изображение оборудования
        /// </summary>
        public byte[] Image
        {
            get => image;
            set { image = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Проверяет, доступно ли оборудование для аренды
        /// </summary>
        /// <returns>true, если оборудование доступно, иначе false</returns>
        public bool IsAvailable()
        {
            return Status == EquipmentStatus.Available;
        }

        /// <summary>
        /// Проверяет, находится ли оборудование на обслуживании
        /// </summary>
        /// <returns>true, если оборудование на обслуживании, иначе false</returns>
        public bool IsUnderMaintenance()
        {
            return Status == EquipmentStatus.UnderMaintenance;
        }

        /// <summary>
        /// Рассчитывает стоимость аренды на указанное количество дней
        /// </summary>
        /// <param name="days">Количество дней аренды</param>
        /// <returns>Общая стоимость аренды</returns>
        public decimal CalculateRentalPrice(int days)
        {
            return DailyRentalPrice * days;
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
        /// Проверяет корректность данных оборудования
        /// </summary>
        /// <returns>Кортеж с результатом валидации и сообщением об ошибке</returns>
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

        /// <summary>
        /// Создает объект Equipment из строки данных DataRow
        /// </summary>
        /// <param name="row">Строка данных из базы данных</param>
        /// <returns>Объект Equipment</returns>
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