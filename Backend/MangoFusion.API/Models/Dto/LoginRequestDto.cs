namespace MangoFusion.API.Models.Dto;

public class LoginRequestDto
{
    public string UserEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
