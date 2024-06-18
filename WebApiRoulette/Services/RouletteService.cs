using WebApiRoulette.Interfaces;
using WebApiRoulette.Helpers;
using WebApiRoulette.Models;
using System.Data;
using WebApiRoulette.Context;
using System.Drawing;
using Microsoft.EntityFrameworkCore;


namespace WebApiRoulette.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly AppDbContext _context;

        public RouletteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Roulette> SpinRoulette(string userName, string betType, string betValue, double betAmount)
        {
            Random random = new Random();
            int numero = random.Next(0, 37);
            int randomIndex = random.Next(2);
            List<string> colors = new List<string>() { "Rojo", "Negro" };
            string randomColor = colors[randomIndex];

            NumberType numberType = isOddorEven(numero) ? NumberType.Even : NumberType.Odd;

            var (win, winnings) = DetermineWin(betType, betValue, numero, randomColor, betAmount);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == userName);
            if (user != null)
            {
                if (win)
                {
                    user.Saldo += winnings; // Sumar las ganancias al saldo si gana
                }
                else
                {
                    user.Saldo -= betAmount; // Restar la apuesta si pierde
                }
            }

            Roulette result = new Roulette
            {
                Number = numero,
                NumberType = numberType,
                Color = randomColor,
                Win = win,
                TotalAmount = user.Saldo,
                Winnings = win ? winnings : -betAmount
            };
            return result;
        }

        public bool isOddorEven(int number)
        {
            return number % 2 == 0;
        
        }

        private (bool Win, double Winnings) DetermineWin(string betType, string betValue, int randomNumber, string color, double betAmount)
        {
            bool win = false;
            double winnings = 0;

            switch (betType)
            {
                case "Color":
                    if (color.Equals(betValue, StringComparison.OrdinalIgnoreCase))
                    {
                        win = true;
                        winnings = betAmount / 2; // Gana la mitad del monto apostado
                    }
                    break;

                case "ParityColor":
                    //"Odd Rojo", "Even Negro"
                    var isEven = randomNumber % 2 == 0;
                    var parity = isEven ? "Even" : "Odd";
                    string[] betValues = betValue.Split(' ');
                    if (betValues.Length == 2)
                    {
                        if (parity.Equals(betValues[0], StringComparison.OrdinalIgnoreCase) && color.Equals(betValues[1], StringComparison.OrdinalIgnoreCase))
                        {
                            win = true;
                            winnings = betAmount; // Gana el mismo monto apostado
                        }
                    }
                    break;

                case "NumberColor":
                    //"12 Negro"
                    string[] betNumberColor = betValue.Split(' ');
                    if (betNumberColor.Length == 2)
                    {
                        if (int.TryParse(betNumberColor[0], out int betNumber) && betNumber == randomNumber && color.Equals(betNumberColor[1], StringComparison.OrdinalIgnoreCase))
                        {
                            win = true;
                            winnings = betAmount * 3; // Gana el triple del monto apostado
                        }
                    }
                    break;
            }
            return (win, winnings);
        }

    }
}
