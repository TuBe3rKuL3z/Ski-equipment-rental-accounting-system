using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
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

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        // ID клиента
        public int ClientId
        {
            get => clientId;
            set { clientId = value; OnPropertyChanged(); }
        }

        // ID оборудования
        public int EquipmentId
        {
            get => equipmentId;
            set { equipmentId = value; OnPropertyChanged(); }
        }

        // Дата начала аренды
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; OnPropertyChanged(); }
        }

        // Дата окончания аренды
        public DateTime EndDate
        {
            get => endDate;
            set { endDate = value; OnPropertyChanged(); }
        }

        // Общая стоимость
        public decimal TotalPrice
        {
            get => totalPrice;
            set { totalPrice = value; OnPropertyChanged(); }
        }

        // Статус аренды
        public RentalStatus Status
        {
            get => status;
            set { status = value; OnPropertyChanged(); }
        }

        // Фактическая дата возврата
        public DateTime? ActualReturnDate
        {
            get => actualReturnDate;
            set { actualReturnDate = value; OnPropertyChanged(); }
        }

        // Статус оплаты
        public PaymentStatus PaymentStatus
        {
            get => paymentStatus;
            set { paymentStatus = value; OnPropertyChanged(); }
        }

        // Способ оплаты
        public PaymentMethod PaymentMethod
        {
            get => paymentMethod;
            set { paymentMethod = value; OnPropertyChanged(); }
        }

        // Количество дней аренды
        public int RentalDays
        {
            get => (int)(EndDate - StartDate).TotalDays;
        }

        // Проверка просрочки
        public bool IsOverdue()
        {
            if (Status == RentalStatus.Active && DateTime.Now > EndDate)
            {
                Status = RentalStatus.Overdue;
                return true;
            }
            return false;
        }

        // Расчет общей стоимости
        public decimal CalculateTotalPrice(decimal dailyPrice)
        {
            TotalPrice = dailyPrice * RentalDays;
            return TotalPrice;
        }

        // Закрытие аренды
        public void CloseRental(DateTime returnDate)
        {
            ActualReturnDate = returnDate;
            Status = RentalStatus.Completed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Валидация аренды
        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (StartDate >= EndDate)
                return (false, "Дата начала должна быть раньше даты окончания");

            if (TotalPrice <= 0)
                return (false, "Стоимость должна быть больше 0");

            return (true, string.Empty);
        }

        // Создание из DataRow
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