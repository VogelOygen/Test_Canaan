using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Test_Canaan.Models;

namespace Test_Canaan.Services;

public class CsvTourBookingService : ITourBookingService
{
    private readonly string _csvFilePath;

    public CsvTourBookingService(string csvFilePath)
    {
        _csvFilePath = csvFilePath ?? throw new ArgumentNullException(nameof(csvFilePath));
    }

    public async Task<IEnumerable<TourBooking>> GetAllBookingsAsync()
    {
        return await Task.Run(() =>
        {
            if (!File.Exists(_csvFilePath))
                return Enumerable.Empty<TourBooking>();

            try
            {
                using var reader = new StreamReader(_csvFilePath, Encoding.UTF8);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    BadDataFound = null,
                    TrimOptions = TrimOptions.Trim,
                    IgnoreBlankLines = true,
                    HeaderValidated = null
                };

                using var csv = new CsvReader(reader, config);
                csv.Context.RegisterClassMap<TourBookingMap>();

                var records = new List<TourBooking>();

                while (csv.Read())
                {
                    try
                    {
                        var booking = csv.GetRecord<TourBooking>();
                        if (booking != null && !string.IsNullOrWhiteSpace(booking.DealNumber))
                        {
                            ParseHotels(csv, booking);
                            ParseServices(csv, booking);
                            ParseTransportation(csv, booking);
                            ParseGuides(csv, booking);
                            ParseTickets(csv, booking);
                            ParseRestaurants(csv, booking);

                            records.Add(booking);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error parsing row: {ex.Message}");
                    }
                }

                return records;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading CSV: {ex.Message}");
                return Enumerable.Empty<TourBooking>();
            }
        });
    }

    private void ParseHotels(CsvReader csv, TourBooking booking)
    {
        ParseTashkentHotels(csv, booking);
        ParseSamarkandHotels(csv, booking);
        ParseBukharaHotels(csv, booking);
        ParseKhivaHotels(csv, booking);
    }

    private void ParseTashkentHotels(CsvReader csv, TourBooking booking)
    {
        AddHotelIfValid(csv, booking.TashkentHotels, 23, 24, 25, 26, 27, 28, 29, 30);
        AddHotelIfValid(csv, booking.TashkentHotels, 31, -1, 32, 33, 34, 35, 36, 37);
    }

    private void ParseSamarkandHotels(CsvReader csv, TourBooking booking)
    {
        AddHotelIfValid(csv, booking.SamarkandHotels, 42, 43, 44, 45, 46, 47, 48, 49);
    }

    private void ParseBukharaHotels(CsvReader csv, TourBooking booking)
    {
        AddHotelIfValid(csv, booking.BukharaHotels, 50, 51, 52, 53, 54, 55, 56, 57);
    }

    private void ParseKhivaHotels(CsvReader csv, TourBooking booking)
    {
        AddHotelIfValid(csv, booking.KhivaHotels, 58, 59, 60, 61, 62, 63, 64, 65);
    }

    private void AddHotelIfValid(CsvReader csv, List<HotelStay> hotels, int nameIdx, int categoryIdx, int nightsIdx, int checkInIdx, int checkOutIdx, int priceIdx, int debtIdx, int paidIdx)
    {
        var name = GetFieldSafe(csv, nameIdx);
        if (string.IsNullOrWhiteSpace(name)) return;

        hotels.Add(new HotelStay
        {
            HotelName = name,
            Category = categoryIdx >= 0 ? GetFieldSafe(csv, categoryIdx) : null,
            Nights = GetIntField(csv, nightsIdx),
            CheckInDate = GetDateField(csv, checkInIdx),
            CheckOutDate = GetDateField(csv, checkOutIdx),
            Price = GetDecimalField(csv, priceIdx),
            Debt = GetDecimalField(csv, debtIdx),
            Paid = GetDecimalField(csv, paidIdx)
        });
    }

