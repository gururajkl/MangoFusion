using System.ComponentModel.DataAnnotations;

namespace MangoFusion.API.Models.Dto;

public class RegisterRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
