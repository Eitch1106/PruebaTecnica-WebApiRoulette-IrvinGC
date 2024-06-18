namespace WebApiRoulette.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Saldo { get; set; }
    }
}
