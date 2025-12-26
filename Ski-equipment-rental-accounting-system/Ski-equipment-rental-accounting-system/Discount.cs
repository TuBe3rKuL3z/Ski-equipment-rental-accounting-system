using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Тип скидки
    /// </summary>
    public enum DiscountType
    {
        /// <summary>
        /// Процентная скидка
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// Фиксированная сумма
        /// </summary>
        FixedAmount = 2
    }

    /// <summary>
    /// Класс, представляющий скидку/промокод
    /// </summary>
    public class Discount : INotifyPropertyChanged
    {
        private int id;
        private string code;
        private DiscountType type;
        private decimal value;
        private DateTime validFrom;
        private DateTime validTo;
        private bool isActive;
        private string description;
        private int usageCount;

        /// <summary>
        /// Уникальный идентификатор скидки
        /// </summary>
        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Код промокода
        /// </summary>
        public string Code
        {
            get => code;
            set { code = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Тип скидки
        /// </summary>
        public DiscountType Type
        {
            get => type;
            set { type = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Значение скидки (процент или сумма)
        /// </summary>
        public decimal Value
        {
            get => value;
            set { this.value = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public DateTime ValidFrom
        {
            get => validFrom;
            set { validFrom = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public DateTime ValidTo
        {
            get => validTo;
            set { validTo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Активна ли скидка
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Описание скидки
        /// </summary>
        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Количество использований
        /// </summary>
        public int UsageCount
        {
            get => usageCount;
            set { usageCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Проверяет, действительна ли скидка в указанную дату
        /// </summary>
        /// <param name="date">Дата для проверки</param>
        /// <returns>true, если скидка действительна</returns>
        public bool IsValid(DateTime date)
        {
            return IsActive && date >= ValidFrom && date <= ValidTo;
        }

        /// <summary>
        /// Проверяет, действительна ли скидка сейчас
        /// </summary>
        /// <returns>true, если скидка действительна</returns>
        public bool IsValidNow()
        {
            return IsValid(DateTime.Now);
        }

        /// <summary>
        /// Рассчитывает сумму скидки для указанной стоимости
        /// </summary>
        /// <param name="originalPrice">Исходная стоимость</param>
        /// <returns>Сумма скидки</returns>
        public decimal CalculateDiscount(decimal originalPrice)
        {
            if (!IsValidNow()) return 0;

            if (Type == DiscountType.Percentage)
            {
                return originalPrice * (Value / 100m);
            }
            else // FixedAmount
            {
                return Value;
            }
        }

        /// <summary>
        /// Увеличивает счетчик использований
        /// </summary>
        public void IncrementUsage()
        {
            UsageCount++;
            OnPropertyChanged(nameof(UsageCount));
        }

        /// <summary>
        /// Возвращает строковое представление скидки
        /// </summary>
        public override string ToString()
        {
            string typeStr = Type == DiscountType.Percentage ? $"{Value}%" : $"{Value:C}";
            return $"{Code}: {typeStr} ({Description})";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}