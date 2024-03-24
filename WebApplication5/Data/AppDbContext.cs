using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.TravelGroupManagement.Models;

namespace WebApplication5.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<CarRental> Cars { get; set;}
    }
}
