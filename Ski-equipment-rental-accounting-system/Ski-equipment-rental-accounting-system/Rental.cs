using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Класс, представляющий аренду оборудования
    /// </summary>
    public class Rental : INotifyPropertyChanged
    {
        private int id;
        private int clientId;
        private int equipmentId;
        private DateTime startDate;
        private DateTime endDate;
        private decimal totalPrice;
        private RentalStatus status;
        private DateTime? actualReturnDate;
        private PaymentStatus paymentStatus;
        private PaymentMethod paymentMethod;
        private decimal discountAmount;
        private string discountCode;

        /// <summary>
        /// Уникальный идентификатор аренды
        /// </summary>
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public int ClientId
        {
            get => clientId;
            set { clientId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Идентификатор оборудования
        /// </summary>
        public int EquipmentId
        {
            get => equipmentId;
            set { equipmentId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата начала аренды
        /// </summary>
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата окончания аренды
        /// </summary>
        public DateTime EndDate
        {
            get => endDate;
            set { endDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Общая стоимость аренды
        /// </summary>
        public decimal TotalPrice
        {
            get => totalPrice;
            set { totalPrice = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Статус аренды
        /// </summary>
        public RentalStatus Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Фактическая дата возврата оборудования
        /// </summary>
        public DateTime? ActualReturnDate
        {
            get => actualReturnDate;
            set { actualReturnDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Статус оплаты
        /// </summary>
        public PaymentStatus PaymentStatus
        {
            get => paymentStatus;
            set { paymentStatus = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Способ оплаты
        /// </summary>
        public PaymentMethod PaymentMethod
        {
            get => paymentMethod;
            set { paymentMethod = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Сумма скидки
        /// </summary>
        public decimal DiscountAmount
        {
            get => discountAmount;
            set
            {
                discountAmount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FinalPrice));
            }
        }

        /// <summary>
        /// Код примененной скидки
        /// </summary>
        public string DiscountCode
        {
            get => discountCode;
            set { discountCode = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Количество дней аренды
        /// </summary>
        public int RentalDays
        {
            get => (int)(EndDate - StartDate).TotalDays;
        }

        /// <summary>
        /// Итоговая стоимость с учетом скидки
        /// </summary>
        public decimal FinalPrice
        {
            get => TotalPrice - DiscountAmount;
        }

        /// <summary>
        /// Проверяет, просрочена ли аренда
        /// </summary>
        /// <returns>true, если аренда просрочена, иначе false</returns>
        public bool IsOverdue()
        {
            if (Status == RentalStatus.Active && DateTime.Now > EndDate)
            {
                Status = RentalStatus.Overdue;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Рассчитывает общую стоимость аренды
        /// </summary>
        /// <param name="dailyPrice">Стоимость аренды за сутки</param>
        /// <returns>Общая стоимость аренды</returns>
        public decimal CalculateTotalPrice(decimal dailyPrice)
        {
            TotalPrice = dailyPrice * RentalDays;
            return TotalPrice;
        }

        /// <summary>
        /// Закрывает аренду с указанием даты возврата
        /// </summary>
        /// <param name="returnDate">Дата фактического возврата оборудования</param>
        public void CloseRental(DateTime returnDate)
        {
            ActualReturnDate = returnDate;
            Status = RentalStatus.Completed;
        }

        /// <summary>
        /// Применяет скидку к аренде
        /// </summary>
        /// <param name="discount">Объект скидки</param>
        public void ApplyDiscount(Discount discount)
        {
            if (discount == null || !discount.IsValidNow()) return;

            DiscountAmount = discount.CalculateDiscount(TotalPrice);
            DiscountCode = discount.Code;
            discount.IncrementUsage();
        }

        /// <summary>
        /// Снимает скидку
        /// </summary>
        public void RemoveDiscount()
        {
            DiscountAmount = 0;
            DiscountCode = null;
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
        /// Проверяет корректность данных аренды
        /// </summary>
        /// <returns>Кортеж с результатом валидации и сообщением об ошибке</returns>
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (StartDate >= EndDate)
                return (false, "Дата начала должна быть раньше даты окончания");

            if (TotalPrice <= 0)
                return (false, "Стоимость должна быть больше 0");

            if (DiscountAmount > TotalPrice)
                return (false, "Скидка не может превышать общую стоимость");

            return (true, string.Empty);
        }

        /// <summary>
        /// Создает объект Rental из строки данных DataRow
        /// </summary>
        /// <param name="row">Строка данных из базы данных</param>
        /// <returns>Объект Rental</returns>
        public static Rental FromDataRow(System.Data.DataRow row)
        {
            return new Rental
            {
                Id = Convert.ToInt32(row["Id"]),
                ClientId = Convert.ToInt32(row["ClientId"]),
                EquipmentId = Convert.ToInt32(row["EquipmentId"]),
                StartDate = DateTime.Parse(row["StartDate"].ToString()),
                EndDate = DateTime.Parse(row["EndDate"].ToString()),
                TotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                Status = (RentalStatus)Convert.ToInt32(row["Status"]),
                PaymentStatus = (PaymentStatus)Convert.ToInt32(row["PaymentStatus"]),
                PaymentMethod = (PaymentMethod)Convert.ToInt32(row["PaymentMethod"]),
                ActualReturnDate = row["ActualReturnDate"] != DBNull.Value ?
                                  DateTime.Parse(row["ActualReturnDate"].ToString()) : (DateTime?)null
            };
        }
    }
}