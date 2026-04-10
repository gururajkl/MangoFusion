using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models.Dto;

public class MenuItemCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? SpecialTag { get; set; }
    [Required]
    public double Price { get; set; }
    public IFormFile File { get; set; } = default!;
}
