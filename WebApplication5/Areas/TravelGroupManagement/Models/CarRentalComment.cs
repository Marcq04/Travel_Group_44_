using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class CarRentalComment
    {
        public int CarRentalCommentId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed more than 500 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        // Foreign Key
        public int CarRentalId { get; set; }

        // Navigation Property
        public CarRental? CarRental { get; set; }
    }
}
