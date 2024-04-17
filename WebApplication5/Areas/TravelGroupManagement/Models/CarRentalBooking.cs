using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class CarRentalBooking
    {
        [Key]
        public int CBookingId { get; set; }
        [Required]
        public int CarRentalId { get; set; }
        [Required]
        public string RenterName { get; set; }

        [Required]
        [EmailAddress]
        public string RenterEmail { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime RentalStartDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime RentalEndDate { get; set; }

        public CarRental? Car { get; set; }
    }
}
