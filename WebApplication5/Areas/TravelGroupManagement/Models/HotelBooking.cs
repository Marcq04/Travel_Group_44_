using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class HotelBooking
    {
        [Key]
        public int HBookingId { get; set; }

        [Required]
        public int HotelId { get; set; }

        [Required]
        public string Guestname { get; set; }

        [Required]
        [EmailAddress]
        public string GuestEmail { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime CheckInDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime CheckOutDate { get; set; }

        public Hotel? Hotel { get; set; }
    }
}
