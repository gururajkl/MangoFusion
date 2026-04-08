using MangoFusion.API.Data;
using MangoFusion.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MangoFusion.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApiResponse _apiResponse;

    public MenuItemController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _apiResponse = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        _apiResponse.Result = await _dbContext.MenuItems.ToListAsync();
        _apiResponse.StatusCode = HttpStatusCode.OK;
        return Ok(_apiResponse);
    }
}
