using System;

namespace Test_Canaan.Models;

public class HotelStay
{
    public string? HotelName { get; set; }
    public string? Category { get; set; }
    public int Nights { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public decimal Price { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
}