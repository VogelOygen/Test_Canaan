using System.Collections.Generic;
using System.Threading.Tasks;
using Test_Canaan.Models;

namespace Test_Canaan.Services;

public interface ITourBookingService
{
    Task<IEnumerable<TourBooking>> GetAllBookingsAsync();
    Task<TourBooking?> GetBookingByDealNumberAsync(string dealNumber);
    Task SaveBookingsAsync(IEnumerable<TourBooking> bookings);
}