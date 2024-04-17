using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }
        public int NumberOfPassengers { get; set; }
        public decimal Price { get; set; }

        public List<FlightBooking>? Bookings { get; set; }
    }
}
