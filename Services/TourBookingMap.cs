using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Test_Canaan.Models;

namespace Test_Canaan.Services;

public class TourBookingMap : ClassMap<TourBooking>
{
    public TourBookingMap()
    {
        Map(m => m.DealNumber).Index(0).Name("ФИ номер сделки");
        Map(m => m.ArrivalDate).Index(1).Name("Дата прилета").TypeConverter<DateTimeConverter>();
        Map(m => m.ArrivalFlight).Index(2).Name("рейс прилета");
        Map(m => m.DepartureDate).Index(3).Name("Дата вылета").TypeConverter<DateTimeConverter>();
        Map(m => m.DepartureFlight).Index(4).Name("рейс вылета,");
        Map(m => m.Tourists).Index(6).Name("Туристов\r\n (ADT-CHD-INF)");
        Map(m => m.TouristCount).Convert(row => ParseTouristCount(row.Row.GetField(6)));
        Map(m => m.Nights).Index(7).Name("Ночей").TypeConverter<Int32Converter>();
        Map(m => m.HotelCategory).Index(8).Name("Категория отеля");
        Map(m => m.TourStartDate).Index(9).Name("Дата начала тура").TypeConverter<DateTimeConverter>();
        Map(m => m.TourEndDate).Index(10).Name("Дата окончания тура").TypeConverter<DateTimeConverter>();
        Map(m => m.RequestDate).Index(11).Name("Дата Заявка ").TypeConverter<DateTimeConverter>();
        Map(m => m.TourType).Index(12).Name("Индивидуальный/ Групповой");
        Map(m => m.Customer).Index(13).Name("Заказчик");
        Map(m => m.PromoCode).Index(14).Name("Промо код");
        Map(m => m.ProgramName).Index(15).Name("Название программы");
        Map(m => m.ProgramVariant).Index(16).Name("Вариант программы");
        Map(m => m.SaleAmount).Index(17).Name("Сумма продажи (Тур)").TypeConverter<DecimalConverter>();
        Map(m => m.AdditionalFees).Index(18).Name("Сумма продажи (Доплаты)").TypeConverter<DecimalConverter>();
        Map(m => m.Payment).Index(19).Name("Оплата").TypeConverter<DecimalConverter>();
        Map(m => m.Debt).Index(20).Name("долг").TypeConverter<DecimalConverter>();
        Map(m => m.Commercial).Index(21).Name("Комерц");
        Map(m => m.Status).Index(22).Name("Статус заявки");
    }

    private static int ParseTouristCount(string? tourists)
    {
        if (string.IsNullOrWhiteSpace(tourists)) return 0;

        var count = 0;
        var parts = tourists.Split(new[] { '\r', '\n', ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            if (int.TryParse(part, out var num))
            {
                count += num;
            }
        }

        return count > 0 ? count : 1;
    }
}

public class DateTimeConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return date;

        if (DateTime.TryParse(text, new CultureInfo("ru-RU"), DateTimeStyles.None, out var dateRu))
            return dateRu;

        return null;
    }
}

public class DecimalConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0m;

        text = text.Replace(" ", "").Replace(",", ".");

        if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return value;

        return 0m;
    }
}

public class Int32Converter : DefaultTypeConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        if (int.TryParse(text.Replace(" ", ""), out var value))
            return value;

        return 0;
    }
}