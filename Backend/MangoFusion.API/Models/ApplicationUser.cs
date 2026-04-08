using Microsoft.AspNetCore.Identity;

namespace MangoFusion.API.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
