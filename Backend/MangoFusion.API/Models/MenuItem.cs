using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models;

public class MenuItem
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string SpecialTag { get; set; } = string.Empty;
    [Required]
    public double Price { get; set; }
    [Required]
    public string Image { get; set; } = string.Empty;
}
