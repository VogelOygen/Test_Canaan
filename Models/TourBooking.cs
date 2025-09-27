using System;

namespace Test_Canaan.Models;

public class TourBooking
{
    public string? DealNumber { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public string? ArrivalFlight { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureFlight { get; set; }
    public string? Tourists { get; set; }
    public int Nights { get; set; }
    public string? HotelCategory { get; set; }
    public DateTime? TourStartDate { get; set; }
    public DateTime? TourEndDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? TourType { get; set; }
    public string? Customer { get; set; }
    public string? PromoCode { get; set; }
    public string? ProgramName { get; set; }
    public string? ProgramVariant { get; set; }
    public decimal SaleAmount { get; set; }
    public decimal AdditionalFees { get; set; }
    public decimal Payment { get; set; }
    public decimal Debt { get; set; }
    public string? Status { get; set; }
    public string? TashkentHotel { get; set; }
    public string? TashkentCategory { get; set; }
    public string? SamarkandHotel { get; set; }
    public string? SamarkandCategory { get; set; }
    public string? BukharaHotel { get; set; }
    public string? BukharaCategory { get; set; }
    public string? KhivaHotel { get; set; }
    public string? KhivaCategory { get; set; }
}