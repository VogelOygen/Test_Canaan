using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
                using var reader = new StreamReader(_csvFilePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    MissingFieldFound = null,
                    BadDataFound = null
                });

                var records = new List<TourBooking>();
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    try
                    {
                        records.Add(new TourBooking
                        {
                            DealNumber = csv.GetField<string>(0),
                            Customer = csv.GetField<string>(5),
                            Tourists = csv.GetField<string>(6),
                            Nights = int.TryParse(csv.GetField<string>(7), out var nights) ? nights : 0,
                            HotelCategory = csv.GetField<string>(8),
                            TourType = csv.GetField<string>(12),
                            ProgramName = csv.GetField<string>(15),
                            ProgramVariant = csv.GetField<string>(16),
                            Status = csv.GetField<string>(22)
                        });
                    }
                    catch
                    {
                    }
                }

                return records;
            }
            catch
            {
                return Enumerable.Empty<TourBooking>();
            }
        });
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
            using var writer = new StreamWriter(_csvFilePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(bookings);
        });
    }
}