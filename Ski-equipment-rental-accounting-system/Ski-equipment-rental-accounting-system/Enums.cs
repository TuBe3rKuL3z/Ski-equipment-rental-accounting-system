using System;

namespace Ski_equipment_rental_accounting_system
{
    /// <summary>
    /// Все перечисления системы учета аренды горнолыжного снаряжения
    /// </summary>

    /// <summary>
    /// Типы горнолыжного снаряжения
    /// </summary>
    public enum EquipmentType
    {
        /// <summary>
        /// Лыжи
        /// </summary>
        Skis = 1,

        /// <summary>
        /// Ботинки лыжные
        /// </summary>
        BootsSkis = 2,

        /// <summary>
        /// Сноуборд
        /// </summary>
        Snowboard = 3,

        /// <summary>
        /// Ботинки для сноуборда
        /// </summary>
        BootsSnowboard = 4,

        /// <summary>
        /// Шлем
        /// </summary>
        Helmet = 5,

        /// <summary>
        /// Костюм
        /// </summary>
        Suit = 6,

        /// <summary>
        /// Палки лыжные
        /// </summary>
        SkiPoles = 7,

        /// <summary>
        /// Очки
        /// </summary>
        Goggles = 8
    }

    /// <summary>
    /// Статусы оборудования
    /// </summary>
    public enum EquipmentStatus
    {
        /// <summary>
        /// Свободно для аренды
        /// </summary>
        Available = 1,

        /// <summary>
        /// Арендовано клиентом
        /// </summary>
        Rented = 2,

        /// <summary>
        /// На обслуживании/ремонте
        /// </summary>
        UnderMaintenance = 3,

        /// <summary>
        /// Списано/неисправно
        /// </summary>
        OutOfService = 4
    }

    /// <summary>
    /// Статусы аренды
    /// </summary>
    public enum RentalStatus
    {
        /// <summary>
        /// Активная аренда
        /// </summary>
        Active = 1,

        /// <summary>
        /// Завершена
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Просрочена
        /// </summary>
        Overdue = 3,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled = 4
    }

    /// <summary>
    /// Типы документов, удостоверяющих личность
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Паспорт РФ
        /// </summary>
        Passport = 1,

        /// <summary>
        /// Загранпаспорт
        /// </summary>
        InternationalPassport = 2,

        /// <summary>
        /// Водительское удостоверение
        /// </summary>
        DriverLicense = 3
    }

    /// <summary>
    /// Статусы бронирования
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Подтверждена
        /// </summary>
        Confirmed = 1,

        /// <summary>
        /// Ожидает подтверждения
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Отменена
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// Завершена
        /// </summary>
        Completed = 4
    }

    /// <summary>
    /// Сезоны для отчетности
    /// </summary>
    public enum Season
    {
        /// <summary>
        /// Зима 2024
        /// </summary>
        Winter2024 = 1,

        /// <summary>
        /// Лето 2024
        /// </summary>
        Summer2024 = 2,

        /// <summary>
        /// Зима 2025
        /// </summary>
        Winter2025 = 3,

        /// <summary>
        /// Лето 2025
        /// </summary>
        Summer2025 = 4
    }

    /// <summary>
    /// Форматы отчетов
    /// </summary>
    public enum ReportFormat
    {
        /// <summary>
        /// Таблица в приложении
        /// </summary>
        Table = 1,

        /// <summary>
        /// Excel файл
        /// </summary>
        Excel = 2,

        /// <summary>
        /// PDF документ
        /// </summary>
        PDF = 3,

        /// <summary>
        /// Печатная форма
        /// </summary>
        Print = 4
    }

    /// <summary>
    /// Методы сортировки отчетов
    /// </summary>
    public enum SortBy
    {
        /// <summary>
        /// По типу снаряжения
        /// </summary>
        EquipmentType = 1,

        /// <summary>
        /// По стоимости аренды
        /// </summary>
        RentalPrice = 2,

        /// <summary>
        /// По дате выдачи
        /// </summary>
        StartDate = 3,

        /// <summary>
        /// По клиенту
        /// </summary>
        ClientName = 4,

        /// <summary>
        /// По статусу
        /// </summary>
        Status = 5
    }

    /// <summary>
    /// Статусы платежей
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Оплачено полностью
        /// </summary>
        Paid = 1,

        /// <summary>
        /// Ожидает оплаты
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Частично оплачено
        /// </summary>
        Partial = 3,

        /// <summary>
        /// Возврат средств
        /// </summary>
        Refunded = 4,

        /// <summary>
        /// Просрочен платеж
        /// </summary>
        Overdue = 5
    }

    /// <summary>
    /// Методы оплаты
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Наличные
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Банковская карта
        /// </summary>
        Card = 2,

        /// <summary>
        /// Безналичный расчет
        /// </summary>
        BankTransfer = 3,

        /// <summary>
        /// Онлайн платеж
        /// </summary>
        Online = 4
    }
}