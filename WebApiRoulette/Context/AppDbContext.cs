using Microsoft.EntityFrameworkCore;
using WebApiRoulette.Models;
namespace WebApiRoulette.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
