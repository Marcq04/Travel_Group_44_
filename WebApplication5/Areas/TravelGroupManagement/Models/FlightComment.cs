using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Areas.TravelGroupManagement.Models
{
    public class FlightComment
    {
        public int FlightCommentId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed more than 500 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        // Foreign Key
        public int FlightId { get; set; }

        // Navigation Property
        public Flight? Flight { get; set; }
    }
}
