using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string DepartureLocation { get; set; }
        public string DestinationLocation { get; set; }
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }
        public int NumberOfPassengers { get; set; }
        public decimal Price { get; set; }
    }
}