    private void ParseServices(CsvReader csv, TourBooking booking)
    {
        var serviceName = GetFieldSafe(csv, 66);
        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            booking.AdditionalServices.Add(new ServiceItem
            {
                ServiceName = serviceName,
                Cost = GetDecimalField(csv, 67),
                Debt = GetDecimalField(csv, 68),
                Paid = GetDecimalField(csv, 69)
            });
        }
    }

    private void ParseTransportation(CsvReader csv, TourBooking booking)
    {
        var provider = GetFieldSafe(csv, 70);
        if (!string.IsNullOrWhiteSpace(provider))
        {
            booking.Transportation.Add(new TransportationInfo
            {
                Provider = provider,
                DriverName = GetFieldSafe(csv, 71),
                DriverContact = GetFieldSafe(csv, 72),
                Note = GetFieldSafe(csv, 73),
                Cost = GetDecimalField(csv, 74),
                Debt = GetDecimalField(csv, 75),
                Paid = GetDecimalField(csv, 76)
            });
        }
    }

    private void ParseGuides(CsvReader csv, TourBooking booking)
    {
        ParseGuideService(csv, booking, "Tashkent", 96, 97, 98, 99, 100);
        ParseGuideService(csv, booking, "Samarkand", 104, 105, 106, 107, 108);
        ParseGuideService(csv, booking, "Bukhara", 112, 113, 114, 115, 116);
        ParseGuideService(csv, booking, "Khiva", 120, 121, 122, 123, 124);
    }

    private void ParseGuideService(CsvReader csv, TourBooking booking, string city, int guideIdx, int dateIdx, int costIdx, int debtIdx, int paidIdx)
    {
        var guideName = GetFieldSafe(csv, guideIdx);
        if (!string.IsNullOrWhiteSpace(guideName))
        {
            booking.Guides.Add(new GuideService
            {
                City = city,
                GuideName = guideName,
                Date = GetDateField(csv, dateIdx),
                Cost = GetDecimalField(csv, costIdx),
                Debt = GetDecimalField(csv, debtIdx),
                Paid = GetDecimalField(csv, paidIdx)
            });
        }
    }

    private void ParseTickets(CsvReader csv, TourBooking booking)
    {
        ParseTicketService(csv, booking, "Tashkent", 101, 102, 103);
        ParseTicketService(csv, booking, "Samarkand", 109, 110, 111);
        ParseTicketService(csv, booking, "Bukhara", 117, 118, 119);
        ParseTicketService(csv, booking, "Khiva", 125, 126, 127);
    }

    private void ParseTicketService(CsvReader csv, TourBooking booking, string city, int costIdx, int debtIdx, int paidIdx)
    {
        var cost = GetDecimalField(csv, costIdx);
        if (cost > 0)
        {
            booking.AttractionTickets.Add(new TicketService
            {
                City = city,
                Cost = cost,
                Debt = GetDecimalField(csv, debtIdx),
                Paid = GetDecimalField(csv, paidIdx)
            });
        }
    }

    private void ParseRestaurants(CsvReader csv, TourBooking booking)
    {
        ParseRestaurantService(csv, booking, "Tashkent", 128, 129, 130, 131, 132);
        ParseRestaurantService(csv, booking, "Tashkent", 133, 134, 135, 136, 137);
        ParseRestaurantService(csv, booking, "Samarkand", 138, 139, 140, 141, 142);
        ParseRestaurantService(csv, booking, "Samarkand", 143, 144, 145, 146, 147);
        ParseRestaurantService(csv, booking, "Bukhara", 148, 149, 150, 151, 152);
        ParseRestaurantService(csv, booking, "Bukhara", 153, 154, 155, 156, 157);
        ParseRestaurantService(csv, booking, "Khiva", 158, 159, 160, 161, 162);
        ParseRestaurantService(csv, booking, "Khiva", 163, 164, 165, 166, 167);
    }

    private void ParseRestaurantService(CsvReader csv, TourBooking booking, string city, int nameIdx, int dateIdx, int costIdx, int debtIdx, int paidIdx)
    {
        var name = GetFieldSafe(csv, nameIdx);
        if (!string.IsNullOrWhiteSpace(name))
        {
            booking.Restaurants.Add(new RestaurantService
            {
                City = city,
                RestaurantName = name,
                Date = GetDateField(csv, dateIdx),
                Cost = GetDecimalField(csv, costIdx),
                Debt = GetDecimalField(csv, debtIdx),
                Paid = GetDecimalField(csv, paidIdx)
            });
        }
    }

    private string? GetFieldSafe(CsvReader csv, int index)
    {
        try
        {
            return csv.GetField<string>(index);
        }
        catch
        {
            return null;
        }
    }

    private int GetIntField(CsvReader csv, int index)
    {
        var value = GetFieldSafe(csv, index);
        if (string.IsNullOrWhiteSpace(value)) return 0;

        value = value.Replace(" ", "").Replace(",", ".");
        return int.TryParse(value, out var result) ? result : 0;
    }

    private decimal GetDecimalField(CsvReader csv, int index)
    {
        var value = GetFieldSafe(csv, index);
        if (string.IsNullOrWhiteSpace(value)) return 0m;

        value = value.Replace(" ", "").Replace(",", ".");
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
    }

    private DateTime? GetDateField(CsvReader csv, int index)
    {
        var value = GetFieldSafe(csv, index);
        if (string.IsNullOrWhiteSpace(value)) return null;

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return date;

        if (DateTime.TryParse(value, new CultureInfo("ru-RU"), DateTimeStyles.None, out var dateRu))
            return dateRu;

        return null;
    }

    public async Task<TourBooking?> GetBookingByDealNumberAsync(string dealNumber)
    {
        var bookings = await GetAllBookingsAsync();
        return bookings.FirstOrDefault(b => b.DealNumber == dealNumber);
    }

    public async Task SaveBookingsAsync(IEnumerable<TourBooking> bookings)
    {
        await Task.Run(() =>
        {
            using var writer = new StreamWriter(_csvFilePath, false, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(bookings);
        });
    }
}