using System;

namespace Test_Canaan.Models;

public class RestaurantService
{
    public string? City { get; set; }
    public string? RestaurantName { get; set; }
    public DateTime? Date { get; set; }
    public decimal Cost { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
}