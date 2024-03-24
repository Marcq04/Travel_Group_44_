using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public double Rating { get; set; }
        public decimal PricePerNight { get; set; }
        public string? ImagePath { get; set; }
    }
}
