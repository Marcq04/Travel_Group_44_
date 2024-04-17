using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class FlightBooking
    {
        [Key]
        public int FBookingId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        public string PassengerName { get; set; }

        [Required]
        [EmailAddress]
        public string PassengerEmail { get; set; }

        public Flight? Flight { get; set; }
    }
}
