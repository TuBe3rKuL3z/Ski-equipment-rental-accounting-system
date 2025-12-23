using System;

namespace Ski_equipment_rental_accounting_system
{
    //Все перечисления системы учета аренды горнолыжного снаряжения

    //Типы горнолыжного снаряжения
    public enum EquipmentType
    {
        //Лыжи
        Skis = 1,

        //Ботинки лыжные
        BootsSkis = 2,

        /// <summary>Сноуборд</summary>
        Snowboard = 3,

        //Ботинки для сноуборда
        BootsSnowboard = 4,

        //Шлем
        Helmet = 5,

        //Костюм
        Suit = 6,

        //Палки лыжные>
        SkiPoles = 7,

        //Очки
        Goggles = 8
    }

    //Статусы оборудования
    public enum EquipmentStatus
    {
        //Свободно
        Available = 1,

        //Арендовано
        Rented = 2,

        //На обслуживании
        UnderMaintenance = 3,

        //Списано/неисправно
        OutOfService = 4
    }

    //Статусы аренды
    public enum RentalStatus
    {
        //Активная аренда
        Active = 1,

        //Завершена
        Completed = 2,

        //Просрочена
        Overdue = 3,

        //Отменена
        Cancelled = 4
    }

    //Типы документов, удостоверяющих личность
    public enum DocumentType
    {
        //Паспорт РФ
        Passport = 1,

        //Загранпаспорт
        InternationalPassport = 2,

        //Водительское удостоверение
        DriverLicense = 3
    }

    //Статусы бронирования (если нужны)
    public enum BookingStatus
    {
        /// <summary>Подтверждена</summary>
        Confirmed = 1,

        /// <summary>Ожидает подтверждения</summary>
        Pending = 2,

        /// <summary>Отменена</summary>
        Cancelled = 3,

        /// <summary>Завершена</summary>
        Completed = 4
    }

    /// <summary>
    /// Сезоны для отчетности
    /// </summary>
    public enum Season
    {
        /// <summary>Зима 2024</summary>
        Winter2024 = 1,

        /// <summary>Лето 2024</summary>
        Summer2024 = 2,

        /// <summary>Зима 2025</summary>
        Winter2025 = 3,

        /// <summary>Лето 2025</summary>
        Summer2025 = 4
    }

    /// <summary>
    /// Форматы отчетов
    /// </summary>
    public enum ReportFormat
    {
        /// <summary>Таблица в приложении</summary>
        Table = 1,

        /// <summary>Excel файл</summary>
        Excel = 2,

        /// <summary>PDF документ</summary>
        PDF = 3,

        /// <summary>Печатная форма</summary>
        Print = 4
    }

    /// <summary>
    /// Методы сортировки отчетов
    /// </summary>
    public enum SortBy
    {
        /// <summary>По типу снаряжения</summary>
        EquipmentType = 1,

        /// <summary>По стоимости аренды</summary>
        RentalPrice = 2,

        /// <summary>По дате выдачи</summary>
        StartDate = 3,

        /// <summary>По клиенту</summary>
        ClientName = 4,

        /// <summary>По статусу</summary>
        Status = 5
    }

    /// <summary>
    /// Статусы платежей
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>Оплачено</summary>
        Paid = 1,

        /// <summary>Ожидает оплаты</summary>
        Pending = 2,

        /// <summary>Частично оплачено</summary>
        Partial = 3,

        /// <summary>Возврат средств</summary>
        Refunded = 4,

        /// <summary>Просрочен платеж</summary>
        Overdue = 5
    }

    /// <summary>
    /// Методы оплаты
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>Наличные</summary>
        Cash = 1,

        /// <summary>Банковская карта</summary>
        Card = 2,

        /// <summary>Безналичный расчет</summary>
        BankTransfer = 3,

        /// <summary>Онлайн платеж</summary>
        Online = 4
    }
}