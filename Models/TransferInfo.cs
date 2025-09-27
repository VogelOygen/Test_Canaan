using System;

namespace Test_Canaan.Models;

public class TransferInfo
{
    public string? Provider { get; set; }
    public string? Type { get; set; }
    public string? Direction { get; set; }
    public DateTime? Date { get; set; }
    public decimal Cost { get; set; }
    public decimal Debt { get; set; }
    public decimal Paid { get; set; }
}