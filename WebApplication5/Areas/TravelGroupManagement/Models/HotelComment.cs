using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class HotelComment
    {
        public int HotelCommentId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed more than 500 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        // Foreign Key
        public int HotelId { get; set; }

        // Navigation Property
        public Hotel? Hotel { get; set; }
    }
}
