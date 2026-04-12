using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models.Dto;

public class OrderDetailsUpdateDto
{
    [Required]
    public int MenuItemId { get; set; }
    [Required]
    public int OrderDetailsId { get; set; }
    [Required]
    public int Rating { get; set; }
}
