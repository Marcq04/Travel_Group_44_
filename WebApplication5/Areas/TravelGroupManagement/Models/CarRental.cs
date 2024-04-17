using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class CarRental
    {
        public int CarRentalId { get; set; }
        [Required]
        public string Model { get; set; }
        public string Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime RentalDateMin { get; set; }
        [DataType(DataType.Date)]
        public DateTime RentalDateMax { get; set; }
        public decimal PricePerDay { get; set; }
        public string? ImagePath { get; set; }

        public List<CarRentalBooking>? Bookings { get; set; }
    }
}
