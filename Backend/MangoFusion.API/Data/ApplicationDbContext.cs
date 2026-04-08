using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangoFusion.API.Data;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options) { }
