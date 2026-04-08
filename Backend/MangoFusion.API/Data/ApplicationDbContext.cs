using MangoFusion.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangoFusion.API.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options) { }
