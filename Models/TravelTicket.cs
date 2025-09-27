using System;

namespace Test_Canaan.Models;

public class TravelTicket
{
    public string? Type { get; set; }
    public string? Provider { get; set; }
    public string? FlightNumber { get; set; }
    public DateTime? Date { get; set; }
    public string? Route { get; set; }
    public decimal Cost { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
}