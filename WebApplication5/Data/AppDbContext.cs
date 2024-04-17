using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Areas.TravelGroupManagement.Models;

namespace WebApplication5.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<CarRental> Cars { get; set;}
        public DbSet<FlightBooking> FlightBookings { get; set; }
        public DbSet<HotelBooking> HotelBookings { get; set; }
        public DbSet<CarRentalBooking> CarRentalBookings { get; set; }
        public DbSet<FlightComment> FlightComments { get; set; }
        public DbSet<HotelComment> HotelComments { get; set; }
        public DbSet<CarRentalComment> CarRentalComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Role"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable(name: "UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable(name: "UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable(name: "UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable(name: "RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable(name: "UserTokens"); });
        }

    }
}
