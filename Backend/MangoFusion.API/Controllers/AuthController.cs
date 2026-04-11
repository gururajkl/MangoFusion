using MangoFusion.API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    [HttpGet]
    [Authorize]
    public ActionResult<string> GetSomething()
    {
        return "You are authorized";
    }

    [HttpGet("{someValue:int}")]
    [Authorize(Roles = StaticDetails.RoleAdmin)]
    public ActionResult<string> GetSomething(int someValue)
    {
        return $"You are authorized with admin role {someValue}";
    }
}
