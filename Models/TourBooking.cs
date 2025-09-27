using System;
using System.Collections.Generic;

namespace Test_Canaan.Models;

public class TourBooking
{
    public string? DealNumber { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public string? ArrivalFlight { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? DepartureFlight { get; set; }
    public string? Tourists { get; set; }
    public int TouristCount { get; set; }
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
    public decimal TotalDebt { get; set; }
    public string? Commercial { get; set; }
    public string? Status { get; set; }

    public List<HotelStay> TashkentHotels { get; set; } = new();
    public List<HotelStay> SamarkandHotels { get; set; } = new();
    public List<HotelStay> BukharaHotels { get; set; } = new();
    public List<HotelStay> KhivaHotels { get; set; } = new();

    public List<ServiceItem> AdditionalServices { get; set; } = new();
    public List<TransportationInfo> Transportation { get; set; } = new();
    public List<TransferInfo> Transfers { get; set; } = new();
    public List<TravelTicket> Tickets { get; set; } = new();

    public List<GuideService> Guides { get; set; } = new();
    public List<TicketService> AttractionTickets { get; set; } = new();
    public List<RestaurantService> Restaurants { get; set; } = new();
}