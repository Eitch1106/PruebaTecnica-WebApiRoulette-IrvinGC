using System;
using WebApiRoulette.Helpers;


namespace WebApiRoulette.Models
{
    public class Roulette
    {
        public int? Number { get; set; }
        public NumberType? NumberType { get; set; }
        public string? Color { get; set; }
        public bool Win { get; set; }
        public double TotalAmount { get; set; }
        public double Winnings { get; set; }
    }
}
