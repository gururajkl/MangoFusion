using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models.Dto;

public class OrderDetailsUpdateDto
{
    [Required]
    public int OrderDetailsId { get; set; }
    [Required]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }
}
