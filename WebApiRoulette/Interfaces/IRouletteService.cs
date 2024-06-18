using Azure.Core;
using System.Data;
using WebApiRoulette.Helpers;
using WebApiRoulette.Models;

namespace WebApiRoulette.Interfaces
{
    public interface IRouletteService
    {
        Task<Roulette> SpinRoulette(string userName, string betType, string BetValue, double betAmount);
    }
}
